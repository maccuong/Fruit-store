Scaffolding has generated all the files and added the required dependencies.

However the Application's Startup code may require additional changes for things to work end to end.
Add the following code to the Configure method in your Application's Startup class if not already done:

        app.UseEndpoints(endpoints =>
        {
          endpoints.MapControllerRoute(
            name : "areas",
            pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
        });


Scaffold-DbContext "Server=.;Database=Clothing_boutique_web;user id=sa;password=sa2012;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context DatabaseContext -f
dotnet ef dbcontext scaffold "Server=MACTHIENCHICUON;Database=Clothing_boutique_web;user id=sa;password=sa2012;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context DatabaseContext -f

--Code first
B1: dotnet ef migrations add AddCategoryProduct
B2: dotnet ef database update