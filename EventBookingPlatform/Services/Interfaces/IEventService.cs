using EventBookingPlatform.DTOs.Events;

namespace EventBookingPlatform.Services.Interfaces;

public interface IEventService
{
    Task<List<EventListDto>> GetAllAsync(DateTime? fromDate, int? categoryId);
    Task<EventDetailsDto?> GetByIdAsync(int id);
    Task<EventFormDto?> GetFormByIdAsync(int id);
    Task CreateAsync(EventFormDto dto);
    Task UpdateAsync(EventFormDto dto);
    Task DeleteAsync(int id);
}
