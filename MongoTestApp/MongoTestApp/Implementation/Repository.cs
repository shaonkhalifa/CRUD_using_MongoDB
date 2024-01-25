﻿using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoTestApp.Interface;

namespace MongoTestApp.Implementation;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly IMongoCollection<T> _collection;

    public Repository(IMongoClient client, string databaseName)
    {

        _collection = client.GetDatabase(databaseName).GetCollection<T>(typeof(T).Name);
    }


    public async Task<T> GetByIdAsync(string id)
    {
        return await _collection.Find(Builders<T>.Filter.Eq("_id", id)).FirstOrDefaultAsync();
    }

    public async Task<IList<T>> GetAllAsync(ProjectionDefinition<T> projection = null, SortDefinition<T> sort = null)
    {
        if (projection != null && sort != null)
        {
            var query = _collection.Find(FilterDefinition<T>.Empty).Project(projection).Sort(sort);

            var results = await query.ToListAsync();
            var deserializedResults = results.Select(doc => BsonSerializer.Deserialize<T>(doc)).ToList();
            return deserializedResults;

        }
        else
        {
            return await _collection.Find(FilterDefinition<T>.Empty).ToListAsync();
        }

    }

    public async Task InsertAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(FilterDefinition<T> filter, T entity)
    {
        await _collection.ReplaceOneAsync(filter, entity);
    }




    public async Task DeleteAsync(string id)
    {
        await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", id));
    }

    public async Task InsertManyAync(IList<T> entity)
    {
        await _collection.InsertManyAsync(entity);
    }

    public async Task UpdateFieldAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, bool upsert = false)
    {
        await _collection.UpdateOneAsync(filter, update, options: new UpdateOptions { IsUpsert = upsert });
    }

    public async Task<IList<T>> DateFilter(FilterDefinition<T> filter = null)
    {
        var query = _collection.Find(filter);
        return await query.ToListAsync();

    }

    public async Task<IList<TOutput>> JoinData<TOutput>(PipelineDefinition<T, TOutput> pipeline)
    {
        var query = await _collection.AggregateAsync(pipeline);
        return await query.ToListAsync();
    }







}

