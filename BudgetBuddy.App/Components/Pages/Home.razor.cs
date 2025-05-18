using BudgetBuddy.Application.Transactions.Commands;
using BudgetBuddy.Application.Transactions.Models;
using BudgetBuddy.Application.Transactions.Queries;
using BudgetBuddy.Database.Enums;
using BudgetBuddy.Infrastructure.Enums.Toast;
using BudgetBuddy.Infrastructure.Services.Toast;
using Microsoft.AspNetCore.Components;

namespace BudgetBuddy.App.Components.Pages;

public partial class Home : CustomComponentBase
{
    private readonly string[] _colourPalette =
    [
        "#365A9B", // Blue
        "#C86464", // Red
        "#5C8C3B", // Green
        "#BB6428", // Orange
        "#9B9B9B", // Grey
        "#A9B81A", // Olive
        "#0098AD", // Teal
        "#B9548D", // Rose
        "#9E42AC", // Purple
        "#C19D00", // Goldenrod
        "#7D5E00", // Dark Mustard
        "#6E44B3", // Deep Purple
        "#313E99", // Indigo
        "#A35945", // Brown
        "#4DC7A7", // Mint
        "#7F3A0B", // Rust
        "#5858BC", // Cool Blue
        "#CC9E00", // Warm Yellow
        "#487DB0", // Steel Blue
        "#3D4EB8" // Cobalt
    ];

    private string selectedPeriod = "Daily";

    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IToastManager ToastManager { get; set; }


    private List<GetTransactionsGroupedByDayResult.DailyData> DailyData { get; set; } = [];
    private List<ChartDataViewModel> ChartData { get; set; } = [];
    private List<BarChartViewModel> BarChartData { get; set; } = [];

    private decimal TotalIncome { get; set; }
    private decimal TotalOutcome { get; set; }
    private decimal TotalBalance { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await FetchData();
    }

    public async Task FetchData()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var transactions = await Mediator.Send(new GetTransactionsGroupedByDayQuery { StartDate = startOfMonth, EndDate = startOfMonth.AddMonths(1) }, cancellationToken);

        TotalIncome = transactions.TotalIncome;
        TotalOutcome = transactions.TotalOutcome;
        TotalBalance = transactions.TotalBalance;
        DailyData = transactions.Days;



        // Transactions = (await Mediator.Send(new GetTransactionsQuery(), cancellationToken)).Transactions;
        // 
        // if (Transactions.Any(x => x.Type == TransactionType.Income))
        // {
        //     ChartData = Transactions
        //         .Where(x => x.Type == TransactionType.Outcome)
        //         .OrderBy(x => x.Category)
        //         .GroupBy(t => t.Category)
        //         .Select(g => new ChartDataViewModel
        //         {
        //             Title = g.Key.ToString(),
        //             Price = g.Sum(t => t.Price),
        //             Percentage = Math.Round(g.Sum(t => t.Price) / TotalIncome * 100, 2),
        //             Tooltip = $"{g.Key} - {Math.Round(g.Sum(t => t.Price) / TotalIncome * 100, 2)}%"
        //         })
        //         .ToList();
        // 
        //     var leftOverPercentage = ChartData.Sum(x => x.Percentage);
        //     ChartData.Add(new ChartDataViewModel
        //     {
        //         Title = "Left Over",
        //         Price = TotalIncome - TotalOutcome,
        //         Percentage = 100 - leftOverPercentage,
        //         Tooltip = $"Left Over - {100 - leftOverPercentage}%"
        //     });
        // }

        BarChartData = new List<BarChartViewModel>();
        BarChartData.Add(new BarChartViewModel
        { Title = $"Outcome<br>{TotalOutcome.ToString("£#,0.00")}", Count = TotalOutcome, Colour = "#f87171" });
        BarChartData.Add(new BarChartViewModel
        { Title = $"Income<br>{TotalIncome.ToString("£#,0.00")}", Count = TotalIncome, Colour = "#4ade80" });
    }

    private async Task Delete(Guid id)
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var response = await Mediator.Send(new DeleteTransactionCommand { Id = id }, cancellationToken);

        if (response.Success)
        {
            ToastManager.Show("Deleted Successfully", ToastType.Success);
            await FetchData();
        }
        else
        {
            ToastManager.Show(string.Join(",", response.Errors.Select(x => x.ErrorMessage).ToList()), ToastType.Error);
        }
    }
    public class ChartDataViewModel
    {
        public string Tooltip { get; set; }
        public decimal Percentage { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public TransactionType Type { get; set; }
    }

    public class BarChartViewModel
    {
        public string Title { get; set; }
        public decimal Count { get; set; }
        public string Colour { get; set; }
    }
}