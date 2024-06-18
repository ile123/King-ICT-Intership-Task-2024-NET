using System.ComponentModel.DataAnnotations;

namespace Models.Entities;

public class Product
{
    [Key] 
    public Guid Id { get; init; }

    [MaxLength(100)] 
    public string Name { get; set; } = string.Empty;
    
    public decimal Price { get; set; }

    [MaxLength(100)] 
    public string Description { get; set; } = string.Empty;

    [MaxLength(100)] 
    public string Category { get; set; } = string.Empty;
    
    [MaxLength(100)] 
    public string Sku { get; set; } = string.Empty;

    public IEnumerable<Image> Images { get; init; } = new List<Image>();
}