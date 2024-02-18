using System.Text.Json.Serialization;
using API.Swagger;
using Core.Domain.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Infrastructure;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

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
        new DotNetStartupConfig().AppEnvironmentConfig(app, env);
        new DotNetStartupConfig().AppConfig(app);
        new SwaggerStartupConfig().ConfigSwaggerUI(app, Configuration);
        app.Map("",
            builder => { builder.Run(async context => { context.Response.Redirect("/swagger/index.html"); }); });
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<CoreContext>();
            DbInitializer.Initialize(context);
        }
    }
}