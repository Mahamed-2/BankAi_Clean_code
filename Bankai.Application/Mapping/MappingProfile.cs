using AutoMapper;
using Bankai.Application.DTOs;
using Bankai.Domain.Entities;

namespace Bankai.Application.Mapping;

/// <summary>
/// AutoMapper Mapping Profile (VG Requirement).
/// Maps Domain entities → DTOs, keeping the API decoupled from the domain model.
/// 
/// Benefits:
/// - Domain entities can evolve without breaking the API contract
/// - Sensitive fields (e.g., PasswordHash) are never accidentally exposed
/// - Projection queries can be optimized per DTO
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product → ProductDto
        // CategoryName is resolved from the navigation property
        CreateMap<Product, ProductDto>()
            .ConstructUsing(src => new ProductDto(
                src.Id,
                src.Name,
                src.Description,
                src.Price,
                src.Stock,
                src.CategoryId,
                src.Category != null ? src.Category.Name : string.Empty
            ));

        // Category → CategoryDto
        // ProductCount is derived from the collection count
        CreateMap<Category, CategoryDto>()
            .ConstructUsing(src => new CategoryDto(
                src.Id,
                src.Name,
                src.Slug,
                src.Products.Count
            ));
    }
}
