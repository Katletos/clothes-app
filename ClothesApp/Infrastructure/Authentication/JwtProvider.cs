using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string Generate(User user)
    {
        var role = user.UserType switch
        {
            UserType.Customer => Roles.Customer,
            UserType.Admin => Roles.Admin,
            _ => throw new ArgumentOutOfRangeException(),
        };

        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(CustomClaims.Id, user.Id.ToString()),
            new(CustomClaims.Role, role),
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(_options.ExpiredTimeMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}