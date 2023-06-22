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
            UserType.Customer => "Customer",
            UserType.Admin => "Admin",
            _ => throw new ArgumentOutOfRangeException(),
        };

        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("Id", user.Id.ToString()),
            new("Role", role),
        };
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("This is my supper secret key for jwt")); //add options

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var token = new JwtSecurityToken(null, null,
            claims,
            expires: DateTime.Now.AddMinutes(180),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}