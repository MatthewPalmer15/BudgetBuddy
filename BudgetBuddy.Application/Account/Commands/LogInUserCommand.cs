using BudgetBuddy.Application.Account.Models;
using BudgetBuddy.Infrastructure.Services.Serialization;
using FluentValidation;
using MediatR;
using RestSharp;

namespace BudgetBuddy.Application.Account.Commands;

public class LogInUserCommand : IRequest<BaseResponse>
{
    public string EmailAddress { get; set; }
    public string Password { get; set; }

    internal class Handler(IJsonSerializer serializer) : IRequestHandler<LogInUserCommand, BaseResponse>
    {
        private readonly IRestClient _client = new RestClient("");

        private readonly IJsonSerializer _serializer = serializer;

        public async Task<BaseResponse> Handle(LogInUserCommand request, CancellationToken cancellationToken = default)
        {
            var apiRequest = new RestRequest("/auth/login", Method.Post);
            apiRequest.AddJsonBody(request);

            var response = await _client.ExecuteAsync<ApiResponse>(apiRequest, cancellationToken);
            if (!response.IsSuccessful || response.IsSuccessStatusCode)
                return BaseResponse.Failed(response.ErrorMessage ?? "Failed to connect to the server.");

            if (response is { Data: null or { Success: false } })
                return BaseResponse.Failed(response?.Data?.Errors ??
                                           [new RequestError("", "Failed to retrieve data from the server")]);

            var accountModel = _serializer.Serialize(new AccountModel
            {
                Id = response.Data.Id,
                FirstName = response.Data.FirstName,
                LastName = response.Data.LastName,
                EmailAddress = response.Data.EmailAddress
            });

            await SecureStorage.SetAsync("authentication_token", response.Data.AuthenticationToken);
            await SecureStorage.SetAsync("authentication_user", accountModel);
            return BaseResponse.Succeeded();
        }
    }

    private class Validator : AbstractValidator<LogInUserCommand>
    {
        public Validator()
        {
            RuleFor(x => x.EmailAddress).NotEmpty().WithMessage("Email Address is mandatory");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is mandatory");
        }
    }

    private class ApiResponse : BaseResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string AuthenticationToken { get; set; }
    }
}