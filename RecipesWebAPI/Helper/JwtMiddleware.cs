using Microsoft.IdentityModel.Tokens;
using Recipes.BI;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace RecipesWebAPI;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task Invoke(HttpContext context, IAccountDataAccess _accountData, CurrentUser currentUser)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            await AttachUserToContext(context, _accountData, token, currentUser);
        }
        await _next(context);
    }

    private async Task AttachUserToContext(HttpContext context, IAccountDataAccess _accountData, string token, CurrentUser currentUser)
    {
        var signingKey = _configuration["JWT:Secret"];
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(signingKey);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = Convert.ToInt64(jwtToken.Claims.First(x => x.Type == "UserId").Value);
            var user = await _accountData.FindUserByIdAsync(userId);
            if (user != null)
            {
                //var roles = await userManager.GetRolesAsync(user);
                //context.Items["Account"] = GetApplicationUserRoles(user, roles);
                context.Items["Account"] = user;
                currentUser.UserInfo = user;
            }
        }
        catch (Exception ex)
        {
            var msg = ex.Message;
            // do nothing if jwt validation fails
            // user is not attached to context so request won't have access to secure routes
        }
    }
}
