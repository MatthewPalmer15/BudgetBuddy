using BudgetBuddy.Application.Account.Commands;
using BudgetBuddy.Infrastructure.Enums.Toast;
using BudgetBuddy.Infrastructure.Services.Toast;
using Microsoft.AspNetCore.Components;

namespace BudgetBuddy.App.Components.Pages.Account;

public partial class Login : CustomComponentBase
{
    public LoginViewModel _model = new();
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IToastManager ToastManager { get; set; }

    private async Task LoginUser()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var response = await Mediator.Send(new LogInUserCommand
        {
            EmailAddress = _model.EmailAddress,
            Password = _model.Password
        }, cancellationToken);

        if (response.Success)
        {
            ToastManager.Show("Save successfully", ToastType.Success);
            await Task.Delay(1000, cancellationToken);
            NavigationManager.NavigateTo("/account");
        }

        ToastManager.Show(string.Join(",", response.Errors.Select(x => x.ErrorMessage).ToList()), ToastType.Error);
    }

    public class LoginViewModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}