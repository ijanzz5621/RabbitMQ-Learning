using System;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost", Port = 5672 };

// No need to close the connection for using
using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "letterbox", 
    durable: false, 
    exclusive: false, 
    autoDelete: false,
    arguments: null);

var message  = "This is my second message from CSharp Producer";
var encodedMessage = Encoding.UTF8.GetBytes(message);

// "" exchange = default exchange
channel.BasicPublish("", "letterbox", null, encodedMessage);

Console.WriteLine($"Published message: {message}");