using System.ComponentModel.DataAnnotations;

namespace Models.Entities;

public class Product
{
    [Key]
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    [MaxLength(100)]
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public List<Image> Images { get; } = [];
}