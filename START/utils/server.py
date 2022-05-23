import time
from os.path import exists

while True:
    time.sleep(1)
    if exists("utils\\server.txt"):
        with open("utils\\server.txt") as file:
            for line in file:
                if "Starting Server" in line:
                    exit(0)

