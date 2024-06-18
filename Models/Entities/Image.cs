using System.ComponentModel.DataAnnotations;

namespace Models.Entities;

public class Image
{
    [Key]
    public Guid Id { get; init; }
    
    [MaxLength(2000)]
    public string ImageUrl { get; set; } = string.Empty;
    
    public Product? Product { get; init; }
    
}