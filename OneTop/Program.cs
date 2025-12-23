using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OneTop.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ClothingStoreContext>(
options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyDB")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

// Area route: ensure requests like /Admin or /Account map to area controllers
app.MapAreaControllerRoute(
    name: "Admin_area",
    areaName: "Admin",
    pattern: "Admin/{controller=DashBoard}/{action=DashBoard}/{id?}");

app.MapAreaControllerRoute(
    name: "Account_area",
    areaName: "Account",
    pattern: "Account/{controller=Home}/{action=Home}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();