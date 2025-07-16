using Polly;

namespace eCommerce.Infrastructure.PollyPolicies;
public interface IReusablePollyPolicies
{
    IAsyncPolicy<HttpResponseMessage> GetBulkheadPolicy();
    IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(int handledEventsAllowedBeforeBreaking, TimeSpan durationOfBreak);
    IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy();
    IAsyncPolicy<HttpResponseMessage> GetRetrayPolicy(int retryCount);
    IAsyncPolicy<HttpResponseMessage> GetTimeutPolicy(TimeSpan timeout);
}