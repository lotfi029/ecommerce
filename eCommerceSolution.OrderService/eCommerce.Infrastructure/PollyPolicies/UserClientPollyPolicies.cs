using Polly;
using Serilog;
using System.Net;

namespace eCommerce.Infrastructure.PollyPolicies;

public class UserClientPollyPolicies(IReusablePollyPolicies reusablePollyPolicies) : IUserClientPollyPolicies
{
    public IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
    {
        var retryPolicy = reusablePollyPolicies.GetRetrayPolicy(3);
        var circuitBreakerPolicy = reusablePollyPolicies.GetCircuitBreakerPolicy(5, TimeSpan.FromSeconds(30));
        var combinedPolicy = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
        return combinedPolicy;
    }

}
