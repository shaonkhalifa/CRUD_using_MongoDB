using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoTestApp.Custom;
using MongoTestApp.Entity;
using MongoTestApp.Interface;

namespace MongoTestApp.Implementation;

public class Repository<TDocument> : IRepository<TDocument> where TDocument : class
{
    private readonly IMongoCollection<TDocument> _collection;
    private readonly IMongoClient _client;

    public Repository(IMongoClient client, string databaseName)
    {

        _collection = client.GetDatabase(databaseName).GetCollection<TDocument>(typeof(TDocument).Name);
        _client = client;
    }


    public async Task<TDocument> GetByIdAsync(string id)
    {
        return await _collection.Find(Builders<TDocument>.Filter.Eq("_id", id)).FirstOrDefaultAsync();
    }

    public async Task<IList<TDocument>> GetAllAsync(ProjectionDefinition<TDocument> projection = null, SortDefinition<TDocument> sort = null)
    {

        var sumStage = CustomPipelineStageDefinitionBuilder.CustomSum<TDocument>();
        var pipeline = PipelineDefinition<Product, decimal>.Create(new[]
        {
          PipelineStageDefinitionBuilder.Count<TDocument>()
        });

        if (projection != null && sort != null)
        {
            var query = _collection.Find(FilterDefinition<TDocument>.Empty).Project(projection).Sort(sort);

            var results = await query.ToListAsync();
            var deserializedResults = results.Select(doc => BsonSerializer.Deserialize<TDocument>(doc)).ToList();
            return deserializedResults;

        }
        else
        {
            return await _collection.Find(FilterDefinition<TDocument>.Empty).ToListAsync();
        }

    }


    public async Task InsertAsync(TDocument entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(FilterDefinition<TDocument> filter, TDocument entity)
    {
        await _collection.ReplaceOneAsync(filter, entity);
    }




    public async Task DeleteAsync(string id)
    {
        await _collection.DeleteOneAsync(Builders<TDocument>.Filter.Eq("_id", id));
    }

    public async Task InsertManyAync(IList<TDocument> entity)
    {
        await _collection.InsertManyAsync(entity);
    }

    public async Task UpdateFieldAsync(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, bool upsert = false)
    {
        await _collection.UpdateOneAsync(filter, update, options: new UpdateOptions { IsUpsert = upsert });
    }

    public async Task<IList<TDocument>> DateFilter(FilterDefinition<TDocument> filter = null)
    {
        var query = _collection.Find(filter);
        return await query.ToListAsync();

    }

    public async Task<IList<TOutput>> JoinData<TOutput>(PipelineDefinition<TDocument, TOutput> pipeline)
    {


        var query = await _collection.AggregateAsync(pipeline);
        return await query.ToListAsync();
    }


    public async Task<IList<TOutput>> ExecutePipeline<TOutput>(BsonDocument[] stages)
    {
        var pipeline = PipelineDefinition<TDocument, TOutput>.Create(stages);
        var query = await _collection.AggregateAsync(pipeline);
        return await query.ToListAsync();
    }


    public async Task<Pager<TDocument>> GetdatawithPaging<T>(FilterDefinition<TDocument> filter, SortDefinition<TDocument> sort, int page, int pagesize)
    {
        page = page != 0 ? page : 1;
        pagesize = (pagesize != 0 || pagesize != null || pagesize > 50) ? pagesize : 50;

        var countFact = AggregateFacet.Create("count", PipelineDefinition<TDocument, AggregateCountResult>.Create(new[]
        {
          PipelineStageDefinitionBuilder.Count<TDocument>()
        }));

        var dataFact = AggregateFacet.Create("data", PipelineDefinition<TDocument, TDocument>.Create(new[]
        {
            PipelineStageDefinitionBuilder.Sort(sort),
            PipelineStageDefinitionBuilder.Skip<TDocument>((page-1)*pagesize),
            PipelineStageDefinitionBuilder.Limit<TDocument>(pagesize),


        }));

        List<AggregateFacetResults> aggregation = await _collection.Aggregate().Match(filter).Facet(countFact, dataFact).ToListAsync();
        var documents = aggregation.First();
        var count = aggregation.First().Facets.First(x => x.Name == "count").Output<AggregateCountResult>()?.FirstOrDefault()?.Count;


        var totalPage = (int)Math.Ceiling((double)count / pagesize);


        var data = aggregation.First().Facets.First(x => x.Name == "data").Output<TDocument>();

        var d = aggregation.First().Facets.First();
        return new Pager<TDocument>
        {
            Count = (int)count,
            Data = data,
            Pagesize = pagesize,
            Page = page,
            TotalPage = totalPage,
        };



    }

    public async Task<decimal> GetSumResult(string fieldName = null)
    {
        //var collection = _database.GetCollection<TDocument>(collectionName);

        if (fieldName != null)
        {
            var sumStage = CustomPipelineStageDefinitionBuilder.CustomSum<TDocument>(fieldName);

            // Create the aggregation pipeline with the custom sum stage
            var pipeline = PipelineDefinition<TDocument, AggregateSumResult>.Create(new IPipelineStageDefinition[]
            {
            sumStage
            });

            // Execute the aggregation pipeline
            var result = await _collection.AggregateAsync(pipeline);

            // Access the result
            decimal sumValue = result.FirstOrDefault()?.Sum ?? 0;
            return sumValue;
        }
        else
        {
            return 0;
        }
    }

    public async Task RunTransactionAsync(Func<IClientSessionHandle, Task> transactionFunc)
    {
        using (var session = await _client.StartSessionAsync())
        {
            session.StartTransaction();

            try
            {
                await transactionFunc(session);

                session.CommitTransaction();
            }
            catch (Exception)
            {
                session.AbortTransaction();
                throw;
            }
        }
    }
}

