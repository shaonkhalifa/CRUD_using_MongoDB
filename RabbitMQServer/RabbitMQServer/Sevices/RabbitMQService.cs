using RabbitMQ.Client;
using RabbitMQServer.Entity;
using RabbitMQServer.Interface;
using System.Text;
using System.Text.Json;


namespace RabbitMQServer.Sevices;

public class RabbitMQService<T> where T : class, IMessage
{
    private readonly IConnection connection;
    public RabbitMQService(IConfiguration configuration)
    {
        var options = configuration.GetSection("RabbitMQ").Get<RabbitMQOptions>();
        var factory = new ConnectionFactory
        {
            HostName = options.HostName,
            UserName = options.UserName,
            Password = options.Password,
            //VirtualHost = options.VirtualHost,
            //Port = options.Port
        };
        connection = factory.CreateConnection();

    }

    public void PublishMessage(T message, string queueName, string exchangeName)
    {
        using (IModel channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: false, autoDelete: false, arguments: null);
            var body = SerializeMessage(message);
            channel.BasicPublish(exchange: exchangeName, routingKey: queueName, basicProperties: null, body: body);
        }
    }

    private byte[] SerializeMessage(T message)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message, options));
    }

    public void Dispose()
    {
        connection.Dispose();
    }
}
