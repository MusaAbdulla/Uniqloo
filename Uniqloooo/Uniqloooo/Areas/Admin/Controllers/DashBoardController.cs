using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD

namespace Uniqloooo.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize]
=======
using System.Web.Http;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace Uniqloooo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
>>>>>>> 42ae760e56ef6a7a4df6362ba101bbd7547e117f
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
