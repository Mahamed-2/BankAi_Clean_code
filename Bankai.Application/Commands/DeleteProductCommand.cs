using MediatR;

namespace Bankai.Application.Commands;

/// <summary>
/// CQRS Command: Permanently delete a Product by ID.
/// Returns Unit — no body, status 204 from controller.
/// </summary>
public record DeleteProductCommand(int Id) : IRequest<Unit>;
