using Microsoft.AspNetCore.Identity;

namespace GJ.Services;

public class PasswordHasherService
{
    private readonly IPasswordHasher<object> _passwordHasher;

    public PasswordHasherService(IPasswordHasher<object> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(null, password);
    }

    public PasswordVerificationResult VerifyPassword(string hashedPassword, string providedPassword)
    {
        return _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
    }
}