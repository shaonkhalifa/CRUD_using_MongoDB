﻿using MongoDB.Bson;
using MongoDB.Driver;
using MongoTestApp.Entity;
using MongoTestApp.Interface;

namespace MongoTestApp.Services;

public class CourseService
{
    private readonly IRepository<Course> _repository;

    public CourseService(IRepository<Course> repository)
    {
        _repository = repository;
    }

    public async Task<List<CourseDetails>> GetCourseDetails()
    {




        BsonDocument lookupStage = new BsonDocument("$lookup", new BsonDocument
                        {
                            { "from", "subject" },
                            { "localField", "subjects" },
                            { "foreignField", "_id" },
                            { "as", "subjectsDetails" }
                        });

        //BsonDocument lookupStage = new BsonDocument("$lookup", new BsonDocument
        //            {
        //                { "from", "Subject" },
        //                { "localField", "Subjects" },
        //                { "foreignField", "Id" },
        //                { "as", "subjectsDetails" }
        //            });



        BsonDocument projectStage = new BsonDocument("$project", new BsonDocument
                {
                    { "_id", 1 },
                    { "name", 1 },
                    { "subjectsDetails", new BsonDocument("$map", new BsonDocument
                        {
                            { "input", "$subjectsDetails" },
                            { "as", "subject" },
                            { "in", "$$subject.name" }
                        })
                    }
                });




        var pipeline = PipelineDefinition<Course, CourseDetails>.Create(
                             new IPipelineStageDefinition[]
                             {
                                 new BsonDocumentPipelineStageDefinition<Course, CourseDetails>(lookupStage),
                                 new BsonDocumentPipelineStageDefinition<CourseDetails, CourseDetails>(projectStage)
                             });

        var data = await _repository.JoinData<CourseDetails>(pipeline);

        return data.ToList();


    }


}
