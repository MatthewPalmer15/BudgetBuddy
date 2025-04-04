using BudgetBuddy.Services.Encryption;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BudgetBuddy.Data.Design;

public class BudgetBuddyDbContextFactory : IDesignTimeDbContextFactory<BudgetBuddyDbContext>
{
    public BudgetBuddyDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BudgetBuddyDbContext>();
        optionsBuilder.UseSqlite(
            "");
        return new BudgetBuddyDbContext(new EncryptionService());
    }
}