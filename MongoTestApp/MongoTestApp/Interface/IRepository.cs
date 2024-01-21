using MongoDB.Driver;

namespace MongoTestApp.Interface;

public interface IRepository<T> where T : IEntity
{
    Task<T> GetByIdAsync(string id);
    Task<IList<T>> GetAllAsync(FilterDefinition<T> filter);
    Task InsertAsync(T entity);
    Task InsertManyAync(IList<T> entity);
    Task UpdateAsync(T entity);
    Task UpdateFieldAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, bool upsert = false);
    Task DeleteAsync(string id);
}
public interface IEntity
{
    string Id { get; set; }
}
