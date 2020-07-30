using Microsoft.Extensions.DependencyInjection;
using Potter.Characters.Application.Interfaces;
using Potter.Characters.Application.PotterApi.Interfaces;
using Potter.Characters.Application.PotterApi.Services;
using Potter.Characters.Application.Services;
using Potter.Characters.Domain.Interfaces;
using Potter.Characters.Infra.Context;
using Potter.Characters.Infra.Repositories;
using Potter.Characters.Infra.UnitOfWork;
using System;

namespace Potter.Characters.Api.Configurations
{
    public static class DependencyInjections
    {
        public static void AddDependencyInjections(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // Infra
            services.AddScoped<DataContext>();
            services.AddScoped<ICharacterRepository, CharacterRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Application
            services.AddScoped<ICharacterService, CharacterService>();
        }
    }
}
