using BudgetBuddy.Database;
using BudgetBuddy.Database.Entities.Transactions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Application.Transactions.Commands;

public class SaveTransactionCommand : IRequest<BaseResponse>
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public DateTime? TransactionDate { get; set; }
    public bool IsRecurring { get; set; }
    public Transaction.TransactionType Type { get; set; }
    public int Rank { get; set; }
    public Guid? ServiceProviderId { get; set; }

    public class Handler(IDbContext context) : IRequestHandler<SaveTransactionCommand, BaseResponse>
    {
        public async Task<BaseResponse> Handle(SaveTransactionCommand request, CancellationToken cancellationToken = default)
        {
            var validationResult = await new Validator().ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return BaseResponse.Failed(validationResult.Errors);

            var response = request.Id.HasValue
                ? await UpdateAsync(request, cancellationToken)
                : await AddAsync(request, cancellationToken);

            return response;
        }

        private async Task<BaseResponse> AddAsync(SaveTransactionCommand request,
            CancellationToken cancellationToken = default)
        {
            var transaction = new Transaction
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StartDate = request.TransactionDate,
                IsRecurring = request.IsRecurring,
                Type = request.Type,
                Rank = request.Rank,
                ServiceProviderId = request.ServiceProviderId
            };

            context.Transactions.Add(transaction);
            await context.SaveChangesAsync(cancellationToken);

            return BaseResponse.Succeeded();
        }

        private async Task<BaseResponse> UpdateAsync(SaveTransactionCommand request,
            CancellationToken cancellationToken = default)
        {
            var transaction = await GetAsync(request, cancellationToken);
            if (transaction == null)
                return BaseResponse.Failed("Unable to get transaction. Please try again");

            transaction.Name = request.Name;
            transaction.Description = request.Description;
            transaction.Price = request.Price;
            transaction.StartDate = request.TransactionDate;
            transaction.Type = request.Type;
            transaction.IsRecurring = request.IsRecurring;
            transaction.Rank = request.Rank;
            transaction.ServiceProviderId = request.ServiceProviderId;


            context.Transactions.Update(transaction);
            await context.SaveChangesAsync(cancellationToken);

            return BaseResponse.Succeeded();
        }

        private async Task<Transaction?> GetAsync(SaveTransactionCommand request,
            CancellationToken cancellationToken = default)
        {
            return await (from t in context.Transactions
                          where !t.Deleted && t.Id == request.Id
                          select t).FirstOrDefaultAsync(cancellationToken);
        }
    }

    private class Validator : AbstractValidator<SaveTransactionCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Name cannot be null or empty");
        }
    }
}