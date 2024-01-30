namespace MongoTestApp.Interface;

public interface IRabbitMQService
{
    Task SendAsync<T>(string queue, T message);
    Task ReceiveAsync<T>(string queue, Action<T> onMessage);
}
