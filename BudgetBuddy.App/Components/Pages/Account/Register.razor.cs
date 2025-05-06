using BudgetBuddy.Application.Account.Commands;
using BudgetBuddy.Infrastructure.Enums.Toast;
using BudgetBuddy.Infrastructure.Services.Toast;
using Microsoft.AspNetCore.Components;

namespace BudgetBuddy.App.Components.Pages.Account;

public partial class Register : CustomComponentBase
{
    private readonly RegisterViewModel _model = new();
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IToastManager ToastManager { get; set; }

    private async Task RegisterUser()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var response = await Mediator.Send(new RegisterUserCommand
        {
            FirstName = _model.FirstName,
            LastName = _model.LastName,
            EmailAddress = _model.EmailAddress,
            Password = _model.Password,
            ConfirmPassword = _model.ConfimPassword
        }, cancellationToken);

        if (response.Success)
        {
            ToastManager.Show("Save successfully", ToastType.Success);
            await Task.Delay(1000, cancellationToken);
            NavigationManager.NavigateTo("/account");
        }

        ToastManager.Show(string.Join(",", response.Errors.Select(x => x.ErrorMessage).ToList()), ToastType.Error);
    }

    private class RegisterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ConfimPassword { get; set; }
    }
}