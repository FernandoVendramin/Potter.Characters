using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Potter.Characters.Api.Configurations;
using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace Potter.Characters.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // implementar LOGs !!!!!!!!!
            services.AddMongoDBConfiguration(Configuration); // Configura o Mongo
            services.AddPotterApiConfiguration(Configuration); // Configura serviço com o site PotterApi
            services.AddDependencyInjections(); // Configura a IOC

            services.Configure<GzipCompressionProviderOptions>(op => op.Level = CompressionLevel.Optimal); // Adiciona compressão de resposta das APIs
            services.AddControllers().AddJsonOptions(op => { op.JsonSerializerOptions.IgnoreNullValues = true; }); // Remove valores nulos da API

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Potter Characters",
                        Version = "v1",
                        Description = "Esta é uma API REST criada com o ASP.NET Core 3.1",
                        Contact = new OpenApiContact
                        {
                            Name = "Fernando Vendramin",
                            Url = new Uri("https://github.com/FernandoVendramin")
                        }
                    });

                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                //swagger.IncludeXmlComments(xmlPath);

                /*
                 <PropertyGroup>
                  <GenerateDocumentationFile>true</GenerateDocumentationFile>
                  <NoWarn>$(NoWarn);1591</NoWarn>
                </PropertyGroup>
                */
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // Ativando Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Potter Characters - v1");
            });

            app.UseRouting();

            app.UseEndpoints(ep =>
            {
                ep.MapControllers();
            });
        }
    }
}
