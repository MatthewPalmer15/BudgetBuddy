using BudgetBuddy.Data;
using MediatR;

namespace BudgetBuddy.Mediator.Profiles;

public class GetTransactionPieChartDataQuery : IRequest<GetTransactionPieChartDataResult>
{
    internal class Handler(BudgetBuddyDbContext dbContext, IMediator mediator) : IRequestHandler<GetTransactionPieChartDataQuery, GetTransactionPieChartDataResult>
    {
        public async Task<GetTransactionPieChartDataResult> Handle(GetTransactionPieChartDataQuery request,
            CancellationToken cancellationToken = default)
        {
            var response = await mediator.Send(new GetTransactionsQuery(), cancellationToken);
            if (response.Transactions.Count == 0)
                return new GetTransactionPieChartDataResult { Data = [new GetTransactionPieChartDataResult.PieChartData { Title = "N/A", Price = 0, Percentage = 100 }] };


            var results = response.Transactions.GroupBy(x => x.CategoryName ?? string.Empty)
                .Select(x => new GetTransactionPieChartDataResult.PieChartData
                {
                    Title = x.Key,
                    Price = x.Sum(y => y.Price)
                }).ToList();



            return new GetTransactionPieChartDataResult { Data = results };
        }
    }
}

public class GetTransactionPieChartDataResult
{
    public List<PieChartData> Data { get; set; } = [];

    public class PieChartData
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public decimal Percentage { get; set; }
    }
}