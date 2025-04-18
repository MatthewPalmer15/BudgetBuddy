// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using BlazorHybrid.Database.Entities.Configuration;
using BlazorHybrid.Database.Extensions;
using BlazorHybrid.Infrastructure.Encryption;
using Microsoft.EntityFrameworkCore;

namespace BlazorHybrid.Database;

/// <summary>
///     Athena DB Context without ASP.NET Identity
/// </summary>
public class ApplicationDbContext(IEncryptionService encryptionService) : DbContext, IDbContext
{
    public DbSet<Setting> Settings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");
        builder.HasDefaultSchema("dbo");
        builder.ApplyConfigurations();
        builder.UseEncryption(encryptionService.EncryptionProvider);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={Path.Combine(AppContext.BaseDirectory, "app.db")}");
        // optionsBuilder.UseLazyLoadingProxies();
        optionsBuilder.EnableSensitiveDataLogging();
        base.OnConfiguring(optionsBuilder);
    }
}