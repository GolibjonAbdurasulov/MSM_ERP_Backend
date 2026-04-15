using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Attributes;
using DataAccess.DataContext;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using Services.Settings;
using Services.ViewModels.TokenViewModels;

namespace Services.Services;

[Injectable]
public class TokenService : ITokenService
{
    private readonly TokenSettings _tokenSettings;

    public TokenService(IOptions<TokenSettings> tokenSettings)
    {
        _tokenSettings = tokenSettings.Value;
    }

    public Task<TokenResponse> GenerateTokenAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");

        var token = CreateJwtToken(email);
        return Task.FromResult(token);
    }

    private TokenResponse CreateJwtToken(string email)
    {
        var expires = DateTime.UtcNow.AddSeconds(_tokenSettings.ExpireSeconds);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.JwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: _tokenSettings.Issuer,
            audience: _tokenSettings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new TokenResponse
        {
            AccessToken = tokenString,
            ExpiresIn = _tokenSettings.ExpireSeconds,
            TokenType = "bearer"
        };
    }
}