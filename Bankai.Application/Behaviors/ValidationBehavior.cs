using FluentValidation;
using MediatR;

namespace Bankai.Application.Behaviors;

/// <summary>
/// MediatR Pipeline Behavior: Validation (VG Requirement).
/// 
/// This behavior intercepts every command/query going through MediatR's pipeline.
/// If FluentValidation validators exist for the request, they are all executed
/// BEFORE the handler runs. If validation fails, a ValidationException is thrown
/// and the handler never executes — keeping handlers clean and focused.
/// 
/// Pipeline order: Request → [LoggingBehavior] → [ValidationBehavior] → Handler
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        => _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next(); // No validators registered → skip

        var context = new ValidationContext<TRequest>(request);

        // Run all validators in parallel
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count > 0)
            throw new ValidationException(failures);

        return await next(); // Validation passed → proceed to handler
    }
}
