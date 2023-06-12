using Dapper;
using Recipes.BO;
using System.Data;

namespace Recipes.BI;

public class AccountDataAccess : IAccountDataAccess
{
    private readonly IConnectionManager _connectionManager;
    public AccountDataAccess(IConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public async Task<UserViewModel> FindUserByIdAsync(long userId)
    {
        UserViewModel userViewModel;
        var param = new DynamicParameters();
        param.Add("@UserId", dbType: DbType.Int64, value: userId, direction: ParameterDirection.Input);

        using var db = _connectionManager.MainConnection();
        using var multi = await db.QueryMultipleAsync("dbo.spUserRole_GetUserByUserId", param, commandType: CommandType.StoredProcedure);
        userViewModel = multi.ReadFirstOrDefault<UserViewModel>();
        userViewModel.Roles = multi.Read<RoleViewModel>().AsList();
        return userViewModel;
    }

    public async Task<UserViewModel> GetUserRoleByEmailAddressAsync(string EmailAddress)
    {
        UserViewModel userViewModel;
        var param = new DynamicParameters();
        param.Add("@EmailAddress", dbType: DbType.String, value: EmailAddress, direction: ParameterDirection.Input);

        using var db = _connectionManager.MainConnection();
        using var multi = await db.QueryMultipleAsync("dbo.spUserRole_GetUserByEmailAddress", param, commandType: CommandType.StoredProcedure);
        userViewModel = multi.ReadFirstOrDefault<UserViewModel>();
        userViewModel.Roles = multi.Read<RoleViewModel>().AsList();
        return userViewModel;
    }

    public async Task<ReturnResponse> LoginUserAsync(LoginUser model)
    {
        var param = new DynamicParameters();
        param.Add("@EmailAddress", dbType: DbType.String, value: model.EmailAddress, direction: ParameterDirection.Input);
        param.Add("@Password", dbType: DbType.String, value: model.Password, direction: ParameterDirection.Input);

        using var db = _connectionManager.MainConnection();
        return await db.QueryFirstOrDefaultAsync<ReturnResponse>("dbo.spUser_LoginUser",
               commandType: CommandType.StoredProcedure, param: param);
    }
}
