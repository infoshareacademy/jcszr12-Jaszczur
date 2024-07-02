using Azure.Core;
using Microsoft.Extensions.Logging;

namespace TutorLizard.BusinessLogic.Extensions;
internal static class LoggingExtensions
{
    internal static IDisposable? BeginMethodCallScope<TService, TRequest>(this ILogger<TService> logger, string methodName, TRequest request, bool destructureRequest = true)
    {
        string scopeMessageFormat = $"Service: {{Service}} Method: {{ServiceMethod}} Request: {(destructureRequest ? "{@Request}" : "{Request}")}";
        var scope = logger.BeginScope(scopeMessageFormat, typeof(TService).Name, methodName, request);

        string logMessageFormat = $"[Service method call] Service: {{Service}} Method: {{ServiceMethod}} Request: {(destructureRequest ? "{@Request}" : "{Request}")}";
        logger.LogInformation(logMessageFormat, typeof(TService).Name, methodName, request);

        return scope;
    }

    internal static void LogReturningResponse<TService, TResponse>(this ILogger<TService> logger, TResponse response, bool destructureResponse = true)
    {
        string logMessageFormat = $"[Returning response] Response: {(destructureResponse ? "{@Response}" : "{Response}")}";
        logger.LogInformation(logMessageFormat, response);
    }
}
