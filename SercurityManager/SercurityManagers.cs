using Clothing_boutique_web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Clothing_boutique_web.SercurityManager
{
    public class SercurityManagers
    {
        public async void SignIn(HttpContext httpContext, Account account)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(GetUserClaim(account), CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await httpContext.SignInAsync(claimsPrincipal);
        }

        //method is used get claim's user
        private IEnumerable<Claim> GetUserClaim(Account account)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, account.Id.ToString()));
            foreach(var roleAccount in account.RoleAccounts){
                claims.Add(new Claim(ClaimTypes.Role, roleAccount.Roles.Name));
            }
            return claims;
        }

        public async void SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }
    }
}
