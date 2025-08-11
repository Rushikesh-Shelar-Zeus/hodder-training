using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ContosoPizzaNoSQl.Services;

public class JwtService
{
    public readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateToken(string userId, string role)
    {
        try
        {
            var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Role, role)
        };

            var jwtKey = _configuration.GetValue<string>("JwtSettings:Key");
            var jwtIssuer = _configuration.GetValue<string>("JwtSettings:Issuer");
            var jwtAudience = _configuration.GetValue<string>("JwtSettings:Audience");

            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                throw new ArgumentException("JWT settings are not properly configured.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            Console.WriteLine($"Generated JWT Token for User ID: {userId}, Role: {role}");
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (System.Exception)
        {

            Console.WriteLine("Error generating JWT Token");
            throw;
        }
    }
}