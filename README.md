## Million-SrDeveloper-Test
# 🏛️ Million Real Estate Backend

API desarrollada con **.NET 9**, aplicando **Clean Architecture**, **DDD
ligero**, **CQRS**, **MediatR**, **Repository + Unit of Work**,
**FluentValidation**, **Swagger**, y **JWT / OAuth2 / OpenID Connect**.

Este documento describe la arquitectura, decisiones técnicas y buenas
prácticas aplicadas al proyecto.

------------------------------------------------------------------------

# 📐 1. Arquitectura General

La arquitectura implementa **Clean Architecture / Onion Architecture**,
asegurando:

-   Bajo acoplamiento\
-   Alta cohesión\
-   Testabilidad\
-   Escalabilidad\
-   Facilidad de mantenimiento

Estructura en proyectos:

    Million.RealEstate.Backend.Domain
    Million.RealEstate.Backend.Application
    Million.RealEstate.Backend.Infrastructure
    Million.RealEstate.Backend.Api

## 🟣 Domain

-   Entidades de negocio (Owner, Property, PropertyImage,
    PropertyTrace)\
-   Reglas e invariantes del dominio\
-   Excepciones\
-   Interfaces de repositorio\
-   Patrón Specification\
-   Sin dependencias externas

## 🔵 Application

-   Implementación de **CQRS**\
-   **Commands/Queries** con MediatR\
-   Validaciones con **FluentValidation**\
-   DTOs y mapeos con AutoMapper\
-   Casos de uso\
-   Depende solo del Domain

## 🟢 Infrastructure

-   EF Core + SQL Server\
-   Configuración de entidades\
-   Repositorios concretos\
-   Unit of Work\
-   Servicios externos (DateTimeProvider)\
-   Depende de Application + Domain

## 🔴 API

-   Controladores REST\
-   Middlewares (incluye manejador de excepciones)\
-   Swagger / OpenAPI\
-   Configuración de seguridad\
-   Depende de Application e Infrastructure

------------------------------------------------------------------------

# 🧩 2. Estructura de Carpetas

    Domain
     ├─ Common
     ├─ Owners
     ├─ Properties
     ├─ Interfaces
     └─ Specifications

    Application
     ├─ Abstractions
     ├─ Properties
     │   ├─ Commands
     │   └─ Queries
     ├─ Dtos
     └─ Mapping

    Infrastructure
     ├─ Persistence
     │   ├─ Configurations
     │   ├─ DbContext
     ├─ Repositories
     ├─ Services
     └─ DependencyInjection

    Api
     ├─ Controllers
     ├─ Middleware
     ├─ Config
     ├─ Models
     └─ Program.cs

------------------------------------------------------------------------

# 📝 3. Documentación del Código

-   Comentarios XML habilitados\
-   Swagger consumiendo XML Comments\
-   Responsabilidades claras\
-   Métodos cortos y expresivos\
-   DTOs documentados\
-   Código alineado con Clean Code y SOLID

Ejemplo:

``` csharp
/// <summary>
/// Creates a new real estate property for an owner.
/// </summary>
public record CreatePropertyCommand(...);
```

------------------------------------------------------------------------

# 🔧 4. Mejores Prácticas Aplicadas

### ✔ Clean Architecture

Separación clara entre dominio, aplicación, infraestructura y API.

### ✔ DDD Ligero

-   Entidades ricas con comportamiento\
-   Agregados centrados en Property\
-   Reglas de negocio encapsuladas\
-   Sin sobre-ingeniería (Bounded Contexts complejos innecesarios)

### ✔ CQRS + MediatR

-   Commands → modifican estado\
-   Queries → lectura optimizada

### ✔ Repository + Unit of Work

-   Persistencia desacoplada\
-   Pruebas unitarias fáciles\
-   Control explícito de transacciones

### ✔ FluentValidation

Reglas de validación consistentes y desacopladas del controlador.

### ✔ Specification

Aplicado en filtros flexibles para listar propiedades.

### ✔ Middleware de Excepciones

Manejo consistente de errores → 400, 404, 500.

------------------------------------------------------------------------

# ⚡ 5. Manejo del Rendimiento

-   IQueryable optimizado\
-   Uso de AsNoTracking en consultas\
-   AutoMapper ProjectTo para minimizar materialización\
-   Separación CQRS para queries más livianas\
-   Futuros puntos de extensión: caching, redis, paginación, etc.

------------------------------------------------------------------------

# 🧪 6. Pruebas Unitarias (NUnit + Moq + FluentAssertions)

Se implementaron pruebas para:

### ✔ CreatePropertyCommandHandler

-   Owner inexistente\
-   CodeInternal duplicado\
-   Creación exitosa

### ✔ ChangePropertyPriceCommandHandler

-   Propiedad inexistente\
-   Precio actualizado

### ✔ AddPropertyImageCommandHandler

-   Propiedad inexistente\
-   Imagen agregada exitosamente

Las pruebas aseguran:

-   Reglas de dominio correctas\
-   Comportamiento esperado de los casos de uso\
-   Interacciones correctas con repositorios\
-   Control del UnitOfWork

------------------------------------------------------------------------

# 🔐 7. Seguridad: OpenID Connect, OAuth 2.0 y JWT

### ✔ OAuth 2.0

Framework para autorización, soporta:

-   Authorization Code\
-   Client Credentials\
-   Implicit Flow (legacy)

### ✔ OpenID Connect (OIDC)

Protocolo de autenticación basado en OAuth 2.0, agrega:

-   Identidad verificable\
-   Claims de usuario\
-   ID Tokens

### ✔ JWT

Token stateless utilizado para autenticación en API:

-   Eficiente\
-   Firmado\
-   Expirable\
-   Fácil de integrar en ASP.NET

Ejemplo de configuración:

``` csharp
builder.Services.AddAuthentication("Bearer")
 .AddJwtBearer("Bearer", options =>
 {
     options.Authority = "<AUTH_SERVER>";
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateAudience = false
     };
 });
```

------------------------------------------------------------------------

# 🚀 8. Cómo Ejecutar el Proyecto

### 1. Ejecutar el script de la base de datos que esta con el esquema y datos:

    /Data Base/MillionRealStateDb.sql

### 2. Cambiar server de la cadena de conexion en el archivo appsettings.json del proyecto

    "ConnectionStrings": {
      "DefaultConnection": "Server=LAPTOP-87NNGB5P;Database=MillionRealStateDb;Trusted_Connection=True;Encrypt=False"
    },

### 3. Ejecutar API y Abrir Swagger

    https://localhost:7079/swagger/index.html

------------------------------------------------------------------------

# 🧾 9. Conclusión

Esta solución:

✔ Está alineada con estándares modernos de la industria\
✔ Mantiene una arquitectura sólida y escalable\
✔ Aplica principios de DDD ligero\
✔ Implementa CQRS de forma profesional\
✔ Tiene un dominio independiente\
✔ Es altamente testeable\
✔ Incluye validaciones robustas\
✔ Muestra un manejo de errores centralizado\
✔ Usa seguridad moderna con JWT/OAuth2/OpenID

Lista para producción, ampliación o evaluación técnica.

------------------------------------------------------------------------

# ✨ Autor

**Marlon Orlando Cardona Jaramillo**  
- 💼 Desarrollador Fullstack | .NET, Node.js, Python, Angular, React | Cloud (Azure & AWS) 
- 📧 marlon18_@hotmail.com
- 🔗 [LinkedIn](www.linkedin.com/in/marlon880215)

---

## 📄 Licencia

Este proyecto se entrega bajo la licencia **MIT**.  
Eres libre de usar, modificar y distribuir este código, siempre y cuando se mantenga la atribución al autor original.

---

## 🙌 Créditos

Este proyecto fue desarrollado como parte de la **Prueba Técnica – Desarrollador SrDeveloper** para una empresa del sector inmobiliario.
**Million Real Estate Backend -- Arquitectura Profesional (.NET 9)**
