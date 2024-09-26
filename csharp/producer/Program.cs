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

var random = new Random();
int messageId = 1;

while(true) {

    var publishingTime = random.Next(1, 4);

    var message  = $"Message {messageId}: Sent to queue";
    var encodedMessage = Encoding.UTF8.GetBytes(message);

    // "" exchange = default exchange
    channel.BasicPublish("", "letterbox", null, encodedMessage);

    Console.WriteLine($"Published message: {message}");
    
    Task.Delay(TimeSpan.FromSeconds(publishingTime)).Wait();
    messageId++;
}

