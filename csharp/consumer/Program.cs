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

// Set the QOS
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

var consumer = new EventingBasicConsumer(channel);

var random = new Random();

consumer.Received += (model, ea) => {

    var processingTime = random.Next(1, 6);

    var body = ea.Body.ToArray();
    
    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Received: '{message}' will take {processingTime} seconds to process");
    Task.Delay(TimeSpan.FromSeconds(processingTime)).Wait();

    // Ack after processing complete.
    // If the processing took longer time to complete, it will send to other consumers to process
    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
};

// with auto ack
// channel.BasicConsume(queue: "letterbox", autoAck: true, consumer: consumer);
// without auto ack
channel.BasicConsume(queue: "letterbox", consumer: consumer);


Console.ReadKey();

