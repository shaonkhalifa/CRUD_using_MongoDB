using MongoDB.Driver;
using MongoTestApp.Entity;
using MongoTestApp.Implementation;
using MongoTestApp.Interface;
using MongoTestApp.Services;

var builder = WebApplication.CreateBuilder(args);




var connectionString = builder.Configuration.GetValue<string>("MongoDB:ConnectionString");
builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(connectionString));

var databaseName = builder.Configuration.GetValue<string>("MongoDB:DatabaseName");
builder.Services.AddScoped<IRepository<Product>, Repository<Product>>(sp =>
    new Repository<Product>(sp.GetRequiredService<IMongoClient>(), databaseName));
builder.Services.AddScoped<IRepository<Course>, Repository<Course>>(sp =>
    new Repository<Course>(sp.GetRequiredService<IMongoClient>(), databaseName));
builder.Services.AddScoped<IRepository<Subject>, Repository<Subject>>(sp =>
    new Repository<Subject>(sp.GetRequiredService<IMongoClient>(), databaseName));

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CourseService>();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
