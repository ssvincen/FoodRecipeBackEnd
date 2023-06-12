using Microsoft.IdentityModel.Tokens;
using Recipes.BO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecipesWebAPI;

public static class JwtHelper
{
    public static JwtSecurityToken GetJwtTokens(UserViewModel model, string signingKey)
    {
        var claims = new List<Claim>
        {
            new Claim("UserId", model.Id.ToString()),
            new Claim("Username", model.EmailAddress),
            new Claim("Displayname", model.FirstName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, model.EmailAddress)
        };

        foreach (var role in model.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityToken(
            expires: DateTime.Now.AddMinutes(60),
            claims: claims,
            signingCredentials: creds
        );
    }
}
