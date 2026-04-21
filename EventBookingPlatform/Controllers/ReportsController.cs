using EventBookingPlatform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingPlatform.Controllers;

[Authorize(Roles = "Admin,Organizer")]
public class ReportsController : Controller
{
    private readonly IStatisticsService _statisticsService;

    public ReportsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate)
    {
        var from = fromDate ?? DateTime.UtcNow.Date.AddMonths(-1);
        var to = toDate ?? DateTime.UtcNow.Date;

        if (to < from)
        {
            to = from;
        }

        var model = await _statisticsService.GetDashboardAsync(from, to);
        return View(model);
    }
}
