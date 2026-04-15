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
        if (await UserExists(model.Email))
            throw new ArgumentException("Email already exists");
        var salt = GenerateSalt();
        var hash = ComputeHash(model.Password, salt);
        var user = new User()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Password = model.Password,
            Role = model.Role,
            DepartmentId = model.DepartmentId,
            PasswordHash = hash,
            PasswordSalt = salt,
            IsSigned = false,
            LastLoginDate = DateTime.Now,
        };
        
        var createdUser= await _userRepository.AddAsync(user);
        var userGetViewModel = new UserGetViewModel
        {
            Id = createdUser.Id,
            FirstName = createdUser.FirstName,
            LastName = createdUser.LastName,
            Email = createdUser.Email,
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
        user.Email = model.Email;
        user.Role = model.Role;
        user.DepartmentId = model.DepartmentId;
        
        var updatedUser=await _userRepository.UpdateAsync(user);
        
        var userGetViewModel = new UserGetViewModel
        {
            Id = updatedUser.Id,
            FirstName = updatedUser.FirstName,
            LastName = updatedUser.LastName,
            Email = updatedUser.Email,
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
    
    public Task<bool> CheckPasswordAsync(UserWithPasswordViewModel user, string password)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (string.IsNullOrWhiteSpace(password))
            return Task.FromResult(false);

        bool isPasswordValid = VerifyHash(
            password,
            user.PasswordHash,
            user.PasswordSalt);

        return Task.FromResult(isPasswordValid);
    }


    public async Task<UserWithPasswordViewModel> GetUserByEmail(string email)
    {
        var user= await _userRepository
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        if (user == null)
            throw new NotFoundException("Email not exists");
        var viewModel = new UserWithPasswordViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            PasswordSalt = user.PasswordSalt,
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
            Email = user.Email,
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
            Email = user.Email,
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
                Email = user.Email,
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
                Email = user.Email,
                Role = user.Role,
                DepartmentId = user.DepartmentId,
                LastLoginDate = user.LastLoginDate,
                IsSigned = user.IsSigned
            });
        }
        return userViewModels;    
    }
    

    public async Task<bool> UserExists(string email)
    {
        return await _userRepository
            .AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }
    
    
    private static byte[] GenerateSalt(int size = 16)
    {
        var salt = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }

    private static byte[] ComputeHash(string password, byte[] salt)
    {
        using var hmac = new HMACSHA512(salt);
        return hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private static bool VerifyHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        var computedHash = ComputeHash(password, storedSalt);
        return computedHash.SequenceEqual(storedHash);
    }
}