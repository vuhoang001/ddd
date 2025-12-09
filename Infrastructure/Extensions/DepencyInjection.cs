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

        services.AddSingleton<AuditableEntityInterceptor>();
        services.AddSingleton<DomainEventsInterceptor>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddHostedService<OutBoxProcessor>();


        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        return services;
    }
}