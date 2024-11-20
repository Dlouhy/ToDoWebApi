using Microsoft.AspNetCore.Mvc;
using Service.Contract;
using Shared.DataTransferObjects;

namespace ToDoWebApi.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IApiAuthenticationService _authenticationService;

    public AuthenticationController(IApiAuthenticationService service) => _authenticationService = service;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUserAsync([FromBody] UserForRegistrationDto userForRegistration)
    {
        var result = await _authenticationService.RegisterUserAsync(userForRegistration);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        return StatusCode(201);
    }

    [HttpPost("login")]
    public async Task<IActionResult> AuthenticateAsync([FromBody] UserForAuthenticationDto userForAuthentication)
    {
        if (!await _authenticationService.ValidateUserAsync(userForAuthentication))
        {
            return Unauthorized();
        }

        var tokenDTO = await _authenticationService.CreateTokenAsync(populateExp: true);

        return Ok(tokenDTO);
    }
}