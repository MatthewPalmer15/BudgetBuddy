﻿using BudgetBuddy.Database.Entities.System;
using BudgetBuddy.Database.Entities.Transactions;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Database;

public interface IDbContext
{
    DbSet<Transaction> Transactions { get; set; }
    DbSet<Setting> Settings { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}