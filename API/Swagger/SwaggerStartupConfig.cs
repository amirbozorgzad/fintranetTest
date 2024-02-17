using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Swagger;

public class SwaggerStartupConfig
{
    public void AddSwaggerGen(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("TaxAreaV1", new OpenApiInfo { Title = "Tax Area APIs", Version = "v1" });

            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory,
                "*.xml",
                SearchOption.TopDirectoryOnly
            ).ToList();
            xmlFiles.ForEach(xmlFile => c.IncludeXmlComments(xmlFile));
        });
    }

    public void ConfigSwaggerUI(IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/TaxAreaV1/swagger.json", "Tax Area v1"); });
    }
}