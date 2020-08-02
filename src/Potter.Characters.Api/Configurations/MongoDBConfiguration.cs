using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Potter.Characters.Infra.Configuration;
using Potter.Characters.Infra.Interfaces;
using System;

namespace Potter.Characters.Api.Configurations
{
    public static class MongoDBConfiguration
    {
        public static void AddMongoDBConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // Carrega as configurações do Mongo
            services.Configure<MongoConfig>(configuration.GetSection(nameof(MongoConfig)));
            services.AddSingleton<IMongoConfig>(x => x.GetRequiredService<IOptions<MongoConfig>>().Value);

            services.AddSingleton<IMongoClient>(new MongoClient(configuration.GetSection("MongoConfig")?.GetSection("ConnectionString")?.Value));
        }
    }
}
