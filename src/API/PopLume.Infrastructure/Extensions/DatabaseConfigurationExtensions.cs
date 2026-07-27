using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PopLume.Infrastructure.DataProvider.Context;

namespace PopLume.Infrastructure.Extensions;

public static class DatabaseConfigurationExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext<PopLumeDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("PopLumeApi");
            options.UseNpgsql(
                connectionString,
                sqlOptions => sqlOptions
                    .CommandTimeout(60)
                    .EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(60), null)
            );
        });
    }
}
