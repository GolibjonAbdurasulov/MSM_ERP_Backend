namespace Services.ViewModels.UserViewModels;

public class UserWithPasswordViewModel : UserGetViewModel
{
    public byte[] PasswordHash { get; set; } 
    public byte[] PasswordSalt { get; set; } 
}