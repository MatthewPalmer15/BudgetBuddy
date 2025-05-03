using MediatR;
using Microsoft.AspNetCore.Components;

namespace BudgetBuddy.App;

public class CustomComponentBase : ComponentBase
{
    [Inject] public IMediator Mediator { get; set; } = null!;
}