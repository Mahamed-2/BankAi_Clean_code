using AutoMapper;
using MediatR;
using Bankai.Application.DTOs;
using Bankai.Application.Queries;
using Bankai.Domain.Interfaces;

namespace Bankai.Application.Handlers;

/// <summary>
/// Handles GetProductByIdQuery.
/// Returns null if the product is not found — controller converts to 404.
/// </summary>
public sealed class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ProductDto?> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        return product is null ? null : _mapper.Map<ProductDto>(product);
    }
}
