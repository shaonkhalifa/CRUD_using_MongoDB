using MongoDB.Bson;
using MongoDB.Driver;
namespace MongoTestApp.Custom
{
    public static class CustomPipelineStageDefinitionBuilder
    {
        public static PipelineStageDefinition<TInput, AggregateCountResultInt> CustomCount<TInput>(
    AggregateExpressionDefinition<TInput, bool> filter = null)
        {
            const string operatorName = "$count";

            var stage = new DelegatedPipelineStageDefinition<TInput, AggregateCountResultInt>(
                operatorName,
                (s, sr, linqProvider) =>
                {
                    var document = new BsonDocument();

                    if (filter != null)
                    {
                        var renderedFilter = filter.Render(s, sr, linqProvider);
                        document.Add("filter", renderedFilter);
                    }

                    return new RenderedPipelineStageDefinition<AggregateCountResultInt>(
                        operatorName,
                        document,
                        sr.GetSerializer<AggregateCountResultInt>());
                });

            return stage;
        }

    }
}
