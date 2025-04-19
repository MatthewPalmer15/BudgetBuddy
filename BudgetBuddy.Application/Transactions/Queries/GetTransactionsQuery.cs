using BudgetBuddy.Application.Transactions.Models;
using BudgetBuddy.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Application.Transactions.Queries;

public class GetTransactionsQuery : IRequest<GetTransactionsResult>
{
    internal class Handler(IDbContext context) : IRequestHandler<GetTransactionsQuery, GetTransactionsResult>
    {
        public async Task<GetTransactionsResult> Handle(GetTransactionsQuery request,
            CancellationToken cancellationToken = default)
        {
            var transactions = await (from t in context.Transactions
                                      where !t.Deleted
                                      select new GetTransactionsResult.Transaction
                                      {
                                          Id = t.Id,
                                          Name = t.Name,
                                          Price = t.Price,
                                          TransactionDate = t.TransactionDate,
                                          Type = t.Type,
                                          Category = t.Category
                                      }).ToListAsync(cancellationToken);

            return new GetTransactionsResult { Transactions = transactions };
        }
    }
}