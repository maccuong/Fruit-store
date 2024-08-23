using Clothing_boutique_web.Areas.Admin.Models;
using Clothing_boutique_web.Areas.Pagination;
using Clothing_boutique_web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace Clothing_boutique_web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
    [Route("admin/categories")]
    public class CategoriesController : Controller
    {
        public DatabaseContext context;
        public CategoriesController(DatabaseContext _context)
        {
            context = _context;
        }

        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index(int? id)
        {
            int currentPage = id == null ? 1: id.Value;
            int itemPerPg = 5;
            List<Category> categoryList = context.Categories.ToList();
            return View(await Models.PaginatedList<Category>.CreateDummyData(currentPage, categoryList, itemPerPg));
        }

        [HttpGet]
        [Route("add")]
        public async Task<IActionResult> Add()
        {
            Category category = new Category();
            return View(category);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(Category category)
        {
            if (category.Name != null)
            {
                context.Categories.Add(category);
                await context.SaveChangesAsync();
                return RedirectToAction("index", "categories", new { Areas = "Admin"});
            }
            else
            {
                ViewBag.Error = "Please input Category's name";
                return View(category);
            }
        }

        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            Category category = await context.Categories.SingleOrDefaultAsync(ca => ca.Id == id);
            return View(category);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(Category category)
        {
            Category cateUpd = await context.Categories.SingleOrDefaultAsync(ca => ca.Id == category.Id);
            if(cateUpd != null)
            {
                cateUpd.Name = category.Name;
                cateUpd.Status = category.Status;
               await context.SaveChangesAsync();
            }
            return RedirectToAction("index", "categories", new {Areas = "Admin"});
        }

        [HttpGet]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Category catedel = await context.Categories.SingleOrDefaultAsync(ca => ca.Id == id);
            context.Categories.Remove(catedel);    
            await context.SaveChangesAsync();
            return RedirectToAction("index", "categories", new { Areas = "Admin" });
        }
    }
}
