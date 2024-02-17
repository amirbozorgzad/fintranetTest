using Core.Domain;
using Core.Domain.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Service.Abstraction;
using Service.Implementation;

namespace API.Infrastructure
{
    public class DependencyInjectionStartupConfig
    {
        public DependencyInjectionStartupConfig(IServiceCollection services, IConfiguration configuration)
        {
            SetupDbContexts(services, configuration);
            SetupServices(services);
        }
        

        private static void SetupDbContexts(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CoreContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("CoreDatabase")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void SetupServices(IServiceCollection services)
        {
            services.AddScoped<ITaxCalculatorService, TaxCalculatorService>();
        }

        
    }
}