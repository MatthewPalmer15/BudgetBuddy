using BudgetBuddy.Database.Enums;

namespace BudgetBuddy.Application.Transactions.Models;

public class GetGroupedTransactionsResult
{
    public decimal TotalIncome { get; set; }
    public decimal TotalOutcome { get; set; }
    public decimal TotalBalance { get; set; }

    public List<DailyData> Days { get; set; } = [];
    public List<WeeklyData> Weeks { get; set; } = [];
    public List<MonthlyData> Months { get; set; } = [];
    public List<YearlyData> Years { get; set; } = [];

    public class DailyData
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public List<Transaction> Transactions { get; set; } = [];
    }


    public class WeeklyData
    {
        public int WeekNumber { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public decimal Amount { get; set; }
        public List<Transaction> Transactions { get; set; } = [];
    }

    public class MonthlyData
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string MonthName { get; set; }
        public decimal Amount { get; set; }
        public List<Transaction> Transactions { get; set; } = [];
    }

    public class YearlyData
    {
        public int Year { get; set; }
        public decimal Amount { get; set; }
        public List<Transaction> Transactions { get; set; } = [];
    }

    public class Transaction
    {
        public Guid Id { get; set; }
        public CategoryEnum Category { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionType Type { get; set; }
    }
}