namespace MongoTestApp.Entity
{
    public class CourseSubjectInserDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = null!;
        public int[] SubjectList { get; set; }
        public int SubjectId { get; set; }
        public string name { get; set; } = null!;
    }
}
