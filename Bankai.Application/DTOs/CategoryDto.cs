namespace Bankai.Application.DTOs;

/// <summary>
/// Data Transfer Object for Category.
/// Keeps the API contract stable even if the domain model evolves.
/// </summary>
public record CategoryDto(
    int Id,
    string Name,
    string Slug,
    int ProductCount
);
