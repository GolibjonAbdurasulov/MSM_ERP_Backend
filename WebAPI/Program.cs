using System.Text;
using System.Text.Json.Serialization;
using DataAccess.DataContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using Services.Services;
using WebAPI.Extension;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Services.AddDbContextPool<AppDbContext>(optionsBuilder =>
{
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString"));
    optionsBuilder.UseLazyLoadingProxies();
    //optionsBuilder.UseChangeTrackingProxies();
});



builder
    .Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        string key = builder.Configuration.GetSection("Jwt")["SecurityKey"];
        string issuer = builder.Configuration.GetSection("Jwt")["Issuer"];
        string audience = builder.Configuration.GetSection("Jwt")["Audience"];
        int expiresInMinutes = builder.Configuration.GetSection("Jwt").GetValue<int>("ExpireAtInMinutes");
        
        
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ClockSkew = TimeSpan.Zero
        };
        
    });


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "https://msm.uz", Version = "v1" });

  
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy
            .AllowAnyOrigin()      
            .AllowAnyHeader()      
            .AllowAnyMethod();     
    });
});






builder.Services
    .AddAuthorization(options =>
    {
    });


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureRepositories();
builder.Services.AddMemoryCache();

// var smtpConfig = builder.Configuration.GetSection("Smtp");
//
// builder.Services.AddScoped<SmtpClient>(sp =>
// {
//     return new SmtpClient(smtpConfig["Host"], int.Parse(smtpConfig["Port"]))
//     {
//         Credentials = new NetworkCredential(smtpConfig["User"], smtpConfig["Password"]),
//         EnableSsl = true
//     };
// });

builder.Services.ConfigureServicesFromTypeAssembly<CommentService>();
builder.Services.ConfigureServicesFromTypeAssembly<UserService>();
builder.Services.ConfigureServicesFromTypeAssembly<DepartmentService>();
builder.Services.ConfigureServicesFromTypeAssembly<JobService>();

builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

//app.UseHttpsRedirection();
app.UseRouting();


app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "bakcend api");
    });
}



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();

app.Run();
