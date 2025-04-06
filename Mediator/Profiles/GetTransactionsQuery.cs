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
        public string? CategoryName { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}