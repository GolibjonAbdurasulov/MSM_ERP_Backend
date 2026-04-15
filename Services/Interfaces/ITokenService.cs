using Services.ViewModels.TokenViewModels;

namespace Services.Interfaces;

public interface ITokenService
{
    public Task<TokenResponse> GenerateTokenAsync(string email);
}