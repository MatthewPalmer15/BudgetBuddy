using BudgetBuddy.Database.Enums;

namespace BudgetBuddy.Database.Entities.Transactions;

public class Vendor : BaseEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public CategoryEnum Category { get; set; }
    public int Rank { get; set; }
}