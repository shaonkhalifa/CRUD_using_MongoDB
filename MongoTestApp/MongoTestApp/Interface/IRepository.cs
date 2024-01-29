using MongoDB.Bson;
using MongoDB.Driver;
using MongoTestApp.Entity;

namespace MongoTestApp.Interface;

public interface IRepository<TDocument> where TDocument : class
{
    Task<TDocument> GetByIdAsync(string id);
    Task<Pager<TDocument>> GetdatawithPaging<T>(FilterDefinition<TDocument> filter, SortDefinition<TDocument> sort, int page, int pagesize);
    Task<IList<TDocument>> GetAllAsync(ProjectionDefinition<TDocument> projection, SortDefinition<TDocument> sort);
    Task<IList<TDocument>> DateFilter(FilterDefinition<TDocument> filter);
    Task<IList<TOutput>> JoinData<TOutput>(PipelineDefinition<TDocument, TOutput> pipeline);
    Task InsertAsync(TDocument entity);
    Task InsertManyAync(IList<TDocument> entity);
    Task UpdateAsync(FilterDefinition<TDocument> filter, TDocument entity);
    Task UpdateFieldAsync(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, bool upsert = false);
    Task<IList<TOutput>> ExecutePipeline<TOutput>(BsonDocument[] stages);
    Task DeleteAsync(string id);
}

