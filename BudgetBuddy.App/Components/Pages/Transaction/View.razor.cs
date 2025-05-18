using BudgetBuddy.Application.Transactions.Queries;
using BudgetBuddy.Database.Enums;
using Microsoft.AspNetCore.Components;

namespace BudgetBuddy.App.Components.Pages.Transaction;

public partial class View : CustomComponentBase
{
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Parameter]
    public Guid? transactionId { get; set; }

    private TransactionModel _transactionModel = new();

    protected override async Task OnInitializedAsync()
    {
        var cancellationToken = new CancellationTokenSource().Token;
        if (transactionId == null || transactionId == Guid.Empty)
            NavigationManager.NavigateTo("/");

        var transaction = await Mediator.Send(new GetTransactionByIdQuery { Id = transactionId.Value }, cancellationToken);
        if (transaction == null)
            NavigationManager.NavigateTo("/");

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
        public CategoryEnum Category { get; set; }
        public int Rank { get; set; }
        public Guid? VendorId { get; set; }
    }

}