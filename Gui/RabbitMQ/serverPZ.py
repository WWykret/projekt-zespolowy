import pika
import json

# data = "PRS:"

daa = ["Joe", "pack://application:,,,/GuiPz;component/Data/Images/Avatars/Nfd.png", "wiedzmin",
       "pack://application:,,,/GuiPz;component/Data/Images/Avatars/Nfd.png"]


def get_profiles():
    return daa


def handler(message, ch, properties):
    code = message[:3]
    rest = message[4:]

    if code == "GET":
        get_handler(rest, ch, properties)


def get_handler(message, ch, properties):
    code = message[:3]
    rest = message[4:]

    if code == "PRS":
        profiles_sender(ch, properties)


def profiles_sender(ch, properties):
    code = "PRS:"
    data = json.dumps(get_profiles())
    ch.basic_publish('', routing_key=properties.reply_to, body=code + data)


def on_request_message_received(ch, method, properties, body: bytes):
    print(f"Received Request")
    handler(body.decode(), ch, properties)


def server():
    lolz = json.dumps(daa)

    print(lolz)
    print(daa)

    connection_parameters = pika.ConnectionParameters('localhost')

    connection = pika.BlockingConnection(connection_parameters)

    channel = connection.channel()

    channel.queue_declare(queue='request-queue', auto_delete=True)

    channel.basic_consume(queue='request-queue', auto_ack=True,
                          on_message_callback=on_request_message_received)

    print("Starting Server")

    channel.start_consuming()
