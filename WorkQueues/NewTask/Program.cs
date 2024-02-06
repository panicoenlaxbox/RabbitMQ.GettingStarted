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
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

// Fair dispatch instead of round-robin
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

var message = GetMessage(args);
// message content is a byte array
var body = Encoding.UTF8.GetBytes(message);

var properties = channel.CreateBasicProperties();
properties.Persistent = true;

channel.BasicPublish(exchange: string.Empty,
    routingKey: "hello",
    basicProperties: properties,
    body: body);
Console.WriteLine($" [x] Sent {message}");

// Console.WriteLine(" Press [enter] to exit.");
// Console.ReadLine();

static string GetMessage(string[] args)
{
    return args.Length > 0 ? string.Join(" ", args) : "Hello World!";
}