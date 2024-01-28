namespace MongoTestApp.Entity
{
    public class Pager<TDocument>
    {
        public int Count { get; set; }

        public int Page { get; set; }
        public int TotalPage { get; set; }

        public int Pagesize { get; set; }

        public IEnumerable<TDocument> Data { get; set; }
    }
}
