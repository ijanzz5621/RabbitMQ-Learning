using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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

var consumer = new EventingBasicConsumer(channel);

var random = new Random();

consumer.Received += (model, ea) => {

    var processingTime = random.Next(1, 6);

    var body = ea.Body.ToArray();
    
    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Message Received: {message}");
};

// with auto ack
// channel.BasicConsume(queue: "letterbox", autoAck: true, consumer: consumer);
// without auto ack
channel.BasicConsume(queue: "letterbox", consumer: consumer);


Console.ReadKey();

