using Clothing_boutique_web.Areas.Admin.Models;
using Clothing_boutique_web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Clothing_boutique_web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
<<<<<<< HEAD
    [Route("admin/account")]
=======
    [Route("admin/Account")]
>>>>>>> main
    public class AccountController : Controller
    {
        private readonly IWebHostEnvironment hostEnvironment;
        private DatabaseContext context;

        public AccountController(DatabaseContext _context, IWebHostEnvironment _hostEnvironment)
        {
            context = _context;
            hostEnvironment = _hostEnvironment;
        }

        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index(int? id)
        {
            int currentPage = id == null ? 1 : id.Value;
            int itemPerPg = 5;
            List<Account> AccountList = new List<Account>();
            AccountList = context.Accounts.AsNoTracking().ToList();
            return View(await Models.PaginatedList<Account>.CreateDummyData(currentPage, AccountList, itemPerPg));
        }

        [HttpGet]
        [Route("add")]
        public async Task<IActionResult> Add()
        {
            Account account = new Account();
            return View(account);
        }

        [HttpPost]
        [Route("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Account account)
        {
            if (!CheckAccountProperties(account))
            {
                ModelState.AddModelError(string.Empty, "");
                return View(account);
            }
            else 
            {
                string wwwRootPath = hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(wwwRootPath);
                string extension = Path.GetExtension(account.ImageAvatar.FileName);
                account.AvatarName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/admin/avatar/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await account.ImageAvatar.CopyToAsync(fileStream);
                }
                account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
                context.Accounts.Add(account);
                await context.SaveChangesAsync();
                return RedirectToAction("index", "account", new { Areas = "admin" });
            }
        }

        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            Account account = new Account();
            account = await context.Accounts.SingleOrDefaultAsync(x => x.Id == id);
            return View(account);
        }

        [HttpPost]
        [Route("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Account account)
        {
            Account accUpd = await context.Accounts.SingleOrDefaultAsync(ac => ac.Id == account.Id);
            if(account != null)
            {
                if (!CheckAccountProperties(account))
                {
                    ModelState.AddModelError(string.Empty, "");
                    return View(account);
                }
                else
                {
                    accUpd.UserName = account.UserName;
                    account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
                    accUpd.FullName = account.FullName;
                    accUpd.Email = account.Email;
                    accUpd.Addresss = account.Addresss;
                    accUpd.Phone = account.Phone;
                    string wwwRootPath = hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(wwwRootPath);
                    string extension = Path.GetExtension(account.ImageAvatar.FileName);
                    accUpd.AvatarName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/admin/avatar/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await account.ImageAvatar.CopyToAsync(fileStream);
                    }
                    await context.SaveChangesAsync();
                    return View(accUpd);
                }
            }
            else
            {
                ViewBag.Error = "Update fail";
                return View(account);
            }
            
        }

        [HttpGet]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Account accDel = await context.Accounts.SingleOrDefaultAsync(ca => ca.Id == id);
            context.Accounts.Remove(accDel);    
            await context.SaveChangesAsync();
            return RedirectToAction("index", "account", new { Areas = "Admin" });
        }

        private bool CheckAccountProperties(Account account)
        {
            if(string.IsNullOrEmpty(account.UserName) || 
                account.UserName.Length < 6 || account.UserName.Length > 25) 
                    { return false;}
            else if(string.IsNullOrEmpty(account.Password)) 
            { 
                return false; 
            }
            else if (string.IsNullOrEmpty(account.UserName) || 
                account.UserName.Length < 6 || account.UserName.Length > 25) 
            {
                return false;
            }
            else if(account.ImageAvatar == null)
            { 
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
