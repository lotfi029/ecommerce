﻿using eCommerce.SharedKernal.Abstractions;

namespace eCommerce.SharedKernal.Messaging;

public interface ICommandHandler<in TCommand> 
    where TCommand : ICommand 
{
    Task<Result> HandleAsync(TCommand command, CancellationToken ct = default);
}
public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken ct = default);
}