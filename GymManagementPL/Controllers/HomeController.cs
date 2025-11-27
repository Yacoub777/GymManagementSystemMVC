using System.Diagnostics;
using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Entities;
using GymManagementPL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public HomeController(IAnalyticsService analyticsService)
        {
            this._analyticsService = analyticsService;
        }
        public IActionResult Index()
        {
            var ViewData = _analyticsService.GetAnalyticsData();
            return View(ViewData);
        }

    }

}
