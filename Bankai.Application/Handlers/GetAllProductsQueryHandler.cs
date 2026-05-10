using AutoMapper;
using MediatR;
using Bankai.Application.DTOs;
using Bankai.Application.Queries;
using Bankai.Domain.Interfaces;

namespace Bankai.Application.Handlers;

/// <summary>
/// Handles GetAllProductsQuery.
/// Read-only: no domain mutation. Maps entities to DTOs via AutoMapper.
/// </summary>
public sealed class GetAllProductsQueryHandler
    : IRequestHandler<GetAllProductsQuery, IReadOnlyList<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ProductDto>> Handle(
        GetAllProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<ProductDto>>(products);
    }
}
