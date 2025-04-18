namespace BudgetBuddy.Database.Seeds;

public interface IDataSeedConfiguration<TEntity> where TEntity : class
{
    public TEntity[] Fetch();
}