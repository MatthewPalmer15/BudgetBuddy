using BudgetBuddy.Application.Account.Models;
using BudgetBuddy.Application.Account.Queries;
using Microsoft.AspNetCore.Components;

namespace BudgetBuddy.App.Components.Pages.Invoice;

public partial class Index : CustomComponentBase
{
    [Inject] public NavigationManager Navigation { get; set; }

    private GetUserResult User { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var cancellationToken = new CancellationTokenSource().Token;
        User = await Mediator.Send(new GetUserQuery(), cancellationToken);
        if (!User.IsAuthenticated) Navigation.NavigateTo("/account/login");
    }
}