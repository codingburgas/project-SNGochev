using EventBookingPlatform.Data;
using EventBookingPlatform.DTOs.Categories;
using EventBookingPlatform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventBookingPlatform.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _db;

    public CategoryService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        return await _db.Categories
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                EventsCount = c.Events.Count
            })
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<CategoryFormDto?> GetByIdAsync(int id)
    {
        return await _db.Categories
            .Where(c => c.Id == id)
            .Select(c => new CategoryFormDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CategoryDto?> GetDetailsAsync(int id)
    {
        return await _db.Categories
            .Where(c => c.Id == id)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                EventsCount = c.Events.Count
            })
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync(CategoryFormDto dto)
    {
        _db.Categories.Add(new Models.Category
        {
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim()
        });

        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(CategoryFormDto dto)
    {
        var category = await _db.Categories.FindAsync(dto.Id);
        if (category is null)
        {
            return;
        }

        category.Name = dto.Name.Trim();
        category.Description = dto.Description?.Trim();

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category is null)
        {
            return;
        }

        _db.Categories.Remove(category);
        await _db.SaveChangesAsync();
    }
}
