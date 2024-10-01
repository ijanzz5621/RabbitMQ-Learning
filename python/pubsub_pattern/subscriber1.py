import pika
from pika.exchange_type import ExchangeType
import pika.channel as pikaChannel
import pika.spec as pikaSpec

EXCHANGE_NAME = 'pubsub'

def on_message_received(
    ch: pikaChannel.Channel, 
    method: pikaSpec.Basic.Deliver, 
    properties: pikaSpec.BasicProperties, 
    body: bytes):
    print(f"first consumer: received new message: {body}")
    
connection_parameters = pika.ConnectionParameters('localhost')
connection = pika.BlockingConnection(connection_parameters)

channel = connection.channel()

channel.exchange_declare(exchange=EXCHANGE_NAME, exchange_type= ExchangeType.fanout)

# random name from server
queue = channel.queue_declare(queue='', exclusive=True)

channel.queue_bind(exchange=EXCHANGE_NAME, queue=queue.method.queue)

channel.basic_consume(queue=queue.method.queue, auto_ack=True, on_message_callback=on_message_received)

print("First Subscriber: Starting consuming")

channel.start_consuming()