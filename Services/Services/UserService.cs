using System.Security.Cryptography;
using System.Text;
using Core.Attributes;
using DataAccess.Entities;
using DataAccess.Enums;
using DataAccess.Exeptions;
using DataAccess.Repositories.UserRepositories;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.ViewModels.UserViewModels;

namespace Services.Services;

[Injectable]
public class UserService : IUserService
{
    private IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserGetViewModel> CreateUser(UserCreationViewModel model)
    {
        if (await UserExists(model.Login))
            throw new ArgumentException("Email already exists");
        var user = new User()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Login = model.Login,
            Password = model.Password,
            Role = model.Role,
            DepartmentId = model.DepartmentId,
            IsSigned = false,
            LastLoginDate = DateTime.Now,
        };
        
        var createdUser= await _userRepository.AddAsync(user);
        var userGetViewModel = new UserGetViewModel
        {
            Id = createdUser.Id,
            FirstName = createdUser.FirstName,
            LastName = createdUser.LastName,
            Login = createdUser.Login,
            Role = createdUser.Role,
            DepartmentId = createdUser.DepartmentId,
            LastLoginDate = createdUser.LastLoginDate,
        };
        return userGetViewModel;
    }

    public async Task<UserGetViewModel> UpdateUser(UserUpdateViewModel model)
    {
        var user= await _userRepository.GetByIdAsync(model.Id);
        if (user == null)
            throw new Exception("User not found on UserService");
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Login = model.Login;
        user.Role = model.Role;
        user.DepartmentId = model.DepartmentId;
        
        var updatedUser=await _userRepository.UpdateAsync(user);
        
        var userGetViewModel = new UserGetViewModel
        {
            Id = updatedUser.Id,
            FirstName = updatedUser.FirstName,
            LastName = updatedUser.LastName,
            Login = updatedUser.Login,
            Role = updatedUser.Role,
            DepartmentId = updatedUser.DepartmentId,
            LastLoginDate = updatedUser.LastLoginDate,
        };
        return userGetViewModel;
    }

    public async Task<bool> DeleteUser(long id)
    {
        var user= await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new Exception("User not found on UserService");

        await _userRepository.RemoveAsync(user);
        return true;
    }
    
   


    public async Task<UserGetViewModel> GetUserByEmail(string email)
    {
        var user= await _userRepository
            .FirstOrDefaultAsync(u => u.Login.ToLower() == email.ToLower());
        if (user == null)
            throw new NotFoundException("Email not exists");
        var viewModel = new UserGetViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Login = user.Login,
            LastLoginDate = user.LastLoginDate,
            Role = user.Role,
            DepartmentId = user.DepartmentId,
            IsSigned = user.IsSigned,
        };
        return viewModel;
    }

    public async Task<UserGetViewModel> LogOutUser(long id)
    {
        var user=await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new Exception("User not found on UserService");
        user.IsSigned = false;
        await _userRepository.UpdateAsync(user);
        var userGetViewModel = new UserGetViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Login = user.Login,
            Role = user.Role,
            DepartmentId = user.DepartmentId,
            LastLoginDate = user.LastLoginDate, 
            IsSigned = user.IsSigned
        };
        return userGetViewModel;
    }

    public async Task<UserGetViewModel> GetUserById(long id)
    {
        var user= await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new Exception("User not found on UserService");    
        var userGetViewModel = new UserGetViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Login = user.Login,
            Role = user.Role,
            DepartmentId = user.DepartmentId,
            LastLoginDate = user.LastLoginDate,
            IsSigned = user.IsSigned
        };
        return userGetViewModel;
    }

    public async Task<List<UserGetViewModel>> GetAllUsers()
    {
        var users = _userRepository.GetAllAsQueryable().ToList();
        var userViewModels = new List<UserGetViewModel>();
        
        foreach (User user in users)
        {
            userViewModels.Add(new UserGetViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Login = user.Login,
                Role = user.Role,
                DepartmentId = user.DepartmentId,
                LastLoginDate = user.LastLoginDate,
                IsSigned = user.IsSigned
            });
        }
        return userViewModels;
    }

    public async Task<List<UserGetViewModel>> GetAllUsersByDepartmentId(long departmentId)
    {
        var users = _userRepository.GetAllAsQueryable().Where(u=>u.DepartmentId==departmentId).ToList();
        var userViewModels = new List<UserGetViewModel>();
        
        foreach (User user in users)
        {
            userViewModels.Add(new UserGetViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Login = user.Login,
                Role = user.Role,
                DepartmentId = user.DepartmentId,
                LastLoginDate = user.LastLoginDate,
                IsSigned = user.IsSigned
            });
        }
        return userViewModels;    
    }
    

    public async Task<bool> UserExists(string login)
    {
        return await _userRepository
            .AnyAsync(u => u.Login.ToLower() == login.ToLower());
    }
}