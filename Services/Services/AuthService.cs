using Core.Attributes;
using DataAccess.Exeptions;
using Services.Interfaces;
using Services.ViewModels.AuthViewModels;
using Services.ViewModels.TokenViewModels;
using Services.ViewModels.UserViewModels;

namespace Services.Services;
[Injectable]
public class AuthService : IAuthService
{
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthService(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }


        public async Task<UserGetViewModel> RegisterAsync(RegisterRequest request)
        {
            if (await _userService.UserExists(request.Email))
                throw new ArgumentException("Email already exists");

            if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters");

            if (!IsStrongPassword(request.Password, request.FirstName, request.LastName))
                throw new ArgumentException("Password is too weak");

            var userVm = new UserCreationViewModel
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Login = request.Email,
                Password = request.Password, 
                Role = request.Role,
                DepartmentId = request.DepartmentId
            };

            return await _userService.CreateUser(userVm);
        }




        public async Task<UserWithTokenViewModel> AuthenticateAsync(string email, string password)
        {
            var user = await _userService.GetUserByEmail(email);
            if (user == null) 
                throw new UnauthorizedAccessException("Invalid email or password");
            
            var tokenResponse = await _tokenService.GenerateTokenAsync(user.Login);
            if(tokenResponse == null)
                throw new NullReferenceException("Token is null on AuthService");
            
            var viewModel = new UserWithTokenViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Login = user.Login,
                LastLoginDate = user.LastLoginDate,
                Role = user.Role,
                DepartmentId = user.DepartmentId,
                Token = tokenResponse.AccessToken,
                TokenExpiresIn = tokenResponse.ExpiresIn
            };

            return viewModel;
        }

        public async Task<bool> LogOut(long id)
        {
            var user = await _userService.LogOutUser(id);
            if (user is null)
             throw new NullReferenceException("User not found on UserService");

            return true;
        }


        private static bool IsStrongPassword(string password, string firstName, string lastName)
        {
            if (password.Length < 8) return false;
            if (!password.Any(char.IsLower)) return false;
            if (!password.Any(char.IsUpper)) return false;
            if (!password.Any(char.IsDigit)) return false;
            if (!password.Any(ch => !char.IsLetterOrDigit(ch))) return false;

            var normPassword = password.Replace("'", "").ToLower();
            var normFirst = firstName.Replace("'", "").ToLower();
            var normLast = lastName.Replace("'", "").ToLower();

            if (!string.IsNullOrEmpty(firstName) && normPassword.Contains(normFirst)) return false;
            if (!string.IsNullOrEmpty(lastName) && normPassword.Contains(normLast)) return false;

            return true;
        }
    }
