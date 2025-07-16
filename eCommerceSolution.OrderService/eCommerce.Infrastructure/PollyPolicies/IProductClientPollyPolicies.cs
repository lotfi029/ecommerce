using Polly;

namespace eCommerce.Infrastructure.PollyPolicies;
public interface IProductClientPollyPolicies
{
    IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy { get; }
}