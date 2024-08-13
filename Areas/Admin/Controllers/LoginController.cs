using Clothing_boutique_web.Models;
using Clothing_boutique_web.SercurityManager;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Net;
using System.Security.Claims;

namespace Clothing_boutique_web.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/login")]
    public class LoginController : Controller
    {
        private DatabaseContext context = new DatabaseContext();
        private SercurityManagers sercurityManager = new SercurityManagers();

        public LoginController(DatabaseContext _context)
        {
            context = _context;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("process")]
        public IActionResult Process(string userName, string password)
        {
            var account = ProcessLogin(userName, password);
            if (account != null)
            {
                sercurityManager.SignIn(this.HttpContext, account);
                SetCurrentUser(this.HttpContext, account);
                return RedirectToAction("index", "dashboard", new { Areas = "Admin"});
            }
            else
            {
                ViewBag.Error = "Invalid Account";
                return View("index");
            }
        }

        private Account ProcessLogin(string userName, string password)
        {
            var account = context.Accounts.SingleOrDefault(a => a.UserName.Equals(userName) && a.Status == true);

            if (account != null)
            {
                var roleOfAccount = account.RoleAccounts.FirstOrDefault();
                if (roleOfAccount.RoleId == 1 && roleOfAccount.Status == true && BCrypt.Net.BCrypt.Verify(password, account.Password)) 
                    return account;
            }
            return null;
        }

        [Route("signout")]
        public IActionResult Signout()
        {
            sercurityManager.SignOut(this.HttpContext);
            return RedirectToAction("index", "login", new { Areas = "Admin" });
        }

        [HttpGet]
        [Route("profile")]
        public IActionResult Profile()
        {
            string Id = HttpContext.User.Identity.Name;
            Account account = context.Accounts.SingleOrDefault(x => x.Id == Convert.ToInt32(Id));
            return View(account);
        }

        [HttpPost]
        [Route("profile")]
        public IActionResult Profile(Account account)
        {
            var accUpd = context.Accounts.FirstOrDefault(a => a.Id.Equals(account.Id));
            if(accUpd != null)
            {
                if(!String.IsNullOrEmpty(account.Password))
                {
                    accUpd.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
                }
                accUpd.UserName = account.UserName;
                accUpd.FullName = account.FullName;
                accUpd.Email = account.Email;
                accUpd.Addresss = account.Addresss;
                accUpd.Phone = account.Phone;
                context.Accounts.Update(accUpd);
                context.SaveChangesAsync();
                SetCurrentUser(this.HttpContext, accUpd);
            }
            return View("profile", accUpd);
        }

        private void SetCurrentUser(HttpContext Context, Account account)
        {
            var session = Context.Session;
            string key_access = "current_user";

            // save current user in session
            Account current_user = new Account
            {
                Id = account.Id,
                UserName = account.UserName
            };

            // doc chuoi luu trong session voi key key_access
            string json = session.GetString(key_access);
            dynamic lastCurrentInfo;
            if (json != null)
            {
                lastCurrentInfo = JsonConvert.DeserializeObject(json, current_user.GetType());
            }
            else
            {
                lastCurrentInfo = current_user;
            }

            Account user_update = new Account
            {
                Id = lastCurrentInfo.Id,
                UserName = lastCurrentInfo.UserName,
            };

            string jsonSave = JsonConvert.SerializeObject(user_update);
            session.SetString(key_access, jsonSave);

        }

        private Account GetUser(HttpContext context)
        {
            var session = context.Session;
            string key = "current_user";
            Account account = new Account
            {
                Id = 0,
                UserName = ""
            };
            string jsonUser = session.GetString(key);
            dynamic user;
            if (jsonUser != null)
            {
                user = JsonConvert.DeserializeObject(jsonUser, account.GetType());
            }
            else
            {
                user = account;
            }
            return user;
        }
    }
}