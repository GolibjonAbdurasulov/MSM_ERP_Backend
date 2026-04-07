using DataAccess.Entities;
using Services.ViewModels.UserViewModels;

namespace Services.Interfaces;

public interface IUserService
{
    public Task<UserGetViewModel> CreateUser(UserCreationViewModel model);
    public Task<UserGetViewModel> UpdateUser(UserUpdateViewModel model);
    public Task<bool> DeleteUser(long id);
    public Task<List<UserGetViewModel>> GetAllUsers();
    public Task<UserGetViewModel> GetUserById(long id);
    public Task<List<UserGetViewModel>> GetAllUsersByDepartmentId(long departmentId);
}