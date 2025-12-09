using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Million.RealEstate.Backend.Application.Abstractions;
using Million.RealEstate.Backend.Application.Abstractions.Security;
using Million.RealEstate.Backend.Domain.Interfaces;
using Million.RealEstate.Backend.Infrastructure.Persistence;
using Million.RealEstate.Backend.Infrastructure.Repositories;
using Million.RealEstate.Backend.Infrastructure.Security;
using Million.RealEstate.Backend.Infrastructure.Services;

namespace Million.RealEstate.Backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<RealEstateDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IPropertyRepository, PropertyRepository>();
        services.AddScoped<IOwnerRepository, OwnerRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<RealEstateDbContext>());

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        //services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
