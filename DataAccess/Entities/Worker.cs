using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities;
[Table("workers")]
public class Worker : BaseEntity<long>
{
   [Column("department_id"),ForeignKey(nameof(Department))]
   public long DepartmentId { get; set; }
   public virtual Department Department { get; set; }
   
   [Column("personnel_number")]
   public long PersonnelNumber { get; set; }
   
   [Column("full_name")]
   public string FullName { get; set; }
   
   [Column("position")]
   public string Position { get; set; }
}