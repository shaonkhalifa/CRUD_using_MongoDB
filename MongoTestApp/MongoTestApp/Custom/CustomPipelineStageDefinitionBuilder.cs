using MongoDB.Bson;
using MongoDB.Driver;
namespace MongoTestApp.Custom
{
    /// <summary>
    /// Provides a custom pipeline stage definition for the $sum aggregation operator.
    /// </summary
    public static class CustomPipelineStageDefinitionBuilder
    {
        // <summary>
        /// Creates a custom $sum pipeline stage with an optional filter.
        /// </summary>
        /// <typeparam name="TInput">The input type.</typeparam>
        /// <param name="filter">An optional filter expression.</param>
        /// <returns>The custom $sum pipeline stage definition.</returns>
        public static PipelineStageDefinition<TInput, AggregateSumResult> CustomSum<TInput>(
    FilterDefinition<TInput> filter = null, string fieldName = null)
        {
            const string operatorName = "$sum";

            var stage = new DelegatedPipelineStageDefinitionCustom<TInput, AggregateSumResult>(
                operatorName,
                (s, sr, linqProvider) =>
                {
                    var document = new BsonDocument();

                    //if (filter != null)
                    //{
                    //    var renderedFilter = filter.Render(s, sr, linqProvider);
                    //    document.Add("filter", renderedFilter);
                    //}

                    if (!string.IsNullOrEmpty(fieldName))
                    {
                        document.Add("field", fieldName);
                    }

                    return new RenderedPipelineStageDefinition<AggregateSumResult>(
                        operatorName,
                        document,
                        sr.GetSerializer<AggregateSumResult>());
                });

            return stage;
        }


    }
}
