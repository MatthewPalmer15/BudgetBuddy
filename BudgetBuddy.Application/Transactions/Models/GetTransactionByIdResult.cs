using BudgetBuddy.Database.Enums;

namespace BudgetBuddy.Application.Transactions.Models;

public class GetTransactionByIdResult
{
    public Guid Id { get; set; }
    public CategoryEnum Category { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public DateTime? TransactionDate { get; set; }
    public TransactionType Type { get; set; }
}