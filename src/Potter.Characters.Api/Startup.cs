using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Potter.Characters.Api.Configurations;
using System.IO.Compression;

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
            services.AddMongoDBConfiguration(Configuration); // Configura o Mongo
            services.AddPotterApiConfiguration(Configuration); // Configura serviço com o site PotterApi
            services.AddDependencyInjections(); // Configura a IOC
            services.AddRedisConfiguration(Configuration); // Configura o servior redis
            services.AddSwaggerConfiguration(Configuration); // Configura o swagger

            services.Configure<GzipCompressionProviderOptions>(op => op.Level = CompressionLevel.Optimal); // Adiciona compressão de resposta das APIs

            // Configurações do JSON
            services.AddControllers().AddJsonOptions(op =>
            {
                op.JsonSerializerOptions.IgnoreNullValues = true;
                op.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // Habilitando o Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
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
