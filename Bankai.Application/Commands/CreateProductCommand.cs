using MediatR;
using Bankai.Application.DTOs;

namespace Bankai.Application.Commands;

/// <summary>
/// CQRS Command: Create a new Product.
/// Commands represent intent to change state.
/// Returns the created ProductDto after success.
/// </summary>
public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    int CategoryId
) : IRequest<ProductDto>;
