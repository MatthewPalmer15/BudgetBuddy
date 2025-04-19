using BudgetBuddy.Database.Enums;

namespace BudgetBuddy.Database.Entities.Transactions;

public class Subscription : BaseEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TransactionType Type { get; set; }
    public RecurringType RecurringType { get; set; }
    public int Rank { get; set; }
    public bool Essential { get; set; }

    public Guid? ServiceProviderId { get; set; }
    public virtual ServiceProvider? ServiceProvider { get; set; }
}