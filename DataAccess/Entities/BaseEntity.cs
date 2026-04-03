using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities;

public class BaseEntity<TId> where TId: struct
{
 [Column("id")]
 public TId Id { get; set; }   
}