using eCommerce.Core.Abstractions.Messaging;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace eCommerce.Core.Abstractions.Behaviors;
internal static class LoggingDecorator
{
    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler,
        ILogger<QueryHandler<TQuery, TResponse>> logger
        ) : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken ct)
        {
            string requestName = query.GetType().Name;
            logger.LogInformation("Handling query {RequestName} with parameters {@Query}", requestName, query);
            var result = await innerHandler.Handle(query, ct);

            if (result.IsSuccess)
            {
                logger.LogInformation("Query {RequestName} handled successfully with response {@Response}", requestName, result.Value);
            }
            else
            {
                using (LogContext.PushProperty(name: "Error", value: result.Error, destructureObjects: true))
                {
                    logger.LogError("Query {RequestName} failed with error {@Error}", requestName, result.Error);
                }
            }

            return result;
        }
    }
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        ILogger<CommandHandler<TCommand, TResponse>> logger
        ) : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken ct)
        {
            string requestName = command.GetType().Name;
            logger.LogInformation("Handling query {RequestName} with parameters {@Query}", requestName, command);
            var result = await innerHandler.Handle(command, ct);

            if (result.IsSuccess)
            {
                logger.LogInformation("Command {RequestName} handled successfully with response {@Response}", requestName, result.Value);
            }
            else
            {
                using (LogContext.PushProperty(name: "Error", value: result.Error, destructureObjects: true))
                {
                    logger.LogError("Command {RequestName} failed with error {@Error}", requestName, result.Error);
                }
            }

            return result;
        }
    }
    internal sealed class CommandHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        ILogger<CommandHandler<TCommand>> logger
        ) : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken ct)
        {
            string requestName = command.GetType().Name;
            logger.LogInformation("Handling query {RequestName} with parameters {@Query}", requestName, command);
            var result = await innerHandler.Handle(command, ct);

            if (result.IsSuccess)
            {
                logger.LogInformation("Query {RequestName} handled successfully", requestName);
            }
            else
            {
                using (LogContext.PushProperty(name: "Error", value: result.Error, destructureObjects: true))
                {
                    logger.LogError("Query {RequestName} failed with error {@Error}", requestName, result.Error);
                }
            }

            return result;
        }
    }
}
