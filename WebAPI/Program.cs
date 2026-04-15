using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using Services.Services;
using Services.Settings;
using WebAPI.Middlewares;
using Core.Hubs;
using DataAccess.DataContext;
using WebAPI.Extension;

var builder = WebApplication.CreateBuilder(args);

// Load configuration files
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile(
        $"appsettings.{builder.Environment.EnvironmentName}.json",
        optional: true,
        reloadOnChange: true
    );

// Configure DbContext with PostgreSQL
builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString"));
    options.UseLazyLoadingProxies();
});

// JWT Settings
var tokenSettingsSection = builder.Configuration.GetSection("TokenSettings");
builder.Services.Configure<TokenSettings>(tokenSettingsSection);

var jwtKey = tokenSettingsSection["JwtKey"];
var jwtIssuer = tokenSettingsSection["Issuer"];
var jwtAudience = tokenSettingsSection["Audience"];

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ClockSkew = TimeSpan.Zero
    };

    // SignalR JWT support
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/jobHub"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});
// CORS — eski kod o'rniga shu:
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        var origins = builder.Configuration
            .GetSection("AllowedOrigins")
            .Get<string[]>();

        if (origins != null && origins.Length > 0)
            policy.WithOrigins(origins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        else
            policy.WithOrigins("http://localhost:8090")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
    });
});

// SignalR
builder.Services.AddSignalR();

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MSM API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Bearer token kiriting. Misol: Bearer eyJhbGciOi...",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme }
            },
            Array.Empty<string>()
        }
    });
});

// Services
builder.Services.ConfigureRepositories();
builder.Services.AddMemoryCache();
builder.Services.ConfigureServicesFromTypeAssembly<CommentService>();
builder.Services.ConfigureServicesFromTypeAssembly<UserService>();
builder.Services.ConfigureServicesFromTypeAssembly<DepartmentService>();
builder.Services.ConfigureServicesFromTypeAssembly<JobService>();
builder.Services.ConfigureServicesFromTypeAssembly<TokenService>();
builder.Services.ConfigureServicesFromTypeAssembly<AuthService>();

// Middleware
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

var app = builder.Build();

// Enable Npgsql dynamic JSON mapping
NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();

// Global exception middleware
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "MSM API v1"); });
}

// Controllers
app.MapControllers();

// SignalR Hub
app.MapHub<JobHub>("/jobHub");

if (args.Contains("--migrate"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    Console.WriteLine("Migration muvaffaqiyatli!");
    return;
}

app.Run();
