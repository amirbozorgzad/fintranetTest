using System.Text.Json.Serialization;
using API.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace API.Infrastructure
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
            new DotNetStartupConfig().ServiceConfig(services);
            new SwaggerStartupConfig().AddSwaggerGen(services, Configuration);
            new DependencyInjectionStartupConfig(services, Configuration);
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .AddJsonOptions(
                    options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });
           
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseResponseCaching();
            new DotNetStartupConfig().AppEnvironmentConfig(app, env);
            new DotNetStartupConfig().AppConfig(app);
            new SwaggerStartupConfig().ConfigSwaggerUI(app, Configuration);
            app.Map("",
                builder => { builder.Run(async context => { context.Response.Redirect("/swagger/index.html"); }); });
        }
    }
}