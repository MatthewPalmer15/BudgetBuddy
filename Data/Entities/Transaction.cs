namespace BudgetBuddy.Data.Entities;

public class Transaction : BaseEntity<Guid>
{
    public enum TransactionType
    {
        Unknown = 0,
        Income = 1,
        Outcome = 2,
        Pending = 3
    }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsRecurring { get; set; }
    public TransactionType Type { get; set; }
    public int Rank { get; set; }

    public Guid? ServiceProviderId { get; set; }
    public virtual ServiceProvider? ServiceProvider { get; set; }

}

public class Category : BaseEntity<Guid>
{
    public string Title { get; set; }
}

public class ServiceProvider : BaseEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    public Guid? CategoryId { get; set; }
    public virtual Category? Category { get; set; }

    public int Rank { get; set; }
}