namespace eCommers.Core.Abstractions;

public record Error(string Code, string Description, int? StatusCode)
{
    public readonly static Error Non = new(string.Empty, string.Empty, null);
    
    public static implicit operator Result(Error error)
        => new(false, error);

    public static Error NotFound(string code, string description)
        => new(code, description, 404);

    public static Error BadRequest(string code, string description)
        => new(code, description, 400);

    public static Error Unauthorized(string code, string description)
        => new(code, description, 401);

    public static Error Forbidden(string code, string description)
        => new(code, description, 403);

    public static Error InternalServerError(string code, string description)
        => new(code, description, 500);

    public static Error Conflict(string code, string description)
        => new(code, description, 409);
}
