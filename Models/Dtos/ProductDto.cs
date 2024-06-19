using Models.Entities;

namespace Models.Dtos;

public record ProductDto(Guid Id, string Title, decimal Price, string Description, string Category, string Sku, List<string> Images);