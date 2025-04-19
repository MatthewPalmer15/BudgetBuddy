using BudgetBuddy.Application.Transactions.Commands;
using BudgetBuddy.Application.Transactions.Queries;
using BudgetBuddy.Database.Enums;
using BudgetBuddy.Infrastructure.Enums.Toast;
using BudgetBuddy.Infrastructure.Services.Toast;
using Microsoft.AspNetCore.Components;

namespace BlazorHybrid.App.Components.Pages.Transactions;

public partial class Edit : CustomComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IToastManager ToastManager { get; set; }

    [Parameter] public Guid? Id { get; set; }

    private TransactionModel _transactionModel = new();

    protected override async Task OnInitializedAsync()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        if (Id.HasValue && Id.Value != Guid.Empty)
        {
            var transaction = await Mediator.Send(new GetTransactionByIdQuery { Id = Id.Value }, cancellationToken);
            if (transaction != null)
            {
                _transactionModel = new TransactionModel
                {
                    Name = transaction.Name
                };
            }
        }
    }

    private async Task OnPostSaveTransaction()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var response = await Mediator.Send(new SaveTransactionCommand
        {
            Id = _transactionModel.Id,
            Name = _transactionModel.Name,
            Description = _transactionModel.Description,
            Price = _transactionModel.Price,
            Rank = _transactionModel.Rank,
            TransactionDate = _transactionModel.TransactionDate,
            Type = _transactionModel.Type,
            IsRecurring = _transactionModel.IsRecurring,
            VendorId = _transactionModel.ServiceProviderId
        }, cancellationToken);

        if (response.Success)
            NavigationManager.NavigateTo("/");

        ToastManager.Show(string.Join(",", response.Errors.Select(x => x.ErrorMessage).ToList()), ToastType.Error);
    }

    public class TransactionModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime? TransactionDate { get; set; }
        public bool IsRecurring { get; set; }
        public TransactionType Type { get; set; }
        public int Rank { get; set; }
        public Guid? ServiceProviderId { get; set; }
    }

}