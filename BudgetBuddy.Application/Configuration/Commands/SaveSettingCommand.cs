using BudgetBuddy.Database;
using BudgetBuddy.Database.Entities.Configuration;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Application.Configuration.Commands;

public class SaveSettingCommand : IRequest<BaseResponse>
{
    public Guid? Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public bool IsSystemManaged { get; set; }

    public class Handler(IDbContext context) : IRequestHandler<SaveSettingCommand, BaseResponse>
    {
        public async Task<BaseResponse> Handle(SaveSettingCommand request,
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

        private async Task<BaseResponse> AddAsync(SaveSettingCommand request,
            CancellationToken cancellationToken = default)
        {
            var setting = new Setting
            {
                Key = request.Key,
                Value = request.Value,
                IsSystemManaged = request.IsSystemManaged
            };

            context.Settings.Add(setting);
            await context.SaveChangesAsync(cancellationToken);

            return BaseResponse.Succeeded();
        }

        private async Task<BaseResponse> UpdateAsync(SaveSettingCommand request,
            CancellationToken cancellationToken = default)
        {
            var setting = await GetAsync(request, cancellationToken);
            if (setting == null)
                return BaseResponse.Failed("Unable to get setting. Please try again");

            setting.Key = request.Key;
            setting.Value = request.Value;
            setting.IsSystemManaged = request.IsSystemManaged;

            context.Settings.Update(setting);
            await context.SaveChangesAsync(cancellationToken);

            return BaseResponse.Succeeded();
        }

        private async Task<Setting?> GetAsync(SaveSettingCommand request,
            CancellationToken cancellationToken = default)
        {
            return await (from s in context.Settings
                          where !s.Deleted && s.Id == request.Id
                          select s).FirstOrDefaultAsync(cancellationToken);
        }
    }

    private class Validator : AbstractValidator<SaveSettingCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Key)
                .NotNull().WithMessage("Key cannot be null or empty");
        }
    }
}