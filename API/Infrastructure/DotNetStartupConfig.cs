using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

namespace API.Infrastructure
{
    public class DotNetStartupConfig
    {
        public void ServiceConfig(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.AllowEmptyInputInBodyModelBinding = true;
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;

            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.MaxDepth = 32;
            })
            .ConfigureApiBehaviorOptions(option => option.SuppressMapClientErrors = true);


            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
        }

        public void AppEnvironmentConfig(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
        }

        public void AppConfig(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseHttpsRedirection();
        }
    }
}
