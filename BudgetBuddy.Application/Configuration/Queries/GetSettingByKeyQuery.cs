using BudgetBuddy.Application.Configuration.Models;
using BudgetBuddy.Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Application.Configuration.Queries;

public class GetSettingByKeyQuery : IRequest<GetSettingByKeyResult?>
{
    public string Key { get; set; }

    public class Handler(IDbContext context) : IRequestHandler<GetSettingByKeyQuery, GetSettingByKeyResult?>
    {
        public async Task<GetSettingByKeyResult?> Handle(GetSettingByKeyQuery request,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await new Validator().ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return null;

            return await (from s in context.Settings
                          where !s.Deleted && s.Key == request.Key
                          select new GetSettingByKeyResult
                          {
                              Id = s.Id,
                              Key = s.Key,
                              Value = s.Value,
                              IsSystemManaged = s.IsSystemManaged
                          }).FirstOrDefaultAsync(cancellationToken);
        }
    }

    private class Validator : AbstractValidator<GetSettingByKeyQuery>
    {
        public Validator()
        {
            RuleFor(x => x.Key)
                .NotNull().WithMessage("Key cannot be null or empty");
        }
    }
}