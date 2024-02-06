using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "hello",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (object? model, BasicDeliverEventArgs ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");
    Console.WriteLine(" [x] Done");
    
    // Manual Acknowledgement
    ((EventingBasicConsumer)model!).Model.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
    // channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

};
channel.BasicConsume(queue: "hello",
    consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();