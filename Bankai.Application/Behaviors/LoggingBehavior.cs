using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Bankai.Application.Behaviors;

/// <summary>
/// MediatR Pipeline Behavior: Logging (VG Requirement).
/// 
/// Wraps every handler execution with structured logging:
/// - Logs the request type and its properties on entry
/// - Logs the execution time on exit
/// - Logs a warning if the handler takes longer than 500ms (slow query detection)
/// 
/// No handler code is changed — cross-cutting concerns are handled here.
/// Pipeline order: Request → [LoggingBehavior] → [ValidationBehavior] → Handler
/// </summary>
public sealed class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation(
            "→ Handling {RequestName}: {@Request}",
            requestName, request);

        var sw = Stopwatch.StartNew();

        TResponse response;
        try
        {
            response = await next();
        }
        finally
        {
            sw.Stop();

            if (sw.ElapsedMilliseconds > 500)
            {
                _logger.LogWarning(
                    "⚠ Slow handler detected: {RequestName} took {ElapsedMs}ms",
                    requestName, sw.ElapsedMilliseconds);
            }
            else
            {
                _logger.LogInformation(
                    "← Completed {RequestName} in {ElapsedMs}ms",
                    requestName, sw.ElapsedMilliseconds);
            }
        }

        return response;
    }
}
