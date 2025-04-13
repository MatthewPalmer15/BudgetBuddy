using BudgetBuddy.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Data;

public interface IDbContext
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Entities.ServiceProvider> ServiceProviders { get; set; }
    public DbSet<Category> Categories { get; set; }
}