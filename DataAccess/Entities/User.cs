using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using DataAccess.Enums;

namespace DataAccess.Entities;
[Table("users")]
public class User : BaseEntity<long>
{
    [Required] 
    [Column("full_name")]
    public string FullName { get; set; } = string.Empty;
    [Required]
    [Column("email")] 
    public string Email { get; set; }= string.Empty;
    [Required]
    [Column("password")] 
    public string Password { get; set; }= string.Empty;
    [Column("role")] public UserRole Role { get; set; }
    [Column("department_id"),ForeignKey(nameof(Department))] public long DepartmentId { get; set; }
    public virtual Department Department { get; set; }
}