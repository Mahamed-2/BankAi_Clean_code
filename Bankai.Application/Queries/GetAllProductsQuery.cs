using MediatR;
using Bankai.Application.DTOs;

namespace Bankai.Application.Queries;

/// <summary>
/// CQRS Query: Retrieve all products.
/// Queries are read-only — they never change state.
/// </summary>
public record GetAllProductsQuery : IRequest<IReadOnlyList<ProductDto>>;
