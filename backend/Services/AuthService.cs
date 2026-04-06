using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpaceDC.Data;
using SpaceDC.DTOs.Auth;
using SpaceDC.Models;
using SpaceDC.Models.Enums;

namespace SpaceDC.Services;

public sealed class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenService _jwt;

    public AuthService(AppDbContext db, IPasswordHasher<User> passwordHasher, IJwtTokenService jwt)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _jwt = jwt;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var email = NormalizeEmail(request.Email);
        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
            throw new AppException(400, "Password must be at least 6 characters.");

        if (await _db.Users.AnyAsync(u => u.Email == email, cancellationToken))
            throw new AppException(409, "Email is already registered.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Role = UserRole.Student
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
        await _db.Users.AddAsync(user, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return new AuthResponse
        {
            Token = _jwt.CreateToken(user.Id, user.Email, user.Role),
            User = MapUser(user)
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var email = NormalizeEmail(request.Email);
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        if (user is null)
            throw new AppException(401, "Invalid email or password.");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
            throw new AppException(401, "Invalid email or password.");

        return new AuthResponse
        {
            Token = _jwt.CreateToken(user.Id, user.Email, user.Role),
            User = MapUser(user)
        };
    }

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();

    private static UserDto MapUser(User user) => new()
    {
        Id = user.Id,
        Email = user.Email,
        Role = user.Role
    };
}
