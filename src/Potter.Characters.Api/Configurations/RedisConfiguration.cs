using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Potter.Characters.Api.Configurations
{
    public static class RedisConfiguration
    {
        public static void AddRedisConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = configuration.GetSection("Redis")?.GetSection("ConnectionString")?.Value;
                options.InstanceName = "PotterCharacter";
            });
        }
    }
}
