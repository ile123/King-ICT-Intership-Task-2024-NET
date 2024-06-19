using System.ComponentModel.DataAnnotations;

namespace Models.Entities;

public class Product
{
    [Key] 
    public Guid Id { get; init; }
    
    public string Title { get; set; } = string.Empty;
    
    public decimal Price { get; set; }

    [MaxLength(100)] 
    public string Description { get; set; } = string.Empty;
    
    public string Category { get; set; } = string.Empty;
    
    public string Sku { get; set; } = string.Empty;

    public IEnumerable<Image> Images { get; init; } = new List<Image>();
}