namespace Models.Dtos;

public record ProductDto(Guid Id, string Name, decimal Price, string Description, string Category, string Sku, List<string> Images);