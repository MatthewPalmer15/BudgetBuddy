using BudgetBuddy.Application.Transactions.Models;
using BudgetBuddy.Database;
using BudgetBuddy.Database.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Application.Transactions.Queries;

public class GetTransactionsBetweenDatesQuery : IRequest<GetTransactionsBetweenDatesResult>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    internal class Handler(IDbContext context) : IRequestHandler<GetTransactionsBetweenDatesQuery, GetTransactionsBetweenDatesResult>
    {
        public async Task<GetTransactionsBetweenDatesResult> Handle(GetTransactionsBetweenDatesQuery request,
            CancellationToken cancellationToken = default)
        {
            var transactions = await (from t in context.Transactions
                                      where !t.Deleted
                                            && (request.StartDate == null || t.TransactionDate >= request.StartDate)
                                            && (request.EndDate == null || t.TransactionDate <= request.EndDate)
                                      select new GetTransactionsBetweenDatesResult.Transaction
                                      {
                                          Id = t.Id,
                                          Name = t.Name,
                                          Price = t.Price,
                                          TransactionDate = t.TransactionDate,
                                          Type = t.Type,
                                          Category = t.Category
                                      }).ToListAsync(cancellationToken);

            var totalIncome = transactions.Where(x => x.Type == TransactionType.Income).Sum(x => x.Price);
            var totalOutcome = transactions.Where(x => x.Type == TransactionType.Outcome).Sum(x => x.Price);

            return new GetTransactionsBetweenDatesResult
            {
                Transactions = transactions,
                TotalIncome = totalIncome,
                TotalOutcome = totalOutcome,
                TotalBalance = totalIncome - totalOutcome
            };
        }
    }
}