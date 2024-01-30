using MongoDB.Bson.Serialization.Attributes;

namespace MongoTestApp.Custom;

/// <summary>
/// Result type for the aggregate $count stage.
/// </summary>
public class AggregateSumResult
{
    private decimal _sum;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateSumResult"/> class.
    /// </summary>
    /// <param name="sum">The count.</param>
    /// 
    public AggregateSumResult(decimal sum)
    {
        _sum = sum;
    }
    /// <summary>
    /// Gets the count.
    /// </summary>
    /// <value>
    /// The count.
    /// </value>
    [BsonElement("sum")]
    public decimal Sum
    {
        get { return _sum; }
    }
}
