namespace BudgetBuddy.Mediator.Transactions.Models;

public class GetTransactionByIdResult
{
    public Guid Id { get; set; }
    public string? CategoryName { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsRecurring { get; set; }

    public Database.Entities.Transactions.Transaction.TransactionType Type { get; set; }
}