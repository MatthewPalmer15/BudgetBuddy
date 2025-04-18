using BudgetBuddy.Database.Entities.Configuration;
using BudgetBuddy.Database.Entities.Transactions;
using Microsoft.EntityFrameworkCore;
using ServiceProvider = BudgetBuddy.Database.Entities.Transactions.ServiceProvider;

namespace BudgetBuddy.Database;

public interface IDbContext
{
    DbSet<Transaction> Transactions { get; set; }
    DbSet<ServiceProvider> ServiceProviders { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<Setting> Settings { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}