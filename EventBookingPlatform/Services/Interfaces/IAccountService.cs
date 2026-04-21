using EventBookingPlatform.DTOs.Account;
using EventBookingPlatform.Models;

namespace EventBookingPlatform.Services.Interfaces;

public interface IAccountService
{
    Task<(bool Succeeded, string ErrorMessage)> RegisterAsync(RegisterDto dto);
    Task<ApplicationUser?> ValidateUserAsync(LoginDto dto);
    Task<List<ApplicationUser>> GetAllUsersAsync();
}
