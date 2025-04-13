using BudgetBuddy.Components.Component;
using BudgetBuddy.Data.Entities;
using BudgetBuddy.Mediator.Transactions.Models;
using BudgetBuddy.Platforms;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Charts;

namespace BudgetBuddy.Components.Pages;

public partial class Home : ComponentBase
{
    private readonly string[] _colourPalette = ["#61EFCD", "#CDDE1F", "#FEC200", "#CA765A", "#2485FA", "#F57D7D", "#C152D2", "#8854D9", "#3D4EB8", "#00BCD7", "#4472c4", "#ed7d31", "#ffc000", "#70ad47", "#5b9bd5", "#c1c1c1", "#6f6fe2", "#e269ae", "#9e480e", "#997300"];
    private SfAccumulationChart _sfAccumulationChart;
    private Modal Modal { get; set; }

    [Inject] public IMediator Mediator { get; set; }
    [Inject] public IJSRuntime JsRuntime { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }

    private List<GetTransactionsResult.Transaction> Transactions { get; } = [];
    private List<ChartDataViewModel> ChartData { get; set; } = [];

    private decimal TotalIncome => Transactions.Where(x => x.Type == Transaction.TransactionType.Income).Sum(x => x.Price);
    private decimal TotalOutcome => Transactions.Where(x => x.Type == Transaction.TransactionType.Outcome).Sum(x => x.Price);

    protected override async Task OnInitializedAsync()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        //Transactions = (await Mediator.Send(new GetTransactionsQuery(), cancellationToken)).Transactions;
        NotificationService.SendNotification("test", "this is a test");

        Transactions.AddRange(new[]
        {
                new GetTransactionsResult.Transaction { Name = "Work Pay", Price = 2189.92m, CategoryName = "Earnings", Type = Transaction.TransactionType.Income },
                new GetTransactionsResult.Transaction { Name = "Rent", Price = 950.00m, CategoryName = "Expenses", Type = Transaction.TransactionType.Outcome },
                new GetTransactionsResult.Transaction { Name = "Phone Contract", Price = 40.23m, CategoryName = "Mobile", Type = Transaction.TransactionType.Outcome },
                new GetTransactionsResult.Transaction { Name = "Car Insurance", Price = 140.91m, CategoryName = "Car", Type = Transaction.TransactionType.Outcome },
                new GetTransactionsResult.Transaction { Name = "Car Tax", Price = 15.55m, CategoryName = "Car", Type = Transaction.TransactionType.Outcome },
                new GetTransactionsResult.Transaction { Name = "Car Fuel", Price = 70.00m, CategoryName = "Car", Type = Transaction.TransactionType.Pending }
            });

        ChartData = Transactions
            .Where(x => x.Type == Transaction.TransactionType.Outcome)
            .GroupBy(t => t.CategoryName ?? "Uncategorized")
            .Select(g => new ChartDataViewModel
            {
                Title = g.Key,
                Price = g.Sum(t => t.Price),
                Percentage = Math.Round((g.Sum(t => t.Price) / TotalIncome) * 100, 2),
                Tooltip = $"{g.Key} - {Math.Round((g.Sum(t => t.Price) / TotalIncome) * 100, 2)}%"
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
        if (Modal.Show)
        {
            Modal.Close();
        }
    }

    public class ChartDataViewModel
    {
        public string Tooltip { get; set; }
        public decimal Percentage { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public Transaction.TransactionType Type { get; set; }
    }
}