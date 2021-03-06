﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace Potter.Characters.Api.Configurations
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Potter Characters",
                        Version = "v1",
                        Description = "Esta é uma API REST criada com o ASP.NET Core 3.1, elaborada em cima do serviço PotterApi",
                        Contact = new OpenApiContact
                        {
                            Name = "Fernando Vendramin",
                            Url = new Uri("https://github.com/FernandoVendramin"),
                            Email = "fernando.vendramin@gmail.com"
                        }
                    });
            });
        }
    }
}
