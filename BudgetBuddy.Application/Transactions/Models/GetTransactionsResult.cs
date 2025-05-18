using BudgetBuddy.Database.Enums;

namespace BudgetBuddy.Application.Transactions.Models;

public class GetTransactionsResult
{
    private decimal TotalIncome => Transactions.Where(x => x.Type == TransactionType.Income).Sum(x => x.Price);
    private decimal TotalOutcome => Transactions.Where(x => x.Type == TransactionType.Outcome).Sum(x => x.Price);
    private decimal TotalLeft => TotalIncome - TotalOutcome;

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