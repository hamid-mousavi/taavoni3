using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NonFactors.Mvc.Grid;
using Taavoni.Data;
using Taavoni.Models.Entities;
using Taavoni.Services.Interfaces;
using SQLitePCL;
using Microsoft.Data.Sqlite;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http.Features;




var builder = WebApplication.CreateBuilder(args);


// خواندن کانکشن استرینگ از appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// افزودن DbContext به DI container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));  // پشتیبانی از SQLite با رمزگذاری



//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IDebtService, DebtService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IDebtTitleService, DebtTitleService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddRazorPages();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10485760; // 10 MB
});

var app = builder.Build();
// Seed داده‌ها


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await ApplicationDbContext.SeedAsync(userManager, roleManager);
     
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();




app.UseAuthentication(); // اضافه کردن احراز هویت
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.MapRazorPages();

app.Run();
