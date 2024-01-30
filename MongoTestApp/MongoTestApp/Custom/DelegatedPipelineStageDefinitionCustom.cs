using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MongoTestApp.Custom;

public class DelegatedPipelineStageDefinitionCustom<TInput, TOutput> : PipelineStageDefinition<TInput, TOutput>
{
    private readonly string _operatorName;
    private readonly Func<IBsonSerializer<TInput>, IBsonSerializerRegistry, LinqProvider, RenderedPipelineStageDefinition<TOutput>> _renderer;

    public DelegatedPipelineStageDefinitionCustom(string operatorName, Func<IBsonSerializer<TInput>, IBsonSerializerRegistry, LinqProvider, RenderedPipelineStageDefinition<TOutput>> renderer)
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
