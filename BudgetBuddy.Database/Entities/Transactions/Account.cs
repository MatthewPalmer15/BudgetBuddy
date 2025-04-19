namespace BudgetBuddy.Database.Entities.Transactions;

public class Account : BaseEntity<Guid>
{
    public string Name { get; set; }
}