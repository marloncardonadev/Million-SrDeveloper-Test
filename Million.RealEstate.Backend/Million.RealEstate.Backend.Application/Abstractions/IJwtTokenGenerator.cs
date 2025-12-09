namespace Million.RealEstate.Backend.Application.Abstractions.Security;

public interface IJwtTokenGenerator
{
    string GenerateToken(int userId, string userName, string email);
}
