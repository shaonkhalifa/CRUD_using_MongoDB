using MongoDB.Bson;
using MongoDB.Driver;
using MongoTestApp.Entity;
using MongoTestApp.Interface;

namespace MongoTestApp.Services;

public class CourseService
{
    private readonly IRepository<Course> _repository;
    private readonly IRepository<Subject> _repository1;

    public CourseService(IRepository<Course> repository, IRepository<Subject> repository1)
    {
        _repository = repository;
        _repository1 = repository1;

    }

    public async Task<List<CourseDetails>> GetCourseDetails()
    {

        BsonDocument lookupStage = new BsonDocument("$lookup", new BsonDocument
        {
            {"from", "Subject"},
            {"localField", "SubjectList"},
            {"foreignField", "_id"},
            {"as", "SubjectDetails"}
        });




        BsonDocument projectStage = new BsonDocument("$project", new BsonDocument
        {
                {"_id", 1},
                {"CourseName", 1},
                {"SubjectDetails", new BsonDocument("$map", new BsonDocument
                {
                    {"input", "$SubjectDetails"},
                    {"as", "subject"},
                    {"in", "$$subject.SubjectName"}
                })}
        });




        var pipeline = PipelineDefinition<Course, CourseDetails>.Create(
                             new IPipelineStageDefinition[]
                             {
                                 new BsonDocumentPipelineStageDefinition<Course, CourseDetails>(lookupStage),
                                 new BsonDocumentPipelineStageDefinition<CourseDetails, CourseDetails>(projectStage)
                             });


        //Different Way......Sending Pipeline
        var pipelinea = new BsonDocument[]
                {
                    new BsonDocument("$lookup", new BsonDocument
                    {
                        {"from", "Subject"},
                        {"localField", "SubjectList"},
                        {"foreignField", "_id"},
                        {"as", "SubjectDetails"}
                    }),

                    new BsonDocument("$project", new BsonDocument
                    {
                        {"_id", 1},
                        {"CourseName", 1},
                        {"SubjectDetails", new BsonDocument("$map", new BsonDocument
                        {
                            {"input", "$SubjectDetails"},
                            {"as", "subject"},
                            {"in", "$$subject.SubjectName"}
                        })}
                    })
                };


        var data = await _repository.JoinData<CourseDetails>(pipeline);


        return data.ToList();


    }

    public async Task<List<CourseDetails>> GetCourseDetails1()
    {

        var pipeline = new BsonDocument[]
                {
                    new BsonDocument("$lookup", new BsonDocument
                    {
                        {"from", "Subject"},
                        {"localField", "SubjectList"},
                        {"foreignField", "_id"},
                        {"as", "SubjectDetails"}
                    }),

                    new BsonDocument("$project", new BsonDocument
                    {
                        {"_id", 1},
                        {"CourseName", 1},
                        {"SubjectDetails", new BsonDocument("$map", new BsonDocument
                        {
                            {"input", "$SubjectDetails"},
                            {"as", "subject"},
                            {"in", "$$subject.SubjectName"}
                        })}
                    })
                };





        var data = await _repository.ExecutePipeline<CourseDetails>(pipeline);


        return data.ToList();


    }

    public async Task DataInsert(CourseSubjectInserDto data)
    {

        Course course = new Course()
        {
            Id = data.CourseId,
            CourseName = data.CourseName,
            SubjectList = data.SubjectList,
        };

        Subject subject = new Subject()
        {
            Id = data.SubjectId,
            name = data.name,
        };


        await _repository.RunTransactionAsync(async (session) =>
        {
            await _repository.InsertAsync(course);
            await _repository1.InsertAsync(subject);
        });

    }


}


