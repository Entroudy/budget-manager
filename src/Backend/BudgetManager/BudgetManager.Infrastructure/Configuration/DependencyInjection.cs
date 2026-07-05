using BudgetManager.Infrastructure.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace BudgetManager.Infrastructure.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    connectionString, ServerVersion.AutoDetect(connectionString),
                    b =>
                    {
                        b.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                        b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                        b.MigrationsHistoryTable("_migrations_history");
                    }
        ));

        services.AddSingleton<IUserAccessor, UserAccessor>();
        services.AddQuartz();

        return services;
    }
}
