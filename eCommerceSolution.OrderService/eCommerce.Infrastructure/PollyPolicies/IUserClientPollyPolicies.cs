using Polly;

namespace eCommerce.Infrastructure.PollyPolicies;

public interface IUserClientPollyPolicies
{
    IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy();
}