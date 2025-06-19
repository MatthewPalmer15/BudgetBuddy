using BudgetBuddy.Application.Transactions.Models;
using BudgetBuddy.Application.Transactions.Queries;
using BudgetBuddy.Database.Enums;

namespace BudgetBuddy.App.Components.Pages;

public partial class Weather : CustomComponentBase
{
    public List<ChartDataViewModel> IncomeChartData { get; set; } = [];
    public decimal AverageIncome { get; set; }
    public List<ChartDataViewModel> OutcomeChartData { get; set; } = [];
    public decimal AverageOutcome { get; set; }
    public List<ChartDataViewModel> BalanceChartData { get; set; } = [];
    public decimal AverageBalance { get; set; }
    public decimal PercentageLeft { get; set; }
    public List<string> Suggestions { get; set; } = [];


    protected override async Task OnInitializedAsync()
    {
        var cancellationToken = new CancellationTokenSource().Token;
        var transactions = await Mediator.Send(new GetGroupedTransactionsQuery(), cancellationToken);

        IncomeChartData = transactions.Months.Select(x => new ChartDataViewModel
        {
            Date = new DateTime(x.Year, x.Month, 1),
            Value = x.Income
        }).ToList();
        AverageIncome = IncomeChartData.Average(x => x.Value);

        OutcomeChartData = transactions.Months.Select(x => new ChartDataViewModel
        {
            Date = new DateTime(x.Year, x.Month, 1),
            Value = x.Outcome
        }).ToList();
        AverageOutcome = OutcomeChartData.Average(x => x.Value);

        BalanceChartData = transactions.Months.Select(x => new ChartDataViewModel
        {
            Date = new DateTime(x.Year, x.Month, 1),
            Value = x.Balance
        }).ToList();
        AverageBalance = BalanceChartData.Average(x => x.Value);

        PercentageLeft = AverageIncome > 0 ? ((AverageIncome - AverageOutcome) / AverageIncome) : 0;

        var focusMonth = transactions.Months
            .Where(x => x.Month <= DateTime.Now.Month && x.Year <= DateTime.Now.Year && x.Transactions.Count > 0)
            .OrderByDescending(x => x.Year)
            .ThenByDescending(x => x.Month)
            .FirstOrDefault();

        if (focusMonth != null)
            Suggestions = GetSuggestions(focusMonth, PercentageLeft);
    }

    private List<string> GetSuggestions(GetGroupedTransactionsResult.MonthlyData monthlyData, decimal percentageLeft)
    {
        var suggestions = new List<string>();
        if (percentageLeft > 50)
        {
            suggestions.Add("No suggestions. Good work!");
            return suggestions;
        }

        var outcomeTransactions = monthlyData.Transactions.Where(x => x.Type == TransactionType.Outcome).ToList();

        var nonEssentialTotal = outcomeTransactions.Where(x => !x.Essential).Sum(x => x.Price);
        if (nonEssentialTotal > 0)
            suggestions.Add($"You are currently spending {nonEssentialTotal:£#,00.00} on non essential transactions. Consider reviewing and save some money.");

        suggestions.Add("Consider setting a weekly budget and tracking your daily expenses.");
        suggestions.Add("Use cashback or discount apps for purchases.");
        return suggestions;
    }

    public class ChartDataViewModel
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }

}