using Million.RealEstate.Backend.Application.Abstractions;

namespace Million.RealEstate.Backend.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
