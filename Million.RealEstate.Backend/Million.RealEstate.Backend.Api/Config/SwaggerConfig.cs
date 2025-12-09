using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Million.RealEstate.Backend.Api.Config;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Million Real Estate API",
                Version = "v1",
                Description = "API for managing properties, owners and related resources",
                Contact = new OpenApiContact
                {
                    Name = "Million Luxury",
                    Email = "support@millionluxury.com"
                }
            });

            // XML comments (si ya lo tenías configurado)
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

            // 🔐 Esquema de seguridad JWT Bearer
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Introduce el token JWT con el esquema **Bearer**. Ejemplo: `Bearer eyJhbGci...`",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            options.AddSecurityDefinition("Bearer", securityScheme);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, Array.Empty<string>() }
            });
        });

        return services;
    }
}
