using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Recipes.BO;

namespace RecipesWebAPI;

public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] allowedroles;
    public AuthorizeAttribute(params string[] roles)
    {
        allowedroles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
        {
            return;
        }

        var user = (UserViewModel)context.HttpContext.Items["Account"];
        if (user == null)
        {
            context.Result = new JsonResult(new { message = "Unauthorized Access!!!" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
        else
        {
            if (allowedroles.Length > 0)
            {
                if (!IsRoleAuthorized(user.Roles))
                {
                    context.Result = new JsonResult(new { message = "Unauthorized Access!!!" }) { StatusCode = StatusCodes.Status401Unauthorized };
                }
            }
        }
    }

    private bool IsRoleAuthorized(IEnumerable<RoleViewModel> roles)
    {
        foreach (var role in roles)
        {
            bool isAuthorize = Array.Exists(allowedroles, x => x == role.RoleName);
            if (isAuthorize)
            {
                return true;
            }
        }
        return false;
    }

}
