using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class SessionSchedule : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
