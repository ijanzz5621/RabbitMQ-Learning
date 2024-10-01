import pika
from pika.exchange_type import ExchangeType

EXCHANGE_NAME = 'pubsub'

connection_parameters = pika.ConnectionParameters('localhost')
connection = pika.BlockingConnection(connection_parameters)

channel = connection.channel()

channel.exchange_declare(exchange=EXCHANGE_NAME, exchange_type=ExchangeType.fanout)

message = "Hello I want to broadcast this message: Second message"

channel.basic_publish(exchange=EXCHANGE_NAME, routing_key='', body=message)

print(f"sent message: {message}")

connection.close()

