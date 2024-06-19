using System.Text.Json.Serialization;

namespace Models.Dtos;

public record AddProductDto(string Title, decimal Price, string Description, string Category, string Sku, List<string> Images);