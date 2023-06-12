using Recipes.BO;
using System.Text;

namespace RecipesWebAPI;

public static class CryptoHelper
{
    public static string PasswordHash(string value)
    {
        return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create()
            .ComputeHash(Encoding.UTF8.GetBytes(value)));
    }

    public static ReturnResponse ComparePassword(string password, string confirmPassword)
    {
        var response = new ReturnResponse
        {
            IsSuccess = false,
            Message = "Password don't match"
        };
        if (string.Compare(password, confirmPassword, true) == 0)
        {
            response.IsSuccess = true;
            response.Message = "Succeess Password Match";
        }
        return response;
    }
}
