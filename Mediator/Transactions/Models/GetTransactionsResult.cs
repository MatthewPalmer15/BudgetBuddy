namespace BudgetBuddy.Mediator.Transactions.Models;

public class GetTransactionsResult
{
    private decimal TotalIncome => Transactions.Where(x => x.Type == Data.Entities.Transaction.TransactionType.Income).Sum(x => x.Price);
    private decimal TotalOutcome => Transactions.Where(x => x.Type == Data.Entities.Transaction.TransactionType.Outcome).Sum(x => x.Price);
    private decimal TotalPending => Transactions.Where(x => x.Type == Data.Entities.Transaction.TransactionType.Pending).Sum(x => x.Price);
    private decimal TotalLeft => TotalIncome - TotalOutcome - TotalPending;

    public List<Transaction> Transactions { get; set; } = [];

    public class Transaction
    {
        public Guid Id { get; set; }
        public string? CategoryName { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsRecurring { get; set; }

        public Data.Entities.Transaction.TransactionType Type { get; set; }
    }
}