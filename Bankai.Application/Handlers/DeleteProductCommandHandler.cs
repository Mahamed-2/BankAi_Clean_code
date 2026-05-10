using MediatR;
using Bankai.Application.Commands;
using Bankai.Domain.Interfaces;

namespace Bankai.Application.Handlers;

/// <summary>
/// Handles DeleteProductCommand.
/// Verifies the entity exists before deleting to return a meaningful 404.
/// </summary>
public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
        => _productRepository = productRepository;

    public async Task<Unit> Handle(
        DeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Product with ID {request.Id} was not found.");

        _productRepository.Delete(product);
        await _productRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
