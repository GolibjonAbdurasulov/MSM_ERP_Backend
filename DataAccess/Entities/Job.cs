using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Enums;

namespace DataAccess.Entities;
[Table("jobs")]
public class Job : BaseEntity<long>
{
    [Required] 
    [MaxLength(200)]
    [Column("title")]
    public string Title { get; set; }
    
    [Required] 
    [Column("description")]
    public string Description { get; set; }
    

    [Column("job_status")]
    public JobStatus JobStatus { get; set; }
    
    [Column("publisher_id"), ForeignKey(nameof(Publisher))]
    public long PublisherId { get; set; }
    public virtual User Publisher { get; set; }

    [Column("mobilized_workers")]
    public int MobilizedWorkers { get; set; }
    
    [Required] 
    [Column("department_id"), ForeignKey(nameof(Department))]
    public long DepartmentId { get; set; }
    public virtual Department Department { get; set; }
    
    [Column("published_date")]
    public DateTime PublishedDate { get; set; }
    
    [Column("started_date")]
    public DateTime StartedDate { get; set; }
    
    [Column("end_date")]
    public DateTime EndDate { get; set; }
}