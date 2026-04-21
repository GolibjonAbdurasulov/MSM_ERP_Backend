namespace Services.ViewModels.DepartmentViewModels;

public class DepartmentCreationViewModel
{
    public string DepartmentShortName { get; set; }
    public string DepartmentFullName { get; set; }
    public int DepartmentWorkersCount { get; set; } = 0;
}