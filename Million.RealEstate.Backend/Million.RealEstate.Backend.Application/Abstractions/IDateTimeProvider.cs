namespace Million.RealEstate.Backend.Application.Abstractions;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
