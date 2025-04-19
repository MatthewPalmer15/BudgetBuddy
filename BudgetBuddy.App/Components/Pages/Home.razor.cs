using BudgetBuddy.Application.Transactions.Models;
using BudgetBuddy.Database.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Charts;

namespace BlazorHybrid.App.Components.Pages;

public partial class Home : CustomComponentBase
{
    private readonly string[] _colourPalette =
    [
        "#61EFCD", "#CDDE1F", "#FEC200", "#CA765A", "#2485FA", "#F57D7D", "#C152D2", "#8854D9", "#3D4EB8", "#00BCD7",
        "#4472c4", "#ed7d31", "#ffc000", "#70ad47", "#5b9bd5", "#c1c1c1", "#6f6fe2", "#e269ae", "#9e480e", "#997300"
    ];

    private SfAccumulationChart _sfAccumulationChart;

    [Inject] public IJSRuntime JsRuntime { get; set; }

    private List<GetTransactionsResult.Transaction> Transactions { get; } = [];
    private List<ChartDataViewModel> ChartData { get; set; } = [];

    private decimal TotalIncome => Transactions.Where(x => x.Type == TransactionType.Income).Sum(x => x.Price);
    private decimal TotalOutcome => Transactions.Where(x => x.Type == TransactionType.Outcome).Sum(x => x.Price);

    protected override async Task OnInitializedAsync()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        //Transactions = (await Mediator.Send(new GetTransactionsQuery(), cancellationToken)).Transactions;

        Transactions.AddRange(new[]
        {
            new GetTransactionsResult.Transaction
                { Name = "Work Pay", Price = 1636.53m, Category = CategoryEnum.None, Type = TransactionType.Income },
            new GetTransactionsResult.Transaction
                { Name = "Rent", Price = 950.00m, Category = CategoryEnum.Bills, Type = TransactionType.Outcome },
            new GetTransactionsResult.Transaction
                { Name = "Phone Contract", Price = 40.23m, Category = CategoryEnum.Bills, Type = TransactionType.Outcome },
            new GetTransactionsResult.Transaction
                { Name = "Car Insurance", Price = 140.91m, Category = CategoryEnum.Bills, Type = TransactionType.Outcome },
            new GetTransactionsResult.Transaction
                { Name = "Car Tax", Price = 15.55m, Category = CategoryEnum.Bills, Type = TransactionType.Outcome },
            new GetTransactionsResult.Transaction
                { Name = "Car Fuel", Price = 70.00m, Category = CategoryEnum.Travel, Type = TransactionType.Pending },
            new GetTransactionsResult.Transaction
            {
                Name = "Entertainment", Price = 156.93m, Category = CategoryEnum.Entertainment, Type = TransactionType.Outcome
            }
        });

        ChartData = Transactions
            .Where(x => x.Type == TransactionType.Outcome)
            .GroupBy(t => t.Category)
            .Select(g => new ChartDataViewModel
            {
                Title = g.Key.ToString(),
                Price = g.Sum(t => t.Price),
                Percentage = Math.Round(g.Sum(t => t.Price) / TotalIncome * 100, 2),
                Tooltip = $"{g.Key} - {Math.Round(g.Sum(t => t.Price) / TotalIncome * 100, 2)}%"
            })
            .ToList();

        var leftOverPercentage = ChartData.Sum(x => x.Percentage);
        ChartData.Add(new ChartDataViewModel
        {
            Title = "Left Over",
            Price = TotalIncome - TotalOutcome,
            Percentage = 100 - leftOverPercentage,
            Tooltip = $"Left Over - {100 - leftOverPercentage}%"
        });
    }

    private async Task Delete(string id)
    {
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