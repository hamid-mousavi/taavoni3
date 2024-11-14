using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Taavoni.Models;
using Taavoni.Models.Entities;


namespace Taavoni.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
     : base(options)
    {
    }

    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // بررسی وجود رول "Admin"
        var roleExist = await roleManager.RoleExistsAsync("Admin");
        if (!roleExist)
        {
            var role = new IdentityRole("Admin");
            await roleManager.CreateAsync(role);
        }

        // بررسی وجود کاربر ادمین
        var user = await userManager.FindByEmailAsync("admin@example.com");
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com"
            };
            await userManager.CreateAsync(user, "AdminPassword123!");
        }

        // افزودن کاربر به رول "Admin" اگر هنوز اضافه نشده

        if (!await userManager.IsInRoleAsync(user, "Admin"))
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }

    }







    public DbSet<Debt> Debts { get; set; }  // اضافه کردن DbSet
    public DbSet<FixedDebt> FixedDebts { get; set; }  // اضافه کردن DbSet
    public DbSet<Payment> Payments { get; set; }
    public DbSet<DebtTitle> debtTitles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.Debts)
            .WithOne(d => d.User)
            .HasForeignKey(d => d.UserId);

        builder.Entity<DebtTitle>()
        .HasMany(u => u.debts)
        .WithOne(s => s.debtTitle)
        .HasForeignKey(s => s.DebtTitleId);

        // رابطه بین User و Payment
        builder.Entity<Payment>()
            .HasOne(p => p.User)
            .WithMany(u => u.payments)
            .HasForeignKey(p => p.UserId);


        // رابطه بین Debt و Payment
        builder.Entity<Payment>()
            .HasOne(p => p.Debt)
            .WithMany(d => d.Payments)
            .HasForeignKey(p => p.DebtId);

    }
}
