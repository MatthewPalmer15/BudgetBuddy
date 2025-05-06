using BudgetBuddy.Database;
using BudgetBuddy.Database.Entities.Transactions;
using BudgetBuddy.Database.Enums;
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
    public TransactionType Type { get; set; }
    public CategoryEnum Category { get; set; }
    public int Rank { get; set; }
    public Guid? VendorId { get; set; }

    public class Handler(IDbContext context) : IRequestHandler<SaveTransactionCommand, BaseResponse>
    {
        public async Task<BaseResponse> Handle(SaveTransactionCommand request,
            CancellationToken cancellationToken = default)
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
                TransactionDate = request.TransactionDate,
                Type = request.Type,
                Category = request.Category,
                Rank = request.Rank,
                VendorId = request.VendorId
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
            transaction.TransactionDate = request.TransactionDate;
            transaction.Type = request.Type;
            transaction.Category = request.Category;
            transaction.Rank = request.Rank;
            transaction.VendorId = request.VendorId;


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