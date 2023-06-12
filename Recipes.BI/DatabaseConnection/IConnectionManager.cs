using System.Data;

namespace Recipes.BI;

public interface IConnectionManager
{
    IDbConnection MainConnection();
}
