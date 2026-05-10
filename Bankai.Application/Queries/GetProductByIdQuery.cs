using MediatR;
using Bankai.Application.DTOs;

namespace Bankai.Application.Queries;

/// <summary>
/// CQRS Query: Retrieve a single product by its ID.
/// Returns null (404) if not found — handled in the controller.
/// </summary>
public record GetProductByIdQuery(int Id) : IRequest<ProductDto?>;
