using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities;
[Table("departments")]
public class Department : BaseEntity<long>
{
    [Required] 
    [Column("department_short_name")] 
    public string DepartmentShortName { get; set; } = string.Empty;
    
    [Required] 
    [Column("department_full_name")]
    public string DepartmentFullName { get; set; } = string.Empty;
    
    [Required] 
    [Column("department_workers_count")]
    public int DepartmentWorkersCount { get; set; } 
}