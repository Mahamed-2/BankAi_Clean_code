using MediatR;

namespace Bankai.Application.Commands;

/// <summary>
/// CQRS Command: Update an existing Product.
/// Carries the updated field values. Returns Unit (no body).
/// </summary>
public record UpdateProductCommand(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int Stock
) : IRequest<Unit>;
