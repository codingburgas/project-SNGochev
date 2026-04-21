using EventBookingPlatform.DTOs.Categories;
using EventBookingPlatform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingPlatform.Controllers;

public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _categoryService.GetAllAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var category = await _categoryService.GetDetailsAsync(id);
        if (category is null)
        {
            return NotFound();
        }

        return View(category);
    }

    [Authorize(Roles = "Admin,Organizer")]
    [HttpGet]
    public IActionResult Create()
    {
        return View(new CategoryFormDto());
    }

    [Authorize(Roles = "Admin,Organizer")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryFormDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        await _categoryService.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin,Organizer")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category is null)
        {
            return NotFound();
        }

        return View(category);
    }

    [Authorize(Roles = "Admin,Organizer")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryFormDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        await _categoryService.UpdateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryService.GetDetailsAsync(id);
        if (category is null)
        {
            return NotFound();
        }

        return View(category);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _categoryService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
