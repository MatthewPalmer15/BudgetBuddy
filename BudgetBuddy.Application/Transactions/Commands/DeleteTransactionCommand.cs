using BudgetBuddy.Database;
using BudgetBuddy.Database.Entities.Transactions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Application.Transactions.Commands;

public class DeleteTransactionCommand : IRequest<BaseResponse>
{
    public Guid Id { get; set; }

    public class Handler(IDbContext context) : IRequestHandler<DeleteTransactionCommand, BaseResponse>
    {
        public async Task<BaseResponse> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken = default)
        {
            var validationResult = await new Validator().ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return BaseResponse.Failed(validationResult.Errors);

            var transaction = await GetAsync(request, cancellationToken);
            if (transaction == null)
                return BaseResponse.Failed("Transaction not found");


            transaction.Deleted = true;
            context.Transactions.Update(transaction);
            await context.SaveChangesAsync(cancellationToken);

            return BaseResponse.Succeeded();
        }

        private async Task<Transaction?> GetAsync(DeleteTransactionCommand request,
            CancellationToken cancellationToken = default)
        {
            return await (from t in context.Transactions
                          where !t.Deleted && t.Id == request.Id
                          select t).FirstOrDefaultAsync(cancellationToken);
        }
    }

    private class Validator : AbstractValidator<DeleteTransactionCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id cannot be null or empty");
        }
    }
}