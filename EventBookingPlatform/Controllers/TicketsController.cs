using EventBookingPlatform.DTOs.Tickets;
using EventBookingPlatform.Extensions;
using EventBookingPlatform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingPlatform.Controllers;

[Authorize]
public class TicketsController : Controller
{
    private readonly ITicketService _ticketService;
    private readonly IEventService _eventService;

    public TicketsController(ITicketService ticketService, IEventService eventService)
    {
        _ticketService = ticketService;
        _eventService = eventService;
    }

    [Authorize(Roles = "Admin,Organizer")]
    public async Task<IActionResult> Index()
    {
        return View(await _ticketService.GetAllAsync());
    }

    public async Task<IActionResult> MyTickets()
    {
        var userId = User.GetUserId();
        if (!userId.HasValue)
        {
            return Challenge();
        }

        return View(await _ticketService.GetByUserAsync(userId.Value));
    }

    public async Task<IActionResult> Details(int id)
    {
        var ticket = await _ticketService.GetByIdAsync(id);
        if (ticket is null)
        {
            return NotFound();
        }

        var userId = User.GetUserId();
        var elevated = User.IsInRole("Admin") || User.IsInRole("Organizer");

        if (!elevated && (!userId.HasValue || ticket.ApplicationUserId != userId.Value))
        {
            return Forbid();
        }

        return View(ticket);
    }

    [HttpGet]
    public async Task<IActionResult> Purchase(int eventId)
    {
        var eventDto = await _eventService.GetByIdAsync(eventId);
        if (eventDto is null)
        {
            return NotFound();
        }

        ViewBag.Event = eventDto;
        return View(new TicketPurchaseDto { EventId = eventId, Quantity = 1 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Purchase(TicketPurchaseDto dto)
    {
        var eventDto = await _eventService.GetByIdAsync(dto.EventId);
        if (eventDto is null)
        {
            return NotFound();
        }

        ViewBag.Event = eventDto;

        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var userId = User.GetUserId();
        if (!userId.HasValue)
        {
            return Challenge();
        }

        var result = await _ticketService.PurchaseAsync(userId.Value, dto);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View(dto);
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction(nameof(MyTickets));
    }

    [Authorize(Roles = "Admin,Organizer")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var dto = await _ticketService.GetEditDtoAsync(id);
        if (dto is null)
        {
            return NotFound();
        }

        return View(dto);
    }

    [Authorize(Roles = "Admin,Organizer")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TicketAdminEditDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var result = await _ticketService.UpdateAsync(dto);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var ticket = await _ticketService.GetByIdAsync(id);
        if (ticket is null)
        {
            return NotFound();
        }

        return View(ticket);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _ticketService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
