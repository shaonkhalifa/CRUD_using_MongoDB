using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MongoTestApp.Custom;

/// <summary>
/// Result type for the aggregate $count stage.
/// </summary>
public class AggregateCountResultInt
{
    private int _count;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateCountResultInt"/> class.
    /// </summary>
    /// <param name="count">The count.</param>
    /// 
    public AggregateCountResultInt(int count)
    {
        _count = count;
    }
    /// <summary>
    /// Gets the count.
    /// </summary>
    /// <value>
    /// The count.
    /// </value>
    [BsonElement("count")]
    public int Count
    {
        get { return _count; }
    }
}
public class DelegatedPipelineStageDefinition<TInput, TOutput> : PipelineStageDefinition<TInput, TOutput>
{
    private readonly string _operatorName;
    private readonly Func<IBsonSerializer<TInput>, IBsonSerializerRegistry, LinqProvider, RenderedPipelineStageDefinition<TOutput>> _renderer;

    public DelegatedPipelineStageDefinition(string operatorName, Func<IBsonSerializer<TInput>, IBsonSerializerRegistry, LinqProvider, RenderedPipelineStageDefinition<TOutput>> renderer)
    {
        _operatorName = operatorName;
        _renderer = renderer;
    }

    public override string OperatorName
    {
        get { return _operatorName; }
    }

    public override RenderedPipelineStageDefinition<TOutput> Render(IBsonSerializer<TInput> inputSerializer, IBsonSerializerRegistry serializerRegistry, LinqProvider linqProvider)
    {
        return _renderer(inputSerializer, serializerRegistry, linqProvider);
    }
}
