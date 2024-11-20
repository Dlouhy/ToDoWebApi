using Microsoft.AspNetCore.Identity;
using Shared.DataTransferObjects;

namespace Service.Contract;

public interface IApiAuthenticationService
{
    Task<TokenDto> CreateTokenAsync(bool populateExp);

    Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto);

    Task<IdentityResult> RegisterUserAsync(UserForRegistrationDto userForRegistration);

    Task<bool> ValidateUserAsync(UserForAuthenticationDto userForAuthentication);
}