using eCommerce.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace eCommerce.Infrastructure.Extensions;

public static class ProblemExtension
{
    public static Result GetProblemDetails(this ProblemDetails problemDetails)
    {

        if (!problemDetails.Extensions.TryGetValue("errors", out object? value))
            return new Error("Error.Users", "Invalid user id.", problemDetails.Status);

        if (value is JsonElement element && element.ValueKind == JsonValueKind.Object)
        {
            var errors = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(element.GetRawText());

            if (errors != null && errors.Count > 0)
            {
                var code = errors.Keys.First();
                var message = errors[code].FirstOrDefault() ?? "Unknown error";

                return new Error(code, message, problemDetails.Status);
            }
        }

        return new Error("Error.Unknown", "Unexpected error format.", problemDetails.Status);
    }
}
