import pika

# create connection
connection_parameters = pika.ConnectionParameters('localhost', 5672)
connection = pika.BlockingConnection(connection_parameters)

# create channel
channel = connection.channel()
channel.queue_declare(queue='letterbox')

# callback function
def on_message_received(channel, method, properties, body):
    print(f"received new message: {body}")

channel.basic_consume(queue='letterbox', 
                      auto_ack=True, 
                      on_message_callback=on_message_received)

print("Start Consuming...")
channel.start_consuming()

connection.close()
