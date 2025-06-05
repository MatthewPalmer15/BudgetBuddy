using BudgetBuddy.Application.Transactions.Models;
using BudgetBuddy.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Application.Transactions.Queries;

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
                              Description = t.Description,
                              Price = t.Price,
                              TransactionDate = t.TransactionDate,
                              Type = t.Type,
                              Category = t.Category,
                              Rank = t.Rank,
                              Essential = t.Essential,
                          }).FirstOrDefaultAsync(cancellationToken);
        }
    }
}