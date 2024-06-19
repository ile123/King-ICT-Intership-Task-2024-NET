using System.ComponentModel.DataAnnotations;

namespace Models.Entities;

public class Image
{
    [Key]
    public Guid Id { get; init; }
    
    public string ImageUrl { get; set; } = string.Empty;
    
    public Product? Product { get; init; }

    public override string ToString()
    {
        return ImageUrl;
    }
}