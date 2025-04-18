using BlazorHybrid.Database.Entities.Configuration;
using Microsoft.EntityFrameworkCore;

namespace BlazorHybrid.Database;

public interface IDbContext
{
    DbSet<Setting> Settings { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}