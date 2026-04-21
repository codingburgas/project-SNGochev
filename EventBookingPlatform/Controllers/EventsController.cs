using EventBookingPlatform.DTOs.Events;
using EventBookingPlatform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventBookingPlatform.Controllers;

public class EventsController : Controller
{
    private readonly IEventService _eventService;
    private readonly ICategoryService _categoryService;

    public EventsController(IEventService eventService, ICategoryService categoryService)
    {
        _eventService = eventService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index(DateTime? fromDate, int? categoryId)
    {
        ViewBag.Categories = new SelectList(await _categoryService.GetAllAsync(), "Id", "Name", categoryId);
        ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
        ViewBag.CategoryId = categoryId;
        return View(await _eventService.GetAllAsync(fromDate, categoryId));
    }

    public async Task<IActionResult> Details(int id)
    {
        var eventDto = await _eventService.GetByIdAsync(id);
        if (eventDto is null)
        {
            return NotFound();
        }

        return View(eventDto);
    }

    [Authorize(Roles = "Admin,Organizer")]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadCategoriesAsync();
        return View(new EventFormDto());
    }

    [Authorize(Roles = "Admin,Organizer")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EventFormDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(dto.CategoryId);
            return View(dto);
        }

        await _eventService.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin,Organizer")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var dto = await _eventService.GetFormByIdAsync(id);
        if (dto is null)
        {
            return NotFound();
        }

        await LoadCategoriesAsync(dto.CategoryId);
        return View(dto);
    }

    [Authorize(Roles = "Admin,Organizer")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EventFormDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(dto.CategoryId);
            return View(dto);
        }

        await _eventService.UpdateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var eventDto = await _eventService.GetByIdAsync(id);
        if (eventDto is null)
        {
            return NotFound();
        }

        return View(eventDto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _eventService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadCategoriesAsync(int? selectedId = null)
    {
        ViewBag.Categories = new SelectList(await _categoryService.GetAllAsync(), "Id", "Name", selectedId);
    }
}
