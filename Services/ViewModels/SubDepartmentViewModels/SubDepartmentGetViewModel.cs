namespace Services.ViewModels.SubDepartmentViewModels;

public class SubDepartmentGetViewModel
{
    public long Id { get; set; }
    public string SubDepartmentShortName { get; set; } 
    public string SubDepartmentFullName { get; set; } 
    public long DepartmentId { get; set; }
    public long? SubDepartmentTelegramChatId { get; set; }
}