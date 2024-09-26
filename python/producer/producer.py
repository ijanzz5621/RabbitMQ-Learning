import pika
import time
import random

# create connection
connection_parameters = pika.ConnectionParameters('localhost', 5672)
connection = pika.BlockingConnection(connection_parameters)

# create channel
channel = connection.channel()
channel.queue_declare(queue='letterbox')

messageId = 1

# infinite loop
while(True):
    # message to send
    message = f"Sending messageId: {messageId}"

    # publish message 
    channel.basic_publish(exchange='', routing_key='letterbox', body=message)

    # print
    print(f"sent message: {message}")
    
    time.sleep(random.randint(1, 4))
    
    messageId+=1


