using Clothing_boutique_web.Areas.Admin.Models;
using Clothing_boutique_web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.Entity;

namespace Clothing_boutique_web.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseContext context;
        public HomeController(DatabaseContext _context)
        {
            context = _context;
        }
        public IActionResult Index()
        {
            ViewBag.ListCate = context.Categories.ToList();
            ViewBag.Categories = context.Categories.Where(ca => ca.Status == true).ToList();
            ViewBag.Products = context.Products.ToList();
            return View();
        }
    }
}
