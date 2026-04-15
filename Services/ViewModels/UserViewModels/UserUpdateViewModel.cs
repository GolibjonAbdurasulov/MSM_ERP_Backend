using DataAccess.Enums;

namespace Services.ViewModels.UserViewModels;

public class UserUpdateViewModel
{
    public long Id { get; set; }
    public string FirstName { get; set; } 
    public string LastName { get; set; } 
    public string Email { get; set; }
    public UserRole Role { get; set; }
    public long DepartmentId { get; set; }
}