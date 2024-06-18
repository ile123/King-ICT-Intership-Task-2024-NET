namespace Models.Dtos;

public record ProductDto(Guid Id, string Name, decimal Price, string Description, string Category, List<string> Images);