using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
// connection abstracts the socket connection,
// and takes care of protocol version negotiation and authentication
using var connection = factory.CreateConnection();
// channel, which is where most of the API for getting things done resides
using var channel = connection.CreateModel();

// it will only be created if it doesn't exist already
channel.QueueDeclare(queue: "hello",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null);

const string message = "Hello World!";
// message content is a byte array
var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(exchange: string.Empty,
    routingKey: "hello",
    basicProperties: null,
    body: body);
Console.WriteLine($" [x] Sent {message}");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();