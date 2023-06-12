using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Recipes.BI;

public class ConnectionManager : IConnectionManager
{
    public IConfiguration Configuration { get; }
    public ConnectionManager(IConfiguration config)
    {
        Configuration = config;
    }


    public IDbConnection MainConnection()
    {
        return new SqlConnection(Configuration.GetConnectionString("MainDB"));
    }
}
