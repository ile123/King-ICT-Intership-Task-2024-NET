namespace Models.Dtos;

public record ApiResponseDto<T>(bool Success, string Message, T Result);