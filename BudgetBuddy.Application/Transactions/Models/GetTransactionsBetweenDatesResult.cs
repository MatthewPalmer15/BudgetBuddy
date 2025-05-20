using BudgetBuddy.Database.Enums;

namespace BudgetBuddy.Application.Transactions.Models;

public class GetTransactionsBetweenDatesResult
{
    public decimal TotalIncome { get; set; }
    public decimal TotalOutcome { get; set; }
    public decimal TotalBalance { get; set; }
    public List<Transaction> Transactions { get; set; } = [];
    public class Transaction
    {
        public Guid Id { get; set; }
        public CategoryEnum Category { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime? TransactionDate { get; set; }
        public TransactionType Type { get; set; }
    }
}