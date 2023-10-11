using Clothing_boutique_web.Areas.Admin.Models;
using Clothing_boutique_web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Clothing_boutique_web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
    [Route("admin/photo")]
    public class PhotoController : Controller
    {
        private DatabaseContext context;
        private readonly IWebHostEnvironment hostEnvironment;
        public PhotoController(DatabaseContext _context, IWebHostEnvironment _hostEnvironment)
        {
            context = _context;
            this.hostEnvironment = hostEnvironment;
            hostEnvironment = _hostEnvironment;
        }


        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index(int? id)
        {
            int currentPage = id == null ? 1 : id.Value;
            int itemPerPg = 5;
            List<Photo> photoList = context.Photos.ToList();
            return View(await Models.PaginatedList<Photo>.CreateDummyData(currentPage, photoList, itemPerPg));
        }

        // add photo with no specific id product
        [HttpGet]
        [Route("add")]
        public async Task<IActionResult> Add()
        {
            Photo photo = new Photo();
            PopulateProductDropDownList();
            return View(photo);
        }

        // add photo with specific id product
        [HttpGet]
        [Route("add/{id?}")]
        public async Task<IActionResult> Add(int id)
        {
            Photo photo = new Photo() { ProductId = id };
            Product? product = context.Products.FirstOrDefault(p => p.Id == id);
            PopulateProductDropDownList(photo.ProductId);
            return View(photo);
        }

        [HttpPost]
        [Route("add/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Photo photo)
        {
            if(photo.Id != 0)
               photo.Id = 0;
            if (!String.IsNullOrEmpty(photo.ImageFile.FileName))
            {
                string wwwRootPath = hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(wwwRootPath);
                string extension = Path.GetExtension(photo.ImageFile.FileName);
                photo.Name = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/admin/image/", fileName);
                using(var fileStream = new FileStream(path, FileMode.Create))
                {
                    await photo.ImageFile.CopyToAsync(fileStream);
                }
                context.Photos.Add(photo);
            }
            await context.SaveChangesAsync();
            return RedirectToAction("index", "photo", new {Areas = "admin"});
        }

        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            Photo? photo = await context.Photos.FirstOrDefaultAsync(pt => pt.Id == id);
            if (photo != null)
                PopulateProductDropDownList(photo.ProductId);
            return View(photo);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(Photo photo)
        {
            Photo? _photo = await context.Photos.FirstOrDefaultAsync(pt => pt.Id == photo.Id);
            if (_photo != null )
            {
                if(photo.ImageFile != null)
                {
                    string wwwRootPath = hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(wwwRootPath);
                    string extension = Path.GetExtension(photo.ImageFile.FileName);
                    _photo.Name = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/admin/image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await photo.ImageFile.CopyToAsync(fileStream);
                    }
                }
                _photo.ProductId = photo.ProductId;
                _photo.Featured = photo.Featured;
                _photo.Status = photo.Status;
            }
            await context.SaveChangesAsync();
            return RedirectToAction("index", "photo", new { Areas = "admin" });
        }

        [HttpGet]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Photo? photo = await context.Photos.FirstOrDefaultAsync(pr => pr.Id == id);
            if (photo != null)
            {
                context.Photos.Remove(photo);
            }
            await context.SaveChangesAsync();
            return RedirectToAction("index", "photo", new { Areas = "admin" });
        }
        private void PopulateProductDropDownList(object selectedProduct = null)
        {
            var productQuery = from d in context.Products
                                orderby d.Name
                                select d;
            ViewBag.ProductId = new SelectList(productQuery.AsNoTracking(), "Id", "Name", selectedProduct);
        }
    }
}
