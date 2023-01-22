using BrandNewDay.Bank.Web.MVC.Website.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BrandNewDay.Bank.Web.MVC.Website.Data;

public class IdentityDataContext : IdentityDbContext<BrandNewDayBankUser>
{
    public IdentityDataContext(DbContextOptions<IdentityDataContext> options)
        : base(options)
    {
    }

    public DbSet<BankAccount> BankAccounts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //The conversion rate from national currency to the euro is expressed with 6 significant figures
        //not to be confused with 6 decimal points
        //https://economy-finance.ec.europa.eu/euro/enlargement-euro-area/adoption-fixed-euro-conversion-rate/converting-euro_en
        builder.Entity<BankAccount>().Property(x => x.Balance).HasPrecision(18, 6);


        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
