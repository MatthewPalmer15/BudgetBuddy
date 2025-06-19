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
    private List<BarChartViewModel> BarChartData { get; set; } = [];

    public string TransactionHeaderTitle { get; set; }

    private decimal TotalIncome { get; set; }
    private decimal TotalOutcome { get; set; }
    private decimal TotalBalance { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await FetchData();
    }

    private async Task FetchData()
    {
        var cancellationToken = new CancellationTokenSource().Token;
        var transactions = await Mediator.Send(new GetGroupedTransactionsQuery(), cancellationToken);
        TotalIncome = transactions.TotalIncome;
        TotalOutcome = transactions.TotalOutcome;
        TotalBalance = transactions.TotalBalance;
        DailyData = transactions.Days.Where(x => x.Date.Month == DateTime.Now.Month).ToList();
        WeeklyData = transactions.Weeks.Where(x => x.Year == DateTime.Now.Year).ToList();
        MonthlyData = transactions.Months.Where(x => x.Year == DateTime.Now.Year).ToList();
        YearlyData = transactions.Years;
        SetPeriod(selectedPeriod);
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
                month.Balance = month.Transactions.Sum(y => y.Type == TransactionType.Income ? y.Price : -y.Price);
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

        SetPeriod(selectedPeriod);
    }

    private void SetPeriod(string period)
    {
        selectedPeriod = period;
        switch (selectedPeriod)
        {
            case "Daily":
                TotalIncome = DailyData.SelectMany(x => x.Transactions).Where(x => x.Type == TransactionType.Income).Sum(x => x.Price);
                TotalOutcome = DailyData.SelectMany(x => x.Transactions).Where(x => x.Type == TransactionType.Outcome).Sum(x => x.Price);
                TotalBalance = TotalIncome - TotalOutcome;
                break;

            case "Weekly":
                TotalIncome = WeeklyData.SelectMany(x => x.Transactions).Where(x => x.Type == TransactionType.Income).Sum(x => x.Price);
                TotalOutcome = WeeklyData.SelectMany(x => x.Transactions).Where(x => x.Type == TransactionType.Outcome).Sum(x => x.Price);
                TotalBalance = TotalIncome - TotalOutcome;
                break;

            case "Monthly":
                TotalIncome = MonthlyData.SelectMany(x => x.Transactions).Where(x => x.Type == TransactionType.Income).Sum(x => x.Price);
                TotalOutcome = MonthlyData.SelectMany(x => x.Transactions).Where(x => x.Type == TransactionType.Outcome).Sum(x => x.Price);
                TotalBalance = TotalIncome - TotalOutcome;
                break;

            case "Yearly":
                TotalIncome = YearlyData.SelectMany(x => x.Transactions).Where(x => x.Type == TransactionType.Income).Sum(x => x.Price);
                TotalOutcome = YearlyData.SelectMany(x => x.Transactions).Where(x => x.Type == TransactionType.Outcome).Sum(x => x.Price);
                TotalBalance = TotalIncome - TotalOutcome;
                break;
        }

        BarChartData = [];
        BarChartData.Add(new BarChartViewModel { Title = $"Outcome<br>{TotalOutcome:£#,0.00}", Count = TotalOutcome, Colour = "#f87171" });
        BarChartData.Add(new BarChartViewModel { Title = $"Income<br>{TotalIncome:£#,0.00}", Count = TotalIncome, Colour = "#4ade80" });
    }

    public class BarChartViewModel
    {
        public string Title { get; set; }
        public decimal Count { get; set; }
        public string Colour { get; set; }
    }
}