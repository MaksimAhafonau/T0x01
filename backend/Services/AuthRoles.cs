using SpaceDC.Models.Enums;

namespace SpaceDC.Services;

public static class AuthRoles
{
    public const string Student = nameof(UserRole.Student);
    public const string Teacher = nameof(UserRole.Teacher);
    public const string Admin = nameof(UserRole.Admin);

    public static string FromRole(UserRole role) => role.ToString();
}
