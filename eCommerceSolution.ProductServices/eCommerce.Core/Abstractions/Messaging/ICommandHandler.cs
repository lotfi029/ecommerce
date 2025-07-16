using eCommerce.Core.Abstractions;

namespace eCommerce.Core.Abstractions.Messaging;
public interface ICommandHandler<in TCommand> 
    where TCommand : ICommand
{
    Task<Result> Handle(TCommand command, CancellationToken ct);
}
public interface ICommandHandler<in TCommand, TResponse> 
    where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken ct);
}
