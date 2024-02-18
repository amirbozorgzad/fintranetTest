using Core.Domain;
using Core.Domain.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Service.Abstraction;
using Service.Implementation;

namespace API.Infrastructure;

public class DependencyInjectionStartupConfig
{
    public DependencyInjectionStartupConfig(IServiceCollection services, IConfiguration configuration)
    {
        SetupDbContexts(services, configuration);
        SetupServices(services);
    }


    private static void SetupDbContexts(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               "Server=.;Initial Catalog=TaxDb;Encrypt=False;TrustServerCertificate=true;Trusted_Connection=True;";
        services.AddDbContext<CoreContext>(options => options
            .UseSqlServer(connectionString));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void SetupServices(IServiceCollection services)
    {
        services.AddScoped<ITaxCalculatorService, TaxCalculatorService>();
    }
}