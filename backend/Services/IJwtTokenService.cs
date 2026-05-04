using SpaceDC.Models.Enums;

namespace SpaceDC.Services;

public interface IJwtTokenService
{
    string CreateToken(Guid userId, string email, UserRole role);
}
