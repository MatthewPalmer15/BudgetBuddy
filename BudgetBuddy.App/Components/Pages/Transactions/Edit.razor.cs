using BudgetBuddy.Application.Transactions.Commands;
using BudgetBuddy.Application.Transactions.Queries;
using BudgetBuddy.Database.Enums;
using BudgetBuddy.Infrastructure.Enums.Toast;
using BudgetBuddy.Infrastructure.Services.Toast;
using Microsoft.AspNetCore.Components;

namespace BudgetBuddy.App.Components.Pages.Transactions;

public partial class Edit : CustomComponentBase
{
    [Inject]
    public IToastManager ToastManager { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }


    [Parameter]
    public Guid? transactionId { get; set; }

    private TransactionModel _transactionModel = new();
    private string _title { get; set; } = "Create Transaction";

    protected override async Task OnInitializedAsync()
    {
        var cancellationToken = new CancellationTokenSource().Token;
        if (transactionId.HasValue && transactionId.Value != Guid.Empty)
        {
            var transaction = await Mediator.Send(new GetTransactionByIdQuery { Id = transactionId.Value }, cancellationToken);
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
                    Essential = transaction.Essential,
                    AccountId = transaction.AccountId
                };
                _title = $"Edit Transaction '{transaction.Name}'";
            }
        }
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
            Essential = _transactionModel.Essential,
            AccountId = _transactionModel.AccountId
        }, cancellationToken);

        if (response.Success)
        {
            ToastManager.Show("Save successfully", ToastType.Success);
            NavigationManager.NavigateTo("/");
        }
        else
        {
            ToastManager.Show(string.Join(",", response.Errors.Select(x => x.ErrorMessage).ToList()), ToastType.Error);
        }
    }


    public class TransactionModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime? TransactionDate { get; set; }
        public TransactionType Type { get; set; }
        public CategoryEnum Category { get; set; }
        public int Rank { get; set; }
        public bool Essential { get; set; }
        public Guid? AccountId { get; set; }
    }

}