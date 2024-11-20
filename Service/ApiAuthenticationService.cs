using AutoMapper;
using Entities;
using Entities.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Service.Contract;
using Shared.DataTransferObjects;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Service;

public class ApiAuthenticationService : IApiAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<ApiAuthenticationService> _logger;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly JwtConfiguration _jwtConfiguration;
    private User? _user;

    private string _secretKey = Environment.GetEnvironmentVariable("SECRET");

    public ApiAuthenticationService(UserManager<User> userManager, ILogger<ApiAuthenticationService> logger, IMapper mapper, IConfiguration configuration)
    {
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
        _configuration = configuration;
        _jwtConfiguration = new JwtConfiguration();
        _configuration.Bind(_jwtConfiguration.Section, _jwtConfiguration);
    }

    public async Task<TokenDto> CreateTokenAsync(bool populateExp)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaimsAsync();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        var refreshToken = GenerateRefreshToken();

        _user.RefreshToken = refreshToken;

        if (populateExp)
        {
            _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        }

        await _userManager.UpdateAsync(_user);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new TokenDto(accessToken, refreshToken);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_secretKey);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaimsAsync()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, _user.UserName)
        };

        var roles = await _userManager.GetRolesAsync(_user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken
                (
                    issuer: _jwtConfiguration.ValidIssuer,
                    audience: _jwtConfiguration.ValidAudience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires, CultureInfo.InvariantCulture)),
                    signingCredentials: signingCredentials
                );

        return tokenOptions;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<IdentityResult> RegisterUserAsync(UserForRegistrationDto userForRegistration)
    {
        var user = _mapper.Map<User>(userForRegistration);

        var result = await _userManager.CreateAsync(user, userForRegistration.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
        }

        return result;
    }

    public async Task<bool> ValidateUserAsync(UserForAuthenticationDto userForAuthentication)
    {
        _user = await _userManager.FindByNameAsync(userForAuthentication.UserName);

        var result = _user != null && await _userManager.CheckPasswordAsync(_user,
            userForAuthentication.Password);

        if (!result)
        {
            _logger.LogWarning($"{nameof(ValidateUserAsync)}: Authentication failed. Wrong user name or password.");
        }

        return result;
    }

    public async Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto)
    {
        ClaimsPrincipal principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
        User? user = await _userManager.FindByNameAsync(principal.Identity.Name);

        if (user == null || user.RefreshToken != tokenDto.RefreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new RefreshTokenBadRequest();
        }

        _user = user;

        return await CreateTokenAsync(populateExp: false);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
            ValidateLifetime = true,
            ValidIssuer = _jwtConfiguration.ValidIssuer,
            ValidAudience = _jwtConfiguration.ValidAudience,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}