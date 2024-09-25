import pika

# create connection
connection_parameters = pika.ConnectionParameters('localhost', 5672)
connection = pika.BlockingConnection(connection_parameters)

# create channel
channel = connection.channel()
channel.queue_declare(queue='letterbox')

# message to send
message = "Hello. This is my sixth message"

# publish message 
channel.basic_publish(exchange='', routing_key='letterbox', body=message)

# print
print(f"sent message: {message}")

# close connection
connection.close()

