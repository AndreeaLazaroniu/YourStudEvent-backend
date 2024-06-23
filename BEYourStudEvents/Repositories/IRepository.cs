namespace BEYourStudEvents.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> GetAllAsync();
    Task<TEntity?> FindByIdAsync(int id);
    Task<TEntity?> FindByNameAsync(String name);
    Task<TEntity?> FindUserByIdAsync(String id);
    Task<TEntity> PostAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task DeleteAsync(int id);
}