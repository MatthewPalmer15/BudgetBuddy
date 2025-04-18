using BudgetBuddy.Database.Entities;

namespace BudgetBuddy.Database.Entities.Configuration;

public class Setting : BaseEntity<Guid>
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public bool IsSystemManaged { get; set; }
    public bool IsHidden { get; set; }
}