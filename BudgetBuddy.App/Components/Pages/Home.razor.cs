using BlazorHybrid.App.Components.Components;
using BudgetBuddy.Application.Transactions.Commands;
using BudgetBuddy.Application.Transactions.Models;
using BudgetBuddy.Application.Transactions.Queries;
using BudgetBuddy.Database.Enums;
using BudgetBuddy.Infrastructure.Enums.Toast;
using BudgetBuddy.Infrastructure.Services.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Charts;

namespace BlazorHybrid.App.Components.Pages;

public partial class Home : CustomComponentBase
{
    [Inject] public IJSRuntime JsRuntime { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IToastManager ToastManager { get; set; }

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
        "#3D4EB8"  // Cobalt
    ];


    private SfAccumulationChart _sfAccumulationChart;

    private Modal _transactionModal;

    private TransactionModel _transactionModel = new();


    public class TransactionModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime? TransactionDate { get; set; }
        public bool IsRecurring { get; set; }
        public TransactionType Type { get; set; }
        public CategoryEnum Category { get; set; }
        public int Rank { get; set; }
        public Guid? VendorId { get; set; }
    }


    private List<GetTransactionsResult.Transaction> Transactions { get; set; } = [];
    private List<ChartDataViewModel> ChartData { get; set; } = [];

    private decimal TotalIncome => Transactions.Where(x => x.Type == TransactionType.Income).Sum(x => x.Price);
    private decimal TotalOutcome => Transactions.Where(x => x.Type == TransactionType.Outcome).Sum(x => x.Price);

    protected override async Task OnInitializedAsync()
    {
        await FetchData();
    }

    public async Task FetchData()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        Transactions = (await Mediator.Send(new GetTransactionsQuery(), cancellationToken)).Transactions;

        ChartData = Transactions
            .Where(x => x.Type == TransactionType.Outcome)
            .OrderBy(x => x.Category)
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

    public async Task OpenTransactionModal(Guid? id = null)
    {
        var cancellationToken = new CancellationTokenSource().Token;
        if (id.HasValue && id.Value != Guid.Empty)
        {
            var transaction = await Mediator.Send(new GetTransactionByIdQuery { Id = id.Value }, cancellationToken);
            if (transaction != null)
            {
                _transactionModel = new TransactionModel
                {
                    Id = transaction.Id,
                    Name = transaction.Name,
                    Description = transaction.Description,
                    Price = transaction.Price,
                    TransactionDate = transaction.TransactionDate,
                    Category = transaction.Category,
                    Type = transaction.Type,
                    Rank = transaction.Rank,
                    VendorId = transaction.VendorId
                };
                _transactionModal.Title = $"Edit Transaction '{transaction.Name}'";
            }
            else
            {
                _transactionModel = new TransactionModel();
                _transactionModal.Title = "Create Transaction";
            }
        }
        else
        {
            _transactionModal.Title = "Create Transaction";
            _transactionModel = new TransactionModel();
        }


        _transactionModal.Open();
    }

    private async Task SaveTransaction()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var response = await Mediator.Send(new SaveTransactionCommand
        {
            Id = _transactionModel.Id,
            Name = _transactionModel.Name,
            Description = _transactionModel.Description,
            Price = _transactionModel.Price,
            Category = _transactionModel.Category,
            Rank = _transactionModel.Rank,
            TransactionDate = _transactionModel.TransactionDate,
            Type = _transactionModel.Type,
            IsRecurring = _transactionModel.IsRecurring,
            VendorId = _transactionModel.VendorId
        }, cancellationToken);

        if (response.Success)
        {
            ToastManager.Show("Save successfully", ToastType.Success);

            _transactionModel = new TransactionModel();
            _transactionModal?.Close();
            await FetchData();
        }
        else
        {
            ToastManager.Show(string.Join(",", response.Errors.Select(x => x.ErrorMessage).ToList()), ToastType.Error);
        }
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
}