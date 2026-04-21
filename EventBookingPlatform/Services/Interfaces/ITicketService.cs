using EventBookingPlatform.DTOs.Tickets;

namespace EventBookingPlatform.Services.Interfaces;

public interface ITicketService
{
    Task<List<TicketListDto>> GetAllAsync();
    Task<List<TicketListDto>> GetByUserAsync(int userId);
    Task<TicketListDto?> GetByIdAsync(int id);
    Task<(bool Succeeded, string Message)> PurchaseAsync(int userId, TicketPurchaseDto dto);
    Task<TicketAdminEditDto?> GetEditDtoAsync(int id);
    Task<(bool Succeeded, string Message)> UpdateAsync(TicketAdminEditDto dto);
    Task DeleteAsync(int id);
}
