namespace BudgetBuddy.Database.Entities.Transactions;

public class Budget : BaseEntity<Guid>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}