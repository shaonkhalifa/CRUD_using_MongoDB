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

    public async Task<List<CourseDetails>> GetCourseDetails1()
    {




        //BsonDocument lookupStage = new BsonDocument("$lookup", new BsonDocument
        //                {
        //                    { "from", "subject" },
        //                    { "localField", "subjects" },
        //                    { "foreignField", "_id" },
        //                    { "as", "subjectsDetails" }
        //                });

        ////BsonDocument lookupStage = new BsonDocument("$lookup", new BsonDocument
        ////            {
        ////                { "from", "Subject" },
        ////                { "localField", "Subjects" },
        ////                { "foreignField", "Id" },
        ////                { "as", "subjectsDetails" }
        ////            });



        //BsonDocument projectStage = new BsonDocument("$project", new BsonDocument
        //        {
        //            { "_id", 1 },
        //            { "name", 1 },
        //            { "subjectsDetails", new BsonDocument("$map", new BsonDocument
        //                {
        //                    { "input", "$subjectsDetails" },
        //                    { "as", "subject" },
        //                    { "in", "$$subject.name" }
        //                })
        //            }
        //        });




        //var pipeline = PipelineDefinition<Course, CourseDetails>.Create(
        //                     new IPipelineStageDefinition[]
        //                     {
        //                         new BsonDocumentPipelineStageDefinition<Course, CourseDetails>(lookupStage),
        //                         new BsonDocumentPipelineStageDefinition<CourseDetails, CourseDetails>(projectStage)
        //                     });

        var pipeline = new BsonDocument[]
                {
                    new BsonDocument("$lookup", new BsonDocument
                    {
                        {"from", "subject"},
                        {"localField", "subjects"},
                        {"foreignField", "_id"},
                        {"as", "subjectsDetails"}
                    }),

                    new BsonDocument("$project", new BsonDocument
                    {
                        {"_id", 1},
                        {"name", 1},
                        {"subjectsDetails", new BsonDocument("$map", new BsonDocument
                        {
                            {"input", "$subjectsDetails"},
                            {"as", "subject"},
                            {"in", "$$subject.name"}
                        })}
                    })
                };


        var data = await _repository.JoinData<CourseDetails>(pipeline);


        return data.ToList();


    }

    public async Task<List<CourseDetails>> GetCourseDetails()
    {
        var pipeline = new BsonDocument[]
                {
                    new BsonDocument("$lookup", new BsonDocument
                    {
                        {"from", "subject"},
                        {"localField", "subjects"},
                        {"foreignField", "_id"},
                        {"as", "subjectsDetails"}
                    }),

                    //new BsonDocument("$project", new BsonDocument
                    //{
                    //    {"_id", 1},
                    //    {"name", 1},
                    //    {"subjectsDetails", new BsonDocument("$map", new BsonDocument
                    //    {
                    //        {"input", "$subjectsDetails"},
                    //        {"as", "subject"},
                    //        {"in", "$$subject.name"}
                    //    })}
                    //})
                };

        var data = await _repository.ExecutePipeline<CourseDetails>(pipeline);


        return data.ToList();


    }


}
