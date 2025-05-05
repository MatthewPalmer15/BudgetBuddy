namespace BudgetBuddy.Application.Account.Models;

public class GetUserResult
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string AuthenticationToken { get; set; }
    public bool IsAuthenticated { get; set; }
}