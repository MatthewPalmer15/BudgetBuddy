using BudgetBuddy.Application.Transactions.Queries;

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
    public string PercentageColour { get; set; }


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
        PercentageColour = PercentageLeft >= 0.60m
            ? "green"
            : PercentageLeft > 0.40m
                ? "yellow"
                : "red";


    }

    public class ChartDataViewModel
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }

}