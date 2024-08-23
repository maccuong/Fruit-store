using Clothing_boutique_web.Areas.Admin.Models;
using Clothing_boutique_web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clothing_boutique_web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
    [Route("admin/Account")]
    public class AccountController : Controller
    {
        private DatabaseContext context;

        public AccountController(DatabaseContext _context)
        {
            context = _context;
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
            //return View();
        }
    }
}
