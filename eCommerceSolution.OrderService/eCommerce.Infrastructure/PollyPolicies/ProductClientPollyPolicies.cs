using Polly;

namespace eCommerce.Infrastructure.PollyPolicies;

public class ProductClientPollyPolicies(IReusablePollyPolicies reusablePollyPolicies) : IProductClientPollyPolicies
{
    public IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy
    {
        get
        {
            var failbackPolicy = reusablePollyPolicies.GetFallbackPolicy();
            var timeoutPolicy = reusablePollyPolicies.GetTimeutPolicy(TimeSpan.FromSeconds(1500));
            var bulkheadPolicy = reusablePollyPolicies.GetBulkheadPolicy();

            var combinedPolicy = Policy.WrapAsync(failbackPolicy, timeoutPolicy, bulkheadPolicy);

            return combinedPolicy;
        }

    }
}