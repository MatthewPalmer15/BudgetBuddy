using BudgetBuddy.Application.Account.Models;
using BudgetBuddy.Database;
using BudgetBuddy.Infrastructure.Encryption;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.Sqlite;

namespace BudgetBuddy.App.Components.Pages.Testing;

public partial class Index(ApplicationDbContext context) : CustomComponentBase
{
    [Inject] public NavigationManager Navigation { get; set; }

    private GetUserResult User { get; set; }

    protected override async Task OnInitializedAsync()
    {
    }

    private async Task OnDeleteDatabase()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        SqliteConnection.ClearAllPools();


        var dbPath = Path.Combine(AppContext.BaseDirectory, "app.db");
        if (File.Exists(dbPath))
            File.Delete(dbPath);

        await using (var context = new ApplicationDbContext(new EncryptionService()))
        {
            await context.Database.EnsureCreatedAsync();
        }
    }
}