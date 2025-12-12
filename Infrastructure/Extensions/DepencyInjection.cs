using Application.Common;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.Repositories;
using Infrastructure.Data;
using Infrastructure.Data.Interceptors;
using Infrastructure.Messaging;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class DepencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);
        });

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        services.AddSingleton<AuditableEntityInterceptor>();
        services.AddSingleton<DomainEventsInterceptor>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddHostedService<OutBoxProcessor>();

        services.Scan(scan => scan
                          .FromAssembliesOf(typeof(DepencyInjection))
                          .AddClasses(
                              classes => classes.Where(t => t.Name.EndsWith("Repository") ||
                                                           t.Name.EndsWith("Service")))
                          .AsImplementedInterfaces()
                          .WithScopedLifetime()
        );

        // services.AddScoped<IUserRepository, UserRepository>();
        // services.AddScoped<IJwtService, JwtService>();


        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        return services;
    }
}