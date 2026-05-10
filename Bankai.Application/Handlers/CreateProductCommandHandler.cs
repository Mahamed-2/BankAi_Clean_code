using AutoMapper;
using MediatR;
using Bankai.Application.Commands;
using Bankai.Application.DTOs;
using Bankai.Domain.Entities;
using Bankai.Domain.Interfaces;

namespace Bankai.Application.Handlers;

/// <summary>
/// Handles CreateProductCommand.
/// The handler is the ONLY place that knows about both the command and the repository.
/// Application layer: orchestrates domain logic, delegates persistence to Infrastructure.
/// </summary>
public sealed class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        // Check for duplicate name
        bool exists = await _productRepository.ExistsAsync(request.Name, cancellationToken);
        if (exists)
            throw new InvalidOperationException($"A product named '{request.Name}' already exists.");

        // Domain entity enforces its own invariants
        var product = new Product(
            request.Name,
            request.Description,
            request.Price,
            request.Stock,
            request.CategoryId
        );

        await _productRepository.AddAsync(product, cancellationToken);
        await _productRepository.SaveChangesAsync(cancellationToken);

        // AutoMapper: Domain → DTO  (VG requirement)
        return _mapper.Map<ProductDto>(product);
    }
}
