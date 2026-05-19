using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities;

[Table("sub_departments")]
public class SubDepartment : BaseEntity<long>
{
    [Required] 
    [Column("sub_department_short_name")] 
    public string SubDepartmentShortName { get; set; } = string.Empty;
    
    [Required] 
    [Column("sub_department_full_name")]
    public string SubDepartmentFullName { get; set; } = string.Empty;
    
    [Column("department_id"),ForeignKey(nameof(Department))] public long DepartmentId { get; set; }
    public virtual Department Department { get; set; }
    
 
    [Column("sub_department_telegram_chat_id")]
    public long SubDepartmentTelegramChatId { get; set; }
}