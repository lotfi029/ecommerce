﻿using eCommerce.SharedKernal.Abstractions;

namespace InventoryService.Api.Extensions;

public static class ResultExtension
{
    public static TOut Match<TOut> (
        this Result result, 
        Func<TOut> onSuccess, 
        Func<Result, TOut> onFailure
        )
        => result.IsSuccess ? onSuccess() : onFailure(result);

    public static TOut Match<TIn, TOut> (
        this Result<TIn> result, 
        Func<TIn, TOut> onSuccess, 
        Func<Result<TIn>, TOut> onFailure
        )
        => result.IsSuccess ? onSuccess(result.Value!) : onFailure(result);
}
