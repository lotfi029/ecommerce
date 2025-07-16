using eCommerce.Core.Contracts;
using Polly;
using Serilog;
using System.Net;
using System.Text.Json;

namespace eCommerce.Infrastructure.PollyPolicies;

public class ReusablePollyPolicies : IReusablePollyPolicies
{
    public IAsyncPolicy<HttpResponseMessage> GetRetrayPolicy(int retryCount)
    {
        var policy = Policy
            .HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.InternalServerError)
            .WaitAndRetryAsync(retryCount: retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            (result, timeSpan, retryCount, context) =>
            {
                Log.Information($"Retry {retryCount} encountered an error: {result.Result.StatusCode}. Waiting {timeSpan} before next retry.");
            });

        return policy;

    }
    public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(int handledEventsAllowedBeforeBreaking, TimeSpan durationOfBreak)
    {
        var policy = Policy
            .HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.InternalServerError)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: handledEventsAllowedBeforeBreaking,
                durationOfBreak: durationOfBreak,
                onBreak: (outcome, timespan) => // from close state to open state
                {
                    Log.Warning($"Circuit broken due to: {outcome.Result.StatusCode}. Breaking for {timespan.TotalSeconds} seconds.");
                },
                onReset: () => // from open state to halfopen state
                {
                    Log.Information("Circuit reset, allowing requests to pass through again.");
                }
            );

        return policy;
    }
    public IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy()
    {

        var policy = Policy
            .HandleResult<HttpResponseMessage>(e => e.StatusCode == HttpStatusCode.InternalServerError)
            .FallbackAsync(async (context) =>
            {
                Log.Warning("Fallback policy triggered due to an internal server error.");

                var fallbackResponse = new ProductResponse(
                    Guid.Empty.ToString(),
                    "Fallback Product",
                    "This is a fallback product description.",
                    0.0m,
                    Guid.Empty.ToString(),
                    "Fallback category"
                    );
                var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(fallbackResponse), System.Text.Encoding.UTF8, "application/json")
                };

                return httpResponseMessage;
            });

        return policy;

    }


    public IAsyncPolicy<HttpResponseMessage> GetTimeutPolicy(TimeSpan timeout)
    {
        var policy = Policy
            .TimeoutAsync<HttpResponseMessage>(timeout);

        return policy;

    }

    public IAsyncPolicy<HttpResponseMessage> GetBulkheadPolicy()
    {

        var policy = Policy
            .BulkheadAsync<HttpResponseMessage>(
            maxParallelization: 2,
            maxQueuingActions: 40,
            onBulkheadRejectedAsync: (context) =>
            {
                Log.Warning("Bulkhead policy rejected a request due to too many concurrent requests or queue size exceeded.");

                var fallbackResponse = new ProductResponse(
                    Guid.Empty.ToString(),
                    "Bulkhead Rejected Product",
                    "This product is not available due to high load.",
                    0.0m,
                    Guid.Empty.ToString(),
                    "Bulkhead category"
                );
                var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(JsonSerializer.Serialize(fallbackResponse), System.Text.Encoding.UTF8, "application/json")
                };
                return Task.FromResult(httpResponseMessage);
            });

        return policy;

    }
}