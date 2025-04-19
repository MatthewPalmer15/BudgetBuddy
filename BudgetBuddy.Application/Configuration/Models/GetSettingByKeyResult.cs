namespace BudgetBuddy.Application.Configuration.Models;

public class GetSettingByKeyResult
{
    public Guid Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public bool IsSystemManaged { get; set; }
}