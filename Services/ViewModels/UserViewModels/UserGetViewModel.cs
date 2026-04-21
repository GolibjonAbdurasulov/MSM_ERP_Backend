using DataAccess.Enums;

namespace Services.ViewModels.UserViewModels;

public class UserGetViewModel
{
    public long Id { get; set; }
    public string FirstName { get; set; } 
    public string LastName { get; set; } 
    public string Login { get; set; }
    public UserRole Role { get; set; }
    public long DepartmentId { get; set; }
    public DateTime LastLoginDate { get; set; }
    public bool IsSigned { get; set; }
}