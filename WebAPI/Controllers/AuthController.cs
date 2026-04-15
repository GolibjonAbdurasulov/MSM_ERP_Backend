using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.ViewModels.AuthViewModels;
using Services.ViewModels.UserViewModels;

namespace WebAPI.Controllers;

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<UserGetViewModel> Register([FromBody] RegisterRequest request)
        {
            var user = await _authService.RegisterAsync(request);

            return user;
        }

        [HttpPost]
        public async Task<UserWithTokenViewModel?> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.AuthenticateAsync(request.Email, request.Password);
            if (user == null) return null;

            return user;
        }
        
        [HttpPost]
        [Authorize]
        public async Task<bool?> LogOut( long id)
        {
            return await _authService.LogOut(id);
        }
    }
