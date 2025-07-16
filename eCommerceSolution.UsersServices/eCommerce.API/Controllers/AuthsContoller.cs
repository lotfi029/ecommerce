using eCommerce.API.Infrastracture;
using eCommers.Core.Contracts.Users;
using eCommers.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthsContoller(IAuthService _authService) : ControllerBase
{
    [HttpPost("token")]
    public async Task<IResult> GetTokenAsync([FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await _authService.GetTokenAsync(request, ct);

        return result.Match(Results.Ok, CustomResults.ToProblem);
    }
    
    [HttpPost("register")]
    public async Task<IResult> RegisterAsync([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var result = await _authService.RegisterAsync(request, ct);
        
        return result.Match(Results.Ok, CustomResults.ToProblem);
    }
}
