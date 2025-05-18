using BudgetBuddy.Application.Transactions.Models;
using BudgetBuddy.Database;
using BudgetBuddy.Database.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Application.Transactions.Queries;

public class GetTransactionsGroupedByDayQuery : IRequest<GetTransactionsGroupedByDayResult>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    internal class Handler(IDbContext context) : IRequestHandler<GetTransactionsGroupedByDayQuery, GetTransactionsGroupedByDayResult>
    {
        public async Task<GetTransactionsGroupedByDayResult> Handle(GetTransactionsGroupedByDayQuery request, CancellationToken cancellationToken = default)
        {
            var transactions = await GetTransactionsAsync(request, cancellationToken);
            if (transactions.Count <= 0)
                return new GetTransactionsGroupedByDayResult { Days = [], TotalIncome = 0, TotalOutcome = 0, TotalBalance = 0 };

            var groupedTransactions = transactions.GroupBy(x => x.TransactionDate ?? DateTime.Now.Date)
                .Select(x => new GetTransactionsGroupedByDayResult.DailyData
                {
                    Date = x.Key,
                    Transactions = x.ToList(),
                    Amount = x.Sum(y => y.Type == TransactionType.Income ? y.Price : -y.Price)
                }).ToList();


            var totalIncome = transactions.Where(x => x.Type == TransactionType.Income).Sum(x => x.Price);
            var totalOutcome = transactions.Where(x => x.Type == TransactionType.Outcome).Sum(x => x.Price);
            var totalBalance = totalIncome - totalOutcome;

            return new GetTransactionsGroupedByDayResult { Days = groupedTransactions, TotalIncome = totalIncome, TotalOutcome = totalOutcome, TotalBalance = totalBalance };
        }

        private async Task<List<GetTransactionsGroupedByDayResult.Transaction>> GetTransactionsAsync(GetTransactionsGroupedByDayQuery request, CancellationToken cancellationToken = default)
        {
            return await (from t in context.Transactions
                          where !t.Deleted
                                && (request.StartDate == null || t.TransactionDate >= request.StartDate)
                                && (request.EndDate == null || t.TransactionDate <= request.EndDate)
                          select new GetTransactionsGroupedByDayResult.Transaction
                          {
                              Id = t.Id,
                              Name = t.Name,
                              Price = t.Price,
                              TransactionDate = t.TransactionDate,
                              Type = t.Type,
                              Category = t.Category
                          }).ToListAsync(cancellationToken);
        }
    }
}