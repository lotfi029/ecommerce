using eCommerce.API.Infrastracture;
using eCommers.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService _userService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IResult> GetUserById([FromRoute] Guid id, CancellationToken ct = default)
    {
        
        var result = await _userService.GetUserByIdAsync(id, ct);

        return result.Match(Results.Ok,  CustomResults.ToProblem);
    }
}
