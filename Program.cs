using Clothing_boutique_web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation()
    .AddNewtonsoftJson(option =>
                option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
var strConnect = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DatabaseContext>(options => options.UseLazyLoadingProxies().UseSqlServer(strConnect));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(
    cfg => {                  
        cfg.Cookie.Name = "chicuong";  
        cfg.IdleTimeout = new TimeSpan(0, 60, 0);
        cfg.IOTimeout = new TimeSpan(0, 60, 0);
    });               
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
     {
         option.LoginPath = "/admin/login/index";
         option.LogoutPath = "/admin/login/signout";
         option.AccessDeniedPath = "/admin/login/accessdenied";
         option.ExpireTimeSpan = new TimeSpan(0, 60, 0);
     });


var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapDefaultControllerRoute();

await app.RunAsync();
