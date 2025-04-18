using BudgetBuddy.Database.Entities.Configuration;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Database;

public interface IDbContext
{
    DbSet<Setting> Settings { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}