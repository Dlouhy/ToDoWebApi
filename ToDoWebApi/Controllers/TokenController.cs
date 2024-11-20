using Microsoft.AspNetCore.Mvc;
using Service.Contract;
using Shared.DataTransferObjects;

namespace ToDoWebApi.Controllers;

[Route("api/token")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IApiAuthenticationService _service;

    public TokenController(IApiAuthenticationService service) => _service = service;

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] TokenDto tokenDto)
    {
        var tokenDtoToReturn = await
        _service.RefreshTokenAsync(tokenDto);

        return Ok(tokenDtoToReturn);
    }
}