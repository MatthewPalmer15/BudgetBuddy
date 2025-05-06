using FluentValidation;
using MediatR;
using RestSharp;

namespace BudgetBuddy.Application.Account.Commands;

public class RegisterUserCommand : IRequest<BaseResponse>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }

    internal class Handler : IRequestHandler<RegisterUserCommand, BaseResponse>
    {
        private readonly IRestClient _client = new RestClient("");

        public async Task<BaseResponse> Handle(RegisterUserCommand request,
            CancellationToken cancellationToken = default)
        {
            var apiRequest = new RestRequest("/auth/register", Method.Post);
            apiRequest.AddJsonBody(request);

            var response = await _client.ExecuteAsync<BaseResponse>(apiRequest, cancellationToken);
            if (!response.IsSuccessful || response.IsSuccessStatusCode)
                return BaseResponse.Failed(response.ErrorMessage ?? "Failed to connect to the server.");

            if (response is { Data: null or { Success: false } })
                return BaseResponse.Failed(response?.Data?.Errors ?? [new RequestError("", "Failed to retrieve data")]);

            return BaseResponse.Succeeded();
        }
    }

    private class Validator : AbstractValidator<RegisterUserCommand>
    {
        public Validator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is mandatory");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is mandatory");
            RuleFor(x => x.EmailAddress).NotEmpty().WithMessage("Email Address is mandatory");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is mandatory");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm Password is mandatory");
            RuleFor(x => x).Must(x => x.Password == x.ConfirmPassword).WithMessage("Passwords must match");
        }
    }
}