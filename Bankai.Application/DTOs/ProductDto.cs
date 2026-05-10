namespace Bankai.Application.DTOs;

/// <summary>
/// Data Transfer Object for Product.
/// VG requirement: Use DTOs with AutoMapper to avoid exposing domain entities directly.
/// The DTO is what the API returns — no domain logic leaks out.
/// </summary>
public record ProductDto(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    int CategoryId,
    string CategoryName
);
