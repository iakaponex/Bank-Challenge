using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BrandNewDay.Bank.Web.MVC.Website.Data;
using BrandNewDay.Bank.Web.MVC.Website.Services;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("IdentityDataContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityDataContextConnection' not found.");

var env = builder.Environment;

builder.Services.AddSingleton<BankService>(b => new BankService(env) { _webApiUrl = "https://localhost:7127/" });

builder.Services.AddDbContext<IdentityDataContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<BrandNewDay.Bank.Web.MVC.Website.Areas.Identity.Data.BrandNewDayBankUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<IdentityDataContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages(); 
});

app.Run();
