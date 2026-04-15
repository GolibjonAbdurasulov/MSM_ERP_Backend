using Services.ViewModels.AuthViewModels;
using Services.ViewModels.UserViewModels;

namespace Services.Interfaces;

public interface IAuthService
{
    Task<UserGetViewModel> RegisterAsync(RegisterRequest request);
    Task<UserWithTokenViewModel> AuthenticateAsync(string email, string password);
    Task<bool> LogOut(long id);
}