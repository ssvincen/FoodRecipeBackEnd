using Recipes.BO;

namespace Recipes.BI;

public interface IAccountDataAccess
{
    Task<UserViewModel> FindUserByIdAsync(long userId);
    Task<ReturnResponse> LoginUserAsync(LoginUser model);
    Task<UserViewModel> GetUserRoleByEmailAddressAsync(string EmailAddress);

}
