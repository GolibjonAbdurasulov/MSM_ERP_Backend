using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using DataAccess.Enums;

namespace DataAccess.Entities;
[Table("users")]
public class User : BaseEntity<long>
{
    [Required] 
    [Column("first_name")]
    public string FirstName { get; set; } = string.Empty;    
    
    [Required] 
    [Column("last_name")]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [Column("email")] 
    public string Email { get; set; }= string.Empty;
    
    [Required]
    [Column("password")] 
    public string Password { get; set; }= string.Empty;
    
    [Column("role")] 
    public UserRole Role { get; set; }
    [Column("department_id"),ForeignKey(nameof(Department))] public long DepartmentId { get; set; }
    public virtual Department Department { get; set; }
    
    [Required]
    [Column("password_hash")]
    public byte[] PasswordHash { get; set; } 

    [Required]
    [Column("password_salt")]
    public byte[] PasswordSalt { get; set; } 
    
    [Column("is_signed")]
    public bool IsSigned { get; set; }
    [Column("last_login_date")]
    public DateTime LastLoginDate { get; set; }
}