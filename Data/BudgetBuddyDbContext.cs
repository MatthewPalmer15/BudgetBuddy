// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using BudgetBuddy.Services.Encryption;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Data;

/// <summary>
///     Athena DB Context without ASP.NET Identity
/// </summary>
public class BudgetBuddyDbContext(IEncryptionService encryptionService) : DbContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");
        builder.HasDefaultSchema("dbo");
        // builder.ApplyConfigurations();
        // 
        // if (_encryptionService != null)
        //     builder.UseEncryption(_encryptionService.EncryptionProvider);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("");
        // optionsBuilder.UseLazyLoadingProxies();
        optionsBuilder.EnableSensitiveDataLogging();
        base.OnConfiguring(optionsBuilder);
    }

}