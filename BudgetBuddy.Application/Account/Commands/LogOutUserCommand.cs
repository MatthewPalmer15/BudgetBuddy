using MediatR;

namespace BudgetBuddy.Application.Account.Commands;

public class LogOutUserCommand : IRequest<BaseResponse>
{
    internal class Handler : IRequestHandler<LogOutUserCommand, BaseResponse>
    {
        public async Task<BaseResponse> Handle(LogOutUserCommand request, CancellationToken cancellationToken = default)
        {
            SecureStorage.Remove("authentication_token");
            SecureStorage.Remove("authentication_user");
            return BaseResponse.Succeeded();
        }
    }
}