using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lab6.Core.Dal;

internal static class Extensions
{
    public static IServiceCollection AddDatabase<TContext>(this IServiceCollection services, IConfiguration configuration)
        where TContext : DbContext
    {
        _ = bool.TryParse(configuration.GetValue<string>("Postgres:IsEnable"), out bool isPostgresEnable);

        _ = bool.TryParse(configuration.GetValue<string>("SqlServer:IsEnable"), out bool isSqlEnable);

        _ = bool.TryParse(configuration.GetValue<string>("Memory:IsEnable"), out bool isMemoryEnable);

        _ = bool.TryParse(configuration.GetValue<string>("Lite:IsEnable"), out bool isLiteEnable);

        services.AddDbContext<TContext>(dbContextOptionsBuilder =>
        {
            if (isPostgresEnable)
            {
                var connectionString = configuration.GetConnectionString("postgres_connection");

                dbContextOptionsBuilder.UseNpgsql(connectionString);
            }

            if (isSqlEnable)
            {
                var connectionString = configuration.GetConnectionString("sql_connection");

                dbContextOptionsBuilder.UseSqlServer(connectionString);
            }

            if (isMemoryEnable)
            {
                dbContextOptionsBuilder.UseInMemoryDatabase("InMemoryDb");
            }

            if (isLiteEnable)
            {
                var connectionString = configuration.GetConnectionString("lite_connection");

                dbContextOptionsBuilder.UseSqlite(connectionString);
            }
        });

        services.AddHostedService<DatabaseInitializer<TContext>>();

        return services;
    }
}
