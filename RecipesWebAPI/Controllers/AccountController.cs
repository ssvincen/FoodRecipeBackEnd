using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.BI;
using Recipes.BO;
using System.IdentityModel.Tokens.Jwt;

namespace RecipesWebAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountDataAccess _accountData;
        private readonly IConfiguration _configuration;


        public AccountController(ILogger<AccountController> logger, IAccountDataAccess accountData, IConfiguration configuration)
        {
            _logger = logger;
            _accountData = accountData;
            _configuration = configuration;
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUser model)
        {
            try
            {
                //{
                //    "emailAddress": "ss.vincen@gmail.com",
                //    "password": "Admin"
                //}
                var signingKey = _configuration["JWT:Secret"];
                var resp = await _accountData.LoginUserAsync(new LoginUser() { EmailAddress = model.EmailAddress, Password = CryptoHelper.PasswordHash(model.Password) });
                if (resp.IsSuccess)
                {
                    var userRoles = await _accountData.GetUserRoleByEmailAddressAsync(model.EmailAddress);
                    var jwtToken = JwtHelper.GetJwtTokens(userRoles, signingKey);
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        expires = jwtToken.ValidTo.ToLocalTime(),
                        username = userRoles.EmailAddress,
                        firstName = userRoles.FirstName,
                        roles = userRoles.Roles
                    });
                }
                return StatusCode(StatusCodes.Status404NotFound, new ReturnResponse { Status = "Error", Message = "Invalid Login Details!!!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to Login. Username {model.EmailAddress}");
                return StatusCode(StatusCodes.Status400BadRequest, new ReturnResponse { Status = "Error", Message = ex.Message });
            }
        }

    }
}
