﻿namespace eCommerce.SharedKernal.Abstractions;
public class Result
{
    public Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.Non || !isSuccess && error == Error.Non)
            throw new ArgumentException("invalid argument result!");
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() 
        => new (true, Error.Non);
    public static Result Failure(Error error)
        => new (false, error);
    public static Result<T> Success<T>(T result)
        => new(result, true, Error.Non);
    public static Result<T> Failure<T>(Error error)
        => new(default, false, error);
    
    public static implicit operator Result(Error error)
        => Failure(error);

}

public class Result<TValue>(TValue? value, bool isSuccess, Error error) : Result(isSuccess, error)
{
    public TValue? Value => IsSuccess
        ? value
        : throw new ArgumentException("invalid success result");


    public static implicit operator Result<TValue>(TValue value)
        => Success(value);

    public static implicit operator Result<TValue>(Error error)
        => Failure<TValue>(error);
}
