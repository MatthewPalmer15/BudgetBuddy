using BudgetBuddy.Application.Transactions.Models;
using BudgetBuddy.Database;
using BudgetBuddy.Database.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BudgetBuddy.Application.Transactions.Queries;

public class GetGroupedTransactionsQuery : IRequest<GetGroupedTransactionsResult>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    internal class Handler(IDbContext context) : IRequestHandler<GetGroupedTransactionsQuery, GetGroupedTransactionsResult>
    {
        public async Task<GetGroupedTransactionsResult> Handle(GetGroupedTransactionsQuery request, CancellationToken cancellationToken = default)
        {
            var transactions = await GetTransactionsAsync(request, cancellationToken);
            if (transactions.Count <= 0)
                return new GetGroupedTransactionsResult();

            var transactionsGroupedByDay = transactions.GroupBy(x => x.TransactionDate.HasValue ? x.TransactionDate.Value.Date : DateTime.Now.Date)
                .OrderByDescending(x => x.Key)
                .Select(x => new GetGroupedTransactionsResult.DailyData
                {
                    Date = x.Key,
                    Transactions = x.ToList(),
                    Amount = x.Sum(y => y.Type == TransactionType.Income ? y.Price : -y.Price)
                }).ToList();

            var culture = CultureInfo.CurrentCulture;
            var transactionsGroupedByWeek = transactions.GroupBy(x => culture.Calendar.GetWeekOfYear(x.TransactionDate ?? DateTime.Now.Date, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek))
                .OrderByDescending(x => x.Key)
                .Select(x => new GetGroupedTransactionsResult.WeeklyData
                {
                    WeekNumber = x.Key,
                    Transactions = x.ToList(),
                    Amount = x.Sum(y => y.Type == TransactionType.Income ? y.Price : -y.Price)
                }).ToList();

            var transactionsGroupedByMonth = transactions.GroupBy(x => new { (x.TransactionDate ?? DateTime.Now.Date).Month, (x.TransactionDate ?? DateTime.Now.Date).Year })
                .OrderByDescending(x => x.Key.Year)
                .ThenByDescending(x => x.Key.Month)
                .Select(x => new GetGroupedTransactionsResult.MonthlyData
                {
                    Month = x.Key.Month,
                    Year = x.Key.Year,
                    MonthName = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Key.Month)} {x.Key.Year}",
                    Transactions = x.ToList(),
                    Amount = x.Sum(y => y.Type == TransactionType.Income ? y.Price : -y.Price)
                }).ToList();


            var transactionsGroupedByYear = transactions.GroupBy(x => (x.TransactionDate ?? DateTime.Now.Date).Year)
                .Select(x => new GetGroupedTransactionsResult.YearlyData
                {
                    Year = x.Key,
                    Transactions = x.ToList(),
                    Amount = x.Sum(y => y.Type == TransactionType.Income ? y.Price : -y.Price)
                }).ToList();

            var totalIncome = transactions.Where(x => x.Type == TransactionType.Income).Sum(x => x.Price);
            var totalOutcome = transactions.Where(x => x.Type == TransactionType.Outcome).Sum(x => x.Price);
            var totalBalance = totalIncome - totalOutcome;

            return new GetGroupedTransactionsResult
            {
                TotalIncome = totalIncome,
                TotalOutcome = totalOutcome,
                TotalBalance = totalBalance,
                Days = transactionsGroupedByDay,
                Weeks = transactionsGroupedByWeek,
                Months = transactionsGroupedByMonth,
                Years = transactionsGroupedByYear
            };
        }

        private async Task<List<GetGroupedTransactionsResult.Transaction>> GetTransactionsAsync(GetGroupedTransactionsQuery request, CancellationToken cancellationToken = default)
        {
            return await (from t in context.Transactions
                          where !t.Deleted
                                && (request.StartDate == null || t.TransactionDate >= request.StartDate)
                                && (request.EndDate == null || t.TransactionDate <= request.EndDate)
                          select new GetGroupedTransactionsResult.Transaction
                          {
                              Id = t.Id,
                              Name = t.Name,
                              Price = t.Price,
                              TransactionDate = t.TransactionDate != null ? t.TransactionDate.Value : DateTime.Now.Date,
                              Type = t.Type,
                              Category = t.Category
                          }).ToListAsync(cancellationToken);
        }
    }
}