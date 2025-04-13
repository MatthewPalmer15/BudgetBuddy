using MediatR;

namespace BudgetBuddy.Mediator.Profiles;

public class GetTransactionsQuery : IRequest<GetTransactionsResult>
{
    internal class Handler : IRequestHandler<GetTransactionsQuery, GetTransactionsResult>
    {
        public async Task<GetTransactionsResult> Handle(GetTransactionsQuery request,
            CancellationToken cancellationToken = default)
        {
            return new GetTransactionsResult();
        }
    }
}

public class GetTransactionsResult
{
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