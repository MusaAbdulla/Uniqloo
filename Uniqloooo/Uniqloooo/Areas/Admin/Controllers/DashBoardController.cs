using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Uniqloooo.Areas.Admin.Controllers
{
   
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
