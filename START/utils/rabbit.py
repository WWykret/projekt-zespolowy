import time

while True:
    time.sleep(1)
    with open("utils\\mq.txt") as file:
        for line in file:
            if "broker... completed" in line:
                exit(0)
            elif "could not bind to distribution port 25672" in line:
                exit(0)
