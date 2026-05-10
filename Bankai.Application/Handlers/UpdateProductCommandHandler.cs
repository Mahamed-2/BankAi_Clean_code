using MediatR;
using Bankai.Application.Commands;
using Bankai.Domain.Interfaces;

namespace Bankai.Application.Handlers;

/// <summary>
/// Handles UpdateProductCommand.
/// Loads the entity from the repository, delegates change to domain method.
/// Infrastructure (EF Core change tracking) detects the mutation automatically.
/// </summary>
public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
        => _productRepository = productRepository;

    public async Task<Unit> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Product with ID {request.Id} was not found.");

        // Domain method validates business rules
        product.UpdateDetails(request.Name, request.Description, request.Price, request.Stock);

        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
