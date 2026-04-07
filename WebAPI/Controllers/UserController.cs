using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.ViewModels.UserViewModels;
using WebAPI.Common;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : Controller
{
    private IUserService _userService;

    public UserController(IUserService userService)
    {
        this._userService = userService;
    }
    
    [HttpPost]
    public async Task<ResponseModelBase> CreateUser(UserCreationViewModel model)
    {
        var user=await _userService.CreateUser(model);
        return (user,200);
    }
    
    [HttpPut]
    public async Task<ResponseModelBase> UpdateUser(UserUpdateViewModel model)
    {
        var user=await _userService.UpdateUser(model);
        return (user,200);
    }
    
    [HttpDelete]
    public async Task<ResponseModelBase> DeleteUser(long id)
    {
        var res=await _userService.DeleteUser(id);
        return (res,200);
    }
    
    
    [HttpGet]
    public async Task<ResponseModelBase> GetUserById(long id)
    {
        var user=await _userService.GetUserById(id);
        return (user,200);
    }
    
    
    [HttpGet]
    public async Task<ResponseModelBase> GetUsersByDepartmentId(long departmentId)
    {
        var users=await _userService.GetAllUsersByDepartmentId(departmentId);
        
        return (users,200);
    }
    
    [HttpGet]
    public async Task<ResponseModelBase> GetAllUsers()
    {
        var users=await _userService.GetAllUsers();
        
        return (users,200);
    }
}