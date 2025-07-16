namespace eCommerce.Core.Abstractions.Behaviors;

internal static class ValidationDecorator
{
    //public sealed class CommandHandler<TCommand>(
    //    ICommandHandler<TCommand> handler,
    //    IEnumerable<IValidator<TCommand>> validators)
    //    : ICommandHandler<TCommand>
    //    where TCommand : ICommand
    //{
    //    public Task<Result> Handle(TCommand command, CancellationToken ct)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public sealed class CommandHandler<TCommand, TResponse>(
    //    ICommandHandler<TCommand, TResponse> innerHandler,
    //    IEnumerable<IValidator<TCommand>> validators)
    //    : ICommandHandler<TCommand, TResponse>
    //    where TCommand : ICommand<TResponse>
    //{
    //    public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken ct)
    //    {
    //        var validationFailures = await ValidateAsync(command, ct);

    //        if (validationFailures.Length == 0)
    //            return await innerHandler.Handle(command, ct);

    //        if (typeof(TResponse).IsGenericType
    //            && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
    //        {
    //            var resultType = typeof(TResponse).GetGenericArguments()[0];

    //            var failureType = typeof(Result<>)
    //                .MakeGenericType(resultType)
    //                .GetMethod(nameof(Result<object>));
    //            var error = new ValidationError(validationFailures.Select(f => Error.BadRequest(f.ErrorCode, f.ErrorMessage)).ToArray());
    //            return (Result<TResponse>)error;
    //        }
    //    }
    //    private async Task<ValidationFailure[]> ValidateAsync(TCommand command, CancellationToken ct)
    //    {
    //        if (!validators.Any())
    //            return [];


    //        var context = new ValidationContext<TCommand>(command);

    //        var validationResults = await Task.WhenAll(
    //            validators.Select(v => v.ValidateAsync(context, ct))
    //        );

    //        var failures = validationResults
    //            .Where(failure => !failure.IsValid)
    //            .SelectMany(result => result.Errors)
    //            .ToArray();

    //        return failures;
    //    }

    //    //private static ValidationError CreateValidationError(ValidationFailure[] failures) => 
    //    //    new(failures.Select(e => Error.BadRequest(e.ErrorCode, e.ErrorMessage)).ToArray());
    //}
}