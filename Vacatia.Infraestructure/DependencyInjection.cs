using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vacatia.Application.Common.Interfaces;
using Vacatia.Domain.Interfaces;
using Vacatia.Domain.Services;
using Vacatia.Infraestructure.Identity;
using Vacatia.Infraestructure.Persistance;
using Vacatia.Infraestructure.Persistance.Repositories;

namespace Vacatia.Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            // PostgreSQL para EF Core
            services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Vacatia.Infraestructure")));

            // Repositorios
            services.AddScoped<ISolicitudRepository, SolicitudRepository>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            // Servicios del dominio
            services.AddScoped<SolicitudDomainService>();

            // Servicios de usuario actual
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}
