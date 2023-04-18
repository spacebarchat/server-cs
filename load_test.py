#!/usr/bin/env python

# pip install websocket-client

import json
import math
import random
import sys
import threading
import time

import websocket

BASE_URL = "ws://localhost:2001/?v=9&encoding=json"
TOKEN = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiMTA5NjYyNzM5ODQyMzYwOTM0NCIsIm5iZiI6MTY4MTUyNjc0OCwiZXhwIjoxNjg0MTE4NzQ4LCJpYXQiOjE2ODE1MjY3NDh9.u9Gmz-SiOmB2rrcMt1zWpCLMM6O2kCphOeuXAOvTASg"

class ConnectionThread(threading.Thread):
    def __init__(self, name):
        threading.Thread.__init__(self)
        self.name = name
        self.heartbeat_interval = 0
        self.heartbeater = None

    def send(self, wsapp: websocket.WebSocketApp, data):
        wsapp.send(json.dumps(data))

    def send_heartbeat(self, wsapp: websocket.WebSocketApp):
        print(f"[{self.name}] Sending heartbeat")
        self.send(wsapp, {"op": 1, "d": None})
        self.heartbeater = threading.Timer(self.heartbeat_interval, self.send_heartbeat, args=(wsapp,))
        self.heartbeater.start()

    def on_open(self, wsapp: websocket.WebSocketApp):
        print(f"[{self.name}] Connected")

    def on_close(self, wsapp: websocket.WebSocketApp, code, reason):
        print(f"[{self.name}] Disconnected with code {code}")

    def on_error(self, wsapp: websocket.WebSocketApp, error):
        print(f"[{self.name}] Error: {error}")

    def on_message(self, wsapp: websocket.WebSocketApp, message):
        #print(f"[{self.name}] Message: {message}")
        data = json.loads(message)
        op = data["op"]
        if op == 10:
            self.heartbeat_interval = data["d"]["heartbeat_interval"] / 1000
            print(f"[{self.name}] Heartbeat interval: {self.heartbeat_interval}")
            self.heartbeater = threading.Timer(self.heartbeat_interval * random.randrange(0, 1), self.send_heartbeat, args=(wsapp,))
            self.heartbeater.start()

            # send identify
            print(f"[{self.name}] Hello, sending identify")
            payload = {"op": 2, "d": {"token": TOKEN, "properties": {"$os": "linux", "$browser": "load_test", "$device": "load_test"}}}

            self.send(wsapp, payload)

        elif op == 11:
            print(f"[{self.name}] Heartbeat ack")
        elif op == 1:
            self.send_heartbeat(wsapp)
        elif op == 0:
            event = data["t"]
            if event == "READY":
                print(f"[{self.name}] Ready")
            else:
                print(f"[{self.name}] Event: {event}")

    def on_pong(self, wsapp: websocket.WebSocketApp, data):
        print(f"[{self.name}] Pong: {data}")

    def on_ping(self, wsapp: websocket.WebSocketApp, data):
        print(f"[{self.name}] Ping: {data}")

    def run(self) -> None:
        print(f"[{self.name}] Starting")
        ws = websocket.WebSocketApp(BASE_URL, on_message=self.on_message, on_close=self.on_close, on_error=self.on_error, on_open=self.on_open, on_pong=self.on_pong, on_ping=self.on_ping)
        ws.run_forever()


if __name__ == "__main__":
    thread_count = int(sys.argv[1]) if len(sys.argv) > 1 else 1
    threads: threading.Thread = []

    for i in range(thread_count):
        thread = ConnectionThread(f"Thread-{i+1}")
        threads.append(thread)
        thread.start()
        time.sleep(0.15)

    # kill all threads on control c, and close when all threads are dead
    try:
        while True:
            time.sleep(1)
            dead = True
            for thread in threads:
                if thread.is_alive():
                    dead = False
                    break
            if dead:
                break
    except KeyboardInterrupt:
        for thread in threads:
            thread.heartbeater.cancel()
            thread.join()
        print("All threads killed")

    print("Done")
