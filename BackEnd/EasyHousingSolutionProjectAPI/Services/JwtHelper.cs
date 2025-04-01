using EasyHousingSolutionProjectAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtHelper
{
    private readonly JWTSettings _jwtSettings;

    public JwtHelper(JWTSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    public string GenerateJwtToken(string username, string userType)
    {
        // Validate inputs
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentException("Username cannot be null or empty.", nameof(username));
        }

        if (string.IsNullOrEmpty(userType))
        {
            throw new ArgumentException("UserType cannot be null or empty.", nameof(userType));
        }

        if (string.IsNullOrEmpty(_jwtSettings.SecretKey))
        {
            throw new ArgumentException("Secret key cannot be null or empty.", nameof(_jwtSettings.SecretKey));
        }

        // Define claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, userType)
        };

        // Create security key
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Set expiration (e.g., 1 hour)
        var expirationTime = DateTime.Now.AddHours(1);

        // Generate the token
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expirationTime,
            signingCredentials: creds
        );

        // Return the serialized JWT token
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
