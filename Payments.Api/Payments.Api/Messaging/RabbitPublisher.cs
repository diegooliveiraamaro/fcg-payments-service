using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

public class RabbitPublisher
{
    private readonly IConnection _connection;
    private readonly RabbitMQ.Client.IModel _channel;

    public RabbitPublisher()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "rabbitmq", // 🔥 nome do service
            Port = 5672
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: "payment-created",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    public void Publish(object message)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        _channel.BasicPublish(
            exchange: "",
            routingKey: "payment-created",
            basicProperties: null,
            body: body);
    }
}