using EventBookingPlatform.Data;
using EventBookingPlatform.DTOs.Account;
using EventBookingPlatform.Models;
using EventBookingPlatform.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventBookingPlatform.Services;

public class AccountService : IAccountService
{
    private readonly AppDbContext _db;
    private readonly PasswordHasher<ApplicationUser> _passwordHasher = new();

    public AccountService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<(bool Succeeded, string ErrorMessage)> RegisterAsync(RegisterDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();

        var exists = await _db.Users.AnyAsync(u => u.Email == email);
        if (exists)
        {
            return (false, "An account with this email already exists.");
        }

        var user = new ApplicationUser
        {
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            Email = email,
            Role = "Attendee"
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return (true, string.Empty);
    }

    public async Task<ApplicationUser?> ValidateUserAsync(LoginDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user is null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        return result == PasswordVerificationResult.Failed ? null : user;
    }

    public async Task<List<ApplicationUser>> GetAllUsersAsync()
    {
        return await _db.Users
            .OrderBy(u => u.FirstName)
            .ThenBy(u => u.LastName)
            .ToListAsync();
    }
}
