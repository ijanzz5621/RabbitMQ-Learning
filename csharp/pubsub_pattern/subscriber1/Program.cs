﻿using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// Declare Exchange
channel.ExchangeDeclare(exchange: "pubsub", type: ExchangeType.Fanout);

// Use temporary Queue
// No need for arguments pass
var queueName = channel.QueueDeclare().QueueName;

var consumer = new EventingBasicConsumer(channel);

// If don't bind, it will not listen for a message from the Queue
channel.QueueBind(queue: queueName, exchange: "pubsub", routingKey: "");

consumer.Received += (model, ea) => {
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Subscriber 1: Received new message: {message}");
};

channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

Console.WriteLine("Consuming...");

Console.ReadKey();