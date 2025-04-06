namespace BudgetBuddy.Data.Seeds;

public interface IDataSeedConfiguration<TEntity> where TEntity : class
{
    public TEntity[] Fetch();
}