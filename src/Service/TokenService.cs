using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Configuration;
using api.Enum;
using api.Interfaces;
using api.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Service;

public class TokenService : ITokenService
{
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly UserManager<AppUser> _userManager;

    public TokenService(IOptions<JwtConfiguration> options,UserManager<AppUser> userManager)
    {
        _jwtConfiguration = options.Value;
        _userManager = userManager;
    }
    public async Task<string> CreateJwtTokenService(AppUser appUser)
    {
        // getting first role
        var userRole = (await _userManager.GetRolesAsync(appUser)).FirstOrDefault() ?? nameof(UserRoleEnum.User);
        List<Claim> claims = new List<Claim>()
        {
            new (JwtRegisteredClaimNames.Sub,appUser.Id.ToString()),
            new (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Email,appUser.Email),
            new (ClaimTypes.Role,userRole)
        };

        var securityKey = Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey);
        var signInCreds = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtConfiguration.Issuer,
            audience: _jwtConfiguration.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtConfiguration.TokenExpiryMinutes),
            signingCredentials: signInCreds
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);

    }

}
