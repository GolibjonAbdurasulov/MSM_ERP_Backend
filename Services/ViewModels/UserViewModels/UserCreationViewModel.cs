using DataAccess.Enums;

namespace Services.ViewModels.UserViewModels;

public class UserCreationViewModel
{
    public string FirstName { get; set; } 
    public string LastName { get; set; } 
    public string Login { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public long DepartmentId { get; set; }
}