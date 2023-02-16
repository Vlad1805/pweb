using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MobyLabWebProgramming.Infrastructure.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class LoginService : ILoginService
{
    private readonly JwtConfiguration _jwtConfiguration;

    public LoginService(IOptions<JwtConfiguration> jwtConfiguration) => _jwtConfiguration = jwtConfiguration.Value;

    public string GetToken(UserDTO user, DateTime issuedAt, TimeSpan expiresIn)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfiguration.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) }),
            Claims = new Dictionary<string, object>
            {
                { ClaimTypes.Name, user.Name },
                { ClaimTypes.Email, user.Email }
            },
            IssuedAt = issuedAt,
            Expires = issuedAt.Add(expiresIn),
            Issuer = _jwtConfiguration.Issuer,
            Audience = _jwtConfiguration.Audience,
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }
}
