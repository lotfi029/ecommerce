using Serilog;
using Serilog.Context;

namespace eCommerceCatalogService.Core.Decorators;

internal class LoggingDecorators
{
    internal class QueryHandler<TQuery, TResponse>(
    IQueryHandler<TQuery, TResponse> innerHandler
    ) : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> HandleAsync(TQuery query, CancellationToken ct = default)
        {
            string requestName = query.GetType().Name;
            Log.Information("Handling query {RequestName} with paramters {@Query}", requestName, query);
            var result = await innerHandler.HandleAsync(query, ct);

            if (result.IsSuccess)
            {
                Log.Information("Query {RequestName} handled successfully with response {@Response}", requestName, result.Value);
            }
            else
            {
                using (LogContext.PushProperty(name: "Error", value: result.Error, destructureObjects: true))
                {
                    Log.Error("Query {RequestName} failed with error {@Error}", requestName, result.Error);
                }
            }
            return result;
        }
    }
    internal class CommandHandler<TCommand, TResponse>(
        CommandHandler<TCommand, TResponse> innerHandler
        ) : ICommandHandler<TCommand, TResponse>
            where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken ct = default)
        {
            string requestName = command.GetType().Name;
            Log.Information("Handling query {RequestName} with paramters {@Query}", requestName, command);
            var result = await innerHandler.HandleAsync(command, ct);

            if (result.IsSuccess)
            {
                Log.Information("Query {RequestName} handled successfully with response {@Response}", requestName, result.Value);
            }
            else
            {
                using (LogContext.PushProperty(name: "Error", value: result.Error, destructureObjects: true))
                {
                    Log.Error("Query {RequestName} failed with error {@Error}", requestName, result.Error);
                }
            }
            return result;
        }
    }
    internal class CommandHandler<TCommand>(
        CommandHandler<TCommand> innerHandler
        ) : ICommandHandler<TCommand>
            where TCommand : ICommand
    {
        public async Task<Result> HandleAsync(TCommand command, CancellationToken ct = default)
        {
            string requestName = command.GetType().Name;
            Log.Information("Handling query {RequestName} with paramters {@Query}", requestName, command);
            var result = await innerHandler.HandleAsync(command, ct);

            if (result.IsSuccess)
            {
                Log.Information("Query {RequestName} handled successfully", requestName);
            }
            else
            {
                using (LogContext.PushProperty(name: "Error", value: result.Error, destructureObjects: true))
                {
                    Log.Error("Query {RequestName} failed with error {@Error}", requestName, result.Error);
                }
            }
            return result;
        }
    }
}
