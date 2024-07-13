using Microsoft.Extensions.Logging;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;

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

    internal static void LogEntityCreated<TEntity, TId>(this ILogger<IDbRepository<TEntity>> logger, string methodName, TId id)
        where TEntity : class
    {
        logger.LogInformation("[DbRepository.{DBRepositoryMethod}] Entity of type: {EntityType} created successfully with Id: {EntityId}", methodName, typeof(TEntity).Name, id);
    }

    internal static void LogEntityNotCreated<TEntity, TId>(this ILogger<IDbRepository<TEntity>> logger, string methodName)
        where TEntity : class
    {
        logger.LogWarning("[DbRepository.{DBRepositoryMethod}] Entity of type: {EntityType} not created", methodName, typeof(TEntity).Name);
    }

    internal static void LogEntityUpdated<TEntity, TId>(this ILogger<IDbRepository<TEntity>> logger, string methodName, TId id)
        where TEntity : class
    {
        logger.LogInformation("[DbRepository.{DBRepositoryMethod}] Entity of type: {EntityType} with Id: {EntityId} updated successfully", methodName, typeof(TEntity).Name, id);
    }

    internal static void LogEntityNotUpdated<TEntity, TId>(this ILogger<IDbRepository<TEntity>> logger, string methodName, TId id)
        where TEntity : class
    {
        logger.LogWarning("[DbRepository.{DBRepositoryMethod}] Entity of type: {EntityType} with Id: {EntityId} not updated", methodName, typeof(TEntity).Name, id);
    }

    internal static void LogEntityDeleted<TEntity, TId>(this ILogger<IDbRepository<TEntity>> logger, string methodName, TId id)
        where TEntity : class
    {
        logger.LogInformation("[DbRepository.{DBRepositoryMethod}] Entity of type: {EntityType} with Id: {EntityId} deleted successfully", methodName, typeof(TEntity).Name, id);
    }

    internal static void LogEntityNotDeleted<TEntity, TId>(this ILogger<IDbRepository<TEntity>> logger, string methodName, TId id)
        where TEntity : class
    {
        logger.LogWarning("[DbRepository.{DBRepositoryMethod}] Entity of type: {EntityType} with Id: {EntityId} not deleted", methodName, typeof(TEntity).Name, id);
    }

    internal static void LogEntityFound<TEntity, TId>(this ILogger<IDbRepository<TEntity>> logger, string methodName, TId id)
        where TEntity : class
    {
        logger.LogInformation("[DbRepository.{DBRepositoryMethod}] Entity of type: {EntityType} with Id: {EntityId} found", methodName, typeof(TEntity).Name, id);
    }

    internal static void LogEntityNotFound<TEntity, TId>(this ILogger<IDbRepository<TEntity>> logger, string methodName, TId id)
        where TEntity : class
    {
        logger.LogWarning("[DbRepository.{DBRepositoryMethod}] Entity of type: {EntityType} with Id: {EntityId} not found", methodName, typeof(TEntity).Name, id);
    }
}
