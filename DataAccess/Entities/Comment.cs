using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Enums;

namespace DataAccess.Entities;

public class Comment : BaseEntity<long>
{
    [Column("job_id"),ForeignKey(nameof(Job))]
    public long  JobId { get; set; }
    public virtual Job Job { get; set; }
    
   
    [Column("publisher_id"),ForeignKey(nameof(Publisher))]
    public long  PublisherId { get; set; }
    public virtual User Publisher { get; set; }

   
    [Column("message")]
    public string Message { get; set; }

    [Column("from_status")]
    public JobStatus FromStatus { get; set; }
    
    [Column("to_status")]
    public JobStatus ToStatus { get; set; } 
    
    [Column("published_date")]
    public DateTime PublishedDate { get; set; }
}