using BudgetBuddy.Database;
using BudgetBuddy.Mediator.Transactions.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Mediator.Transactions.Queries;

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
                                          StartDate = t.StartDate,
                                          EndDate = t.EndDate,
                                          Type = t.Type,
                                          IsRecurring = t.IsRecurring,
                                          CategoryName = (from sp in context.ServiceProviders
                                                          join c in context.Categories on sp.CategoryId.Value equals c.Id
                                                          where !c.Deleted && !sp.Deleted && sp.Id == t.ServiceProviderId
                                                          select c.Title).FirstOrDefault()
                                      }).ToListAsync(cancellationToken);

            return new GetTransactionsResult { Transactions = transactions };
        }
    }
}
