using System;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// for PubSub, Publisher/Producer need to declare a Exchange
channel.ExchangeDeclare(exchange: "pubsub", type: ExchangeType.Fanout);

// For PubSub, Producer/Publisher dont have to declare a queue
// channel.QueueDeclare(
//     queue: "letterbox",
//     durable: false,
//     exclusive: false,
//     autoDelete: false,
//     arguments: null);

var message = "Hello, I want to broadcast this message";
var body = Encoding.UTF8.GetBytes(message);

// Publish to Exchange
channel.BasicPublish(exchange: "pubsub", "", null, body);

Console.WriteLine($"Send Message: {message}");

