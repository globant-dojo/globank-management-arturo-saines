using AutoMapper;
using dojo.net.bp.application.interfaces.DbContexts;
using dojo.net.bp.application.interfaces.services;
using dojo.net.bp.application.Mapper;
using dojo.net.bp.application.services;
using Microsoft.Extensions.DependencyInjection;

namespace dojo.net.bp.application.ioc
{
    public static class DependencyInyection
    {

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //Adding Mapping Profile
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);


            //Adding DI for Services in Application layer
            services.AddScoped<IClientesService, ClientesService>();
            services.AddScoped<ICuentasService, CuentasService>();
            services.AddScoped<IMovimientosService, MovimientosService>();
            services.AddScoped<IReportesService, ReportesService>();

            return services;
        }
    }
}
