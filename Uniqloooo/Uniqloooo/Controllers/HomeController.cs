using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Uniqloooo.Models;

namespace Uniqloooo.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
    }
}
