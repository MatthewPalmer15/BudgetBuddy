using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BudgetBuddy.App;

public class CustomComponentBase : ComponentBase
{
    [Inject] public IMediator Mediator { get; set; } = null!;
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("marukiInit");
        }
    }

}