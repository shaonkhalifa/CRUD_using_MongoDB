using MongoTestApp.Entity;
using MongoTestApp.Interface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MongoTestApp.Services;

public class RabbitMQService<T> : BackgroundService where T : class, IMessage
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly string _queueName;
    private readonly string _exchangeName;
    private readonly string _routingKey;
    private readonly IServiceProvider _serviceProvider;
    //private readonly IRepository<T> _repository;
    public RabbitMQService(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        var options = configuration.GetSection("RabbitMQ").Get<RabbitMQOptions>();
        _connectionFactory = new ConnectionFactory
        {
            HostName = options.HostName,
            UserName = options.UserName,
            Password = options.Password,

        };

        _queueName = "direct-queue";
        _exchangeName = "DirectExchange";
        _routingKey = "product-routing";
        // _repository = repository;
        _serviceProvider = serviceProvider;
    }




    public override Task StartAsync(CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(_queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(_queueName, _exchangeName, _routingKey);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<T>>();

            var body = ea.Body.ToArray();
            var messageData = DeserializeMessage<T>(body);
            await repository.InsertAsync(messageData);

            channel.BasicAck(ea.DeliveryTag, false);
        };

        channel.BasicConsume(_queueName, true, consumer);
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();
        channel?.Dispose();
        connection?.Dispose();
        return Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(_queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(_queueName, _exchangeName, _routingKey);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<T>>();

            var body = ea.Body.ToArray();
            var messageData = DeserializeMessage<T>(body);
            await repository.InsertAsync(messageData);

            channel.BasicAck(ea.DeliveryTag, false);
        };

        channel.BasicConsume(_queueName, true, consumer);

        await Task.Run(() => Console.WriteLine("Waiting for messages..."), stoppingToken);

    }

    private T DeserializeMessage<T>(byte[] messageBytes)
    {
        return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(messageBytes));
    }


}
