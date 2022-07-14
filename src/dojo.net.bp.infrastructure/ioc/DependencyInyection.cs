using dojo.net.bp.application.interfaces.DbContexts;
using dojo.net.bp.application.interfaces.repositories;
using dojo.net.bp.infrastructure.data.Context;
using dojo.net.bp.infrastructure.data.repositories;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;


namespace dojo.net.bp.infrastructure.ioc
{
    public static class DependencyInyection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            Log.Logger = new LoggerConfiguration()
                  .ReadFrom
                  .Configuration(configuration).CreateLogger();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<ABP_DBContext, BP_DBContext>();
            services.AddScoped<IPersonasContextRepository, PersonasContextRepository>();
            services.AddScoped<IClientesContextRepository, ClientesContextRepository>();
            services.AddScoped<ICuentasContextRepository, CuentasContextRepository>();
            services.AddScoped<IMovimientosContextRepository, MovimientosContextRepository>();
            services.AddScoped<IReportesRepository, ReportesRepository>();

            return services;
        }
    }
}
