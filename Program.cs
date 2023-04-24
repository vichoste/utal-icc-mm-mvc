using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Utal.Icc.Mm.Mvc.Data;
using Utal.Icc.Mm.Mvc.Models;
using Utal.Icc.Mm.Mvc.Seeders;

var builder = WebApplication.CreateBuilder(args);
var defaultConnection = builder.Environment.IsDevelopment() ? builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Database string is not set") : Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<IccDbContext>(options => options.UseSqlServer(defaultConnection));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentity<IccUser, IdentityRole>().AddEntityFrameworkStores<IccDbContext>().AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

var app = builder.Build();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var configuration = services.GetRequiredService<IConfiguration>();
var environment = services.GetRequiredService<IWebHostEnvironment>();
await CareerDirectorSeeder.SeedAsync(services, configuration, environment);
await RoleSeeder.SeedAsync(services);
if (!app.Environment.IsDevelopment()) {
	_ = app.UseExceptionHandler("/Home/Error");
	_ = app.UseHsts();
}
_ = app.UseHttpsRedirection();
_ = app.UseStaticFiles();
_ = app.UseRouting();
_ = app.UseAuthentication();
_ = app.UseAuthorization();
_ = app.MapAreaControllerRoute(
	name: "Account",
	areaName: "Account",
	pattern: "Account/{controller=SignIn}/{action=Index}/{id?}"
);
_ = app.MapAreaControllerRoute(
	name: "University",
	areaName: "University",
	pattern: "University/{controller=Memoir}/{action=Index}/{id?}"
);
_ = app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}"
);
_ = app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();