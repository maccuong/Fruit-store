using Clothing_boutique_web.Areas.Admin.Models;
using Clothing_boutique_web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Clothing_boutique_web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
    [Route("admin/product")]
    public class ProductController : Controller
    {
        private DatabaseContext context;
        public ProductController(DatabaseContext _context)
        {
            context = _context;
        }
        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index(int? id)
        {
            int currentPage = id == null ? 1 : id.Value;
            int itemPerPg = 5;
            List<Product> productList = context.Products.ToList();
            return View(await Models.PaginatedList<Product>.CreateDummyData(currentPage, productList, itemPerPg));
        }

        [HttpGet]
        [Route("add")]
        public async Task<IActionResult> Add()
        {
            Product product = new Product();
            PopulateCategoryDropDownList();
            return View(product);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(Product product)
        {

            if (product != null)
            {
                  
                    if (product.CategoryId == 0)
                    {
                        //ModelState.AddModelError("CategoryId", "Please choose category of product  !");
                        ViewBag.ErrorCate = "Please choose category of product  !";
                        PopulateCategoryDropDownList();
                        return View(product);
                    }

                    // if the Quaity is 0
                    else if (product.Quaity == 0)
                    {
                        ModelState.AddModelError("Quatity", "Quatity of product don't allow equal 0  !");
                        PopulateCategoryDropDownList();
                        return View(product);
                    }
                    else
                    {
                        product.DateofInsert = DateTime.Now;
                        context.Products.Add(product);
                        await context.SaveChangesAsync();
                    }

            }
            return RedirectToAction("index","product", new {Areas = "admin"});
        }

        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            Product product =  context.Products.Find(id);
            PopulateCategoryDropDownList(product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(Product product)
        {
            Product _product = await context.Products.SingleOrDefaultAsync(pr => pr.Id.Equals(product.Id));
            if (_product != null)
            {
                _product.Name = product.Name;
                _product.Description = product.Description;
                _product.Detail = product.Detail;
                _product.CategoryId = product.CategoryId;
                _product.Quaity = product.Quaity;
                _product.Price  = product.Price;
                _product.Featured = product.Featured;
                await context.SaveChangesAsync();
            }
            return RedirectToAction("index", "product", new { Areas = "admin" });
        }

        [HttpGet]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await context.Products.FirstOrDefaultAsync(pr => pr.Id == id);
            if(product != null)
            {
                context.Products.Remove(product);
            }
            await context.SaveChangesAsync();
            return RedirectToAction("index", "product", new {Areas = "admin"});
        }

        private void PopulateCategoryDropDownList(object selectedCategory = null)
        {
            var categoryQuery = from d in context.Categories
                                   orderby d.Name
                                   select d;
            ViewBag.CategoryId = new SelectList(categoryQuery.AsNoTracking(), "Id", "Name", selectedCategory);
        }

    }
}
