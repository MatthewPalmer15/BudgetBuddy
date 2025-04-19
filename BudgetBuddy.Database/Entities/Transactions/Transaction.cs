using BudgetBuddy.Database.Enums;

namespace BudgetBuddy.Database.Entities.Transactions;

public class Transaction : BaseEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public DateTime? TransactionDate { get; set; }
    public TransactionType Type { get; set; }
    public CategoryEnum Category { get; set; }
    public int Rank { get; set; }
    public bool Essential { get; set; }
    public Guid? VendorId { get; set; }
    public virtual Vendor? Vendor { get; set; }
}