using Microsoft.Extensions.DependencyInjection;
using Potter.Characters.Application.Interfaces;
using Potter.Characters.Application.Services;
using Potter.Characters.Domain.Interfaces;
using Potter.Characters.Infra.Repositories;
using System;

namespace Potter.Characters.Api.Configurations
{
    public static class DependencyInjections
    {
        public static void AddDependencyInjections(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // Infra
            //services.AddScoped<DataContext>();
            services.AddScoped<ICharacterRepository, CharacterRepository>();
            //services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Application
            services.AddScoped<ICharacterService, CharacterService>();
            services.AddScoped<IHouseService, HouseService>();
        }
    }
}
