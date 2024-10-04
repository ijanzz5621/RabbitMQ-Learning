import pika
import time
import random

from pika.credentials import PlainCredentials

# create connection
connection_parameters = pika.ConnectionParameters('192.168.0.101', 5672, "general_vhost", credentials=PlainCredentials("general", "p@ssw0rd!"))
connection = pika.BlockingConnection(connection_parameters)

# create channel
channel = connection.channel()
channel.queue_declare(queue='general')

messageId = 1

# infinite loop
while(True):
    # message to send
    message = f"Sending messageId: {messageId}"

    # publish message 
    channel.basic_publish(exchange='', routing_key='general', body=message)

    # print
    print(f"sent message: {message}")
    
    time.sleep(random.randint(1, 4))
    
    messageId+=1


