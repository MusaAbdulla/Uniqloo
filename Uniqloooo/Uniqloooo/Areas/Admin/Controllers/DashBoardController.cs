using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uniqloooo.Helpers;


namespace Uniqloooo.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles=RoleConstants.Musa)]
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
