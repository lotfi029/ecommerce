﻿namespace eCommerce.SharedKernal.Messaging;
public interface ICommand : IBaseCommand;

public interface ICommand<T> : IBaseCommand;

public interface IBaseCommand;


