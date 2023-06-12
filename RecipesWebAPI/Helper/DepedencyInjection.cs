using Microsoft.Extensions.DependencyInjection.Extensions;
using Recipes.BI;

namespace RecipesWebAPI.Helper
{
    public static class DepedencyInjection
    {
        public static IServiceCollection CustomInjection(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<CurrentUser, CurrentUser>();
            services.AddTransient<IConnectionManager, ConnectionManager>();
            services.AddScoped<IAccountDataAccess, AccountDataAccess>();
            services.AddScoped<IRecipesDataAccess, RecipesDataAccess>();
            return services;
        }
    }
}
