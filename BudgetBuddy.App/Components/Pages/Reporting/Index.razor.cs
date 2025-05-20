using BudgetBuddy.Application.Transactions.Queries;
using BudgetBuddy.Database.Enums;

namespace BudgetBuddy.App.Components.Pages.Reporting;

public partial class Index : CustomComponentBase
{
    private IEnumerable<TotalViewModel> Totals { get; set; } = [];
    public decimal TotalIncome { get; set; }
    public decimal TotalOutcome { get; set; }
    public decimal TotalBalance { get; set; }
    private List<ChartDataViewModel> ChartData { get; set; } = [];


    protected override async Task OnInitializedAsync()
    {
        var cancellationToken = new CancellationTokenSource().Token;
        var result = await Mediator.Send(new GetTransactionsBetweenDatesQuery { StartDate = DateTime.Now.Date.AddDays(-30), EndDate = DateTime.Now.Date }, cancellationToken);

        Totals = result.Transactions.GroupBy(x => new { x.Type, x.Category }).Select(x => new TotalViewModel
        {
            Type = x.Key.Type,
            Category = x.Key.Category,
            Total = x.Sum(y => y.Price)
        }).ToList();

        TotalIncome = result.TotalIncome;
        TotalOutcome = result.TotalOutcome;
        TotalBalance = result.TotalBalance;

        if (TotalOutcome > 0)
        {
            ChartData = result.Transactions
                .Where(x => x.Type == TransactionType.Outcome)
                .OrderBy(x => x.Category)
                .GroupBy(t => t.Category)
                .Select(g => new ChartDataViewModel
                {
                    Title = g.Key.ToString(),
                    Price = g.Sum(t => t.Price),
                    Percentage = Math.Round(g.Sum(t => t.Price) / TotalOutcome * 100, 2),
                    Tooltip = $"{g.Key} - {g.Sum(t => t.Price):£#,0.00}"
                })
                .ToList();
        }
    }

    private class TotalViewModel
    {
        public TransactionType Type { get; set; }
        public CategoryEnum Category { get; set; }
        public decimal Total { get; set; }
    }

    public class ChartDataViewModel
    {
        public string Tooltip { get; set; }
        public decimal Percentage { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public TransactionType Type { get; set; }
    }
}