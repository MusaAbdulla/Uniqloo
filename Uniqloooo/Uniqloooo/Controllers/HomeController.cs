using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Uniqloooo.Context;
using Uniqloooo.Models;

namespace Uniqloooo.Controllers
{
    public class HomeController(UniqloDb _context) : Controller
    {
        
        public async Task <IActionResult> Index()
        {
            return View(await _context.sliders.ToListAsync());
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
