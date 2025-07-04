﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using BudgetBuddy.Database.Entities.System;
using BudgetBuddy.Database.Entities.Transactions;
using BudgetBuddy.Database.Extensions;
using BudgetBuddy.Infrastructure.Encryption;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Database;

public class ApplicationDbContext(IEncryptionService encryptionService) : DbContext, IDbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
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