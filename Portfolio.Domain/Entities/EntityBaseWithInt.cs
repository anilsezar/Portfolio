using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Domain.Entities;

public abstract class EntityBaseWithInt
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; init; }
    
    public DateTime CreationTime { get; }
    
    protected EntityBaseWithInt()
    {
        CreationTime = DateTime.UtcNow;
    }
}