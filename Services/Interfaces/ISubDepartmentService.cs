using Services.ViewModels.SubDepartmentViewModels;

namespace Services.Interfaces;

public interface ISubDepartmentService
{
    public Task<SubDepartmentGetViewModel> CreateSubDepartment(SubDepartmentCreationViewModel subDepartmentModel);
    public Task<SubDepartmentGetViewModel> UpdateSubDepartment(SubDepartmentUpdateViewModel subDepartmentModel);
    public Task<bool> DeleteSubDepartment(long id);
    public Task<SubDepartmentGetViewModel> GetSubDepartmentById(long id);
    public Task<List<SubDepartmentGetViewModel>> GetAllSubDepartments();
    public Task<List<SubDepartmentGetViewModel>> GetSubDepartmentsByDepartmentId(long id);
    public Task<SubDepartmentStatisticsGetViewModel> GetSubDepartmentStatistics(long id, DateTime date);
    public Task<SubDepartmentGetViewModel> AddTelegramChatIdToSubDepartment(long subDepartmentId, long telegramChatId);
}