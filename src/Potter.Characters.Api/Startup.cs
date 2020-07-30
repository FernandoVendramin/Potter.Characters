using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Potter.Characters.Api.Configurations;

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

            services.AddPotterApiConfiguration(Configuration); // Configura serviço com o site PotterApi
            services.AddDataContext(Configuration); // Configura DataContext
            services.AddDependencyInjections(); // Configura a IOC

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // AutoMigration do banco
            app.AddDataContextConfigureAutoMigrate();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseEndpoints(ep =>
            {
                ep.MapControllers();
            });
        }
    }
}
