import pika
import time
import random

import pika.channel as pikaChannel
import pika.spec as pikaSpec

# create connection
connection_parameters = pika.ConnectionParameters('localhost', 5672)
connection = pika.BlockingConnection(connection_parameters)

# create channel
channel = connection.channel()
channel.queue_declare(queue='letterbox')

# quality of service
# process a single message at a time
# if still in progress, The queue will wait and nothing will be pushed to this consumer
# if this is not being set, it will use round robin mechanism to receive message from queue
# if there are 2 consumers, consumer1 recieve: 1,3,5,7,9, consumer2 receive: 2,4,6,8,10
channel.basic_qos(prefetch_count=1)

# callback function
def on_message_received(channel: pikaChannel.Channel, 
                        method: pikaSpec.Basic.Deliver, 
                        properties: pikaSpec.BasicProperties, 
                        body: bytes):
    processing_time = random.randint(1, 6)
    print(f"received: {body}, will take {processing_time} second(s) to process")
    time.sleep(processing_time)
    # acknowledege after processing completed
    # and ready to receive new queue item
    print(f"Item {body} has been processed. Waiting for next item...")
    channel.basic_ack(delivery_tag=method.delivery_tag)

# channel.basic_consume(queue='letterbox', 
#                       auto_ack=True, # to acknowledge immedieatly to the broker. The next item will be queue evene the processing still in progress 
#                       on_message_callback=on_message_received)
channel.basic_consume(queue='letterbox', 
                      on_message_callback=on_message_received)

print("Start Consuming...")
channel.start_consuming()

connection.close()
