using DataAccess.Entities;
using DataAccess.Enums;
using DataAccess.Repositories.UserRepositories;
using Services.Interfaces;
using Services.ViewModels.UserViewModels;

namespace Services.Services;

public class UserService : IUserService
{
    private IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserGetViewModel> CreateUser(UserCreationViewModel model)
    {
        var user = new User()
        {
            FullName = model.FullName,
            Email = model.Email,
            Password = model.Password,
            Role = model.Role,
            DepartmentId = model.DepartmentId
        };
        
        var createdUser= await _userRepository.AddAsync(user);
        var userGetViewModel = new UserGetViewModel
        {
            Id = createdUser.Id,
            FullName = createdUser.FullName,
            Email = createdUser.Email,
            Password = createdUser.Password,
            Role = createdUser.Role,
            DepartmentId = createdUser.DepartmentId
        };
        return userGetViewModel;
    }

    public async Task<UserGetViewModel> UpdateUser(UserUpdateViewModel model)
    {
        var user= await _userRepository.GetByIdAsync(model.Id);
        if (user == null)
            throw new Exception("User not found on UserService");
        user.FullName = model.FullName;
        user.Email = model.Email;
        user.Password = model.Password;
        user.Role = model.Role;
        user.DepartmentId = model.DepartmentId;
        
        var updatedUser=await _userRepository.UpdateAsync(user);
        
        var userGetViewModel = new UserGetViewModel
        {
            Id = updatedUser.Id,
            FullName = updatedUser.FullName,
            Email = updatedUser.Email,
            Password = updatedUser.Password,
            Role = updatedUser.Role,
            DepartmentId = updatedUser.DepartmentId
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

    public async Task<UserGetViewModel> GetUserById(long id)
    {
        var user= await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new Exception("User not found on UserService");    
        var userGetViewModel = new UserGetViewModel
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Password = user.Password,
            Role = user.Role,
            DepartmentId = user.DepartmentId
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
                FullName = user.FullName,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                DepartmentId = user.DepartmentId
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
                FullName = user.FullName,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                DepartmentId = user.DepartmentId
            });
        }
        return userViewModels;    }
}