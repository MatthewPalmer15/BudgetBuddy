using BudgetBuddy.Application.Transactions.Models;
using BudgetBuddy.Application.Transactions.Queries;
using BudgetBuddy.Database.Enums;
using BudgetBuddy.Infrastructure.Services.Toast;
using Microsoft.AspNetCore.Components;

namespace BudgetBuddy.App.Components.Pages;

public partial class Home : CustomComponentBase
{
    private string selectedPeriod = "Daily";

    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IToastManager ToastManager { get; set; }


    private List<GetGroupedTransactionsResult.DailyData> DailyData { get; set; } = [];
    private List<GetGroupedTransactionsResult.WeeklyData> WeeklyData { get; set; } = [];
    private List<GetGroupedTransactionsResult.MonthlyData> MonthlyData { get; set; } = [];
    private List<GetGroupedTransactionsResult.YearlyData> YearlyData { get; set; } = [];
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
        var transactions = await Mediator.Send(new GetGroupedTransactionsQuery(), cancellationToken);

        TotalIncome = transactions.TotalIncome;
        TotalOutcome = transactions.TotalOutcome;
        TotalBalance = transactions.TotalBalance;
        DailyData = transactions.Days.Where(x => x.Date.Month == DateTime.Now.Month).ToList();
        WeeklyData = transactions.Weeks;
        MonthlyData = transactions.Months.Where(x => x.Year == DateTime.Now.Year).ToList();
        YearlyData = transactions.Years;

        // ChartData = transactions.Days.SelectMany(x => x.Transactions)
        //     .Where(x => x.Type == TransactionType.Outcome)
        //     .OrderBy(x => x.Category)
        //     .GroupBy(t => t.Category)
        //     .Select(g => new ChartDataViewModel
        //     {
        //         Title = g.Key.ToString(),
        //         Price = g.Sum(t => t.Price),
        //         Percentage = Math.Round(g.Sum(t => t.Price) / TotalIncome * 100, 2),
        //         Tooltip = $"{g.Key} - {Math.Round(g.Sum(t => t.Price) / TotalIncome * 100, 2)}%"
        //     })
        //     .ToList();
        // 
        // var leftOverPercentage = ChartData.Sum(x => x.Percentage);
        // ChartData.Add(new ChartDataViewModel
        // {
        //     Title = "Left Over",
        //     Price = TotalIncome - TotalOutcome,
        //     Percentage = 100 - leftOverPercentage,
        //     Tooltip = $"Left Over - {100 - leftOverPercentage}%"
        // });

        BarChartData = new List<BarChartViewModel>();
        BarChartData.Add(new BarChartViewModel
        { Title = $"Outcome<br>{TotalOutcome.ToString("£#,0.00")}", Count = TotalOutcome, Colour = "#f87171" });
        BarChartData.Add(new BarChartViewModel
        { Title = $"Income<br>{TotalIncome.ToString("£#,0.00")}", Count = TotalIncome, Colour = "#4ade80" });
    }

    private void OnTransactionDeleted(Guid id)
    {
        foreach (var day in DailyData)
        {
            if (day.Transactions.Any(x => x.Id == id))
            {
                day.Transactions.RemoveAll(x => x.Id == id);
                day.Amount = day.Transactions.Sum(y => y.Type == TransactionType.Income ? y.Price : -y.Price);
            }
        }

        foreach (var week in WeeklyData)
        {
            if (week.Transactions.Any(x => x.Id == id))
            {
                week.Transactions.RemoveAll(x => x.Id == id);
                week.Amount = week.Transactions.Sum(y => y.Type == TransactionType.Income ? y.Price : -y.Price);
            }
        }

        foreach (var month in MonthlyData)
        {
            if (month.Transactions.Any(x => x.Id == id))
            {
                month.Transactions.RemoveAll(x => x.Id == id);
                month.Amount = month.Transactions.Sum(y => y.Type == TransactionType.Income ? y.Price : -y.Price);
            }
        }

        foreach (var year in YearlyData)
        {
            if (year.Transactions.Any(x => x.Id == id))
            {
                year.Transactions.RemoveAll(x => x.Id == id);
                year.Amount = year.Transactions.Sum(y => y.Type == TransactionType.Income ? y.Price : -y.Price);
            }
        }

        TotalIncome = DailyData.SelectMany(x => x.Transactions).Where(x => x.Type == TransactionType.Income).Sum(x => x.Price);
        TotalOutcome = DailyData.SelectMany(x => x.Transactions).Where(x => x.Type == TransactionType.Outcome).Sum(x => x.Price);
        TotalBalance = TotalIncome - TotalOutcome;

        BarChartData = new List<BarChartViewModel>();
        BarChartData.Add(new BarChartViewModel
        { Title = $"Outcome<br>{TotalOutcome.ToString("£#,0.00")}", Count = TotalOutcome, Colour = "#f87171" });
        BarChartData.Add(new BarChartViewModel
        { Title = $"Income<br>{TotalIncome.ToString("£#,0.00")}", Count = TotalIncome, Colour = "#4ade80" });
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