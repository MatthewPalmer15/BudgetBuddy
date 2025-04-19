using BudgetBuddy.Database;
using BudgetBuddy.Mediator.Transactions.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Mediator.Transactions.Queries;

public class GetTransactionByIdQuery : IRequest<GetTransactionByIdResult?>
{
    public required Guid Id { get; set; }

    internal class Handler(IDbContext context) : IRequestHandler<GetTransactionByIdQuery, GetTransactionByIdResult?>
    {
        public async Task<GetTransactionByIdResult?> Handle(GetTransactionByIdQuery request,
            CancellationToken cancellationToken = default)
        {
            return await (from t in context.Transactions
                          where !t.Deleted && request.Id == t.Id
                          select new GetTransactionByIdResult
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
                          }).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
