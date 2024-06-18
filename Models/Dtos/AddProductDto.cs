namespace Models.Dtos;

public record AddProductDto(string Name, decimal Price, string Description, string Category, string Sku, List<string> Images);