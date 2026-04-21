using EventBookingPlatform.DTOs.Categories;

namespace EventBookingPlatform.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync();
    Task<CategoryFormDto?> GetByIdAsync(int id);
    Task<CategoryDto?> GetDetailsAsync(int id);
    Task CreateAsync(CategoryFormDto dto);
    Task UpdateAsync(CategoryFormDto dto);
    Task DeleteAsync(int id);
}
