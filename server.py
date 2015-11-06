# This file runs the websockets.

import string, cgi, time

from wsserver import *

from time import sleep

def setupMessages():
    m1 = createMsgStruct(1, False)
    m1.addString()

    i1 = createMsgStruct(1, True)
    i1.addChars(2)

class Client:

    def __init__(self, socket, pID):
        self.socket = socket
        self.pID = pID

        self.x = 0;
        self.y = 0;
    
    def handle(self):
        global pID
        #if (self.socket.canHandleMsg() == False):
        #    return
        if (self.socket.hasData()):
            msgID = self.socket.readByte()
            if (msgID == 1):
                self.confirm()
            elif (msgID == 2):
                # Player position
                self.x = self.readFloat();
                self.y = self.readFloat();
                for client in clients:
                    if client.pID != self.pID:
                        client.socket.writeByte(2)
                        client.socket.writeFloat(self.x)
                        client.socket.writeFloat(self.y)

    # This is called to confirm to the client that they have been accepted,
    # after they send us their details.
    def confirm(self):
        self.socket.writeByte(1)
        self.socket.writeByte(self.pID)
        #self.socket.sendMessage()

        for client in clients:
            if (client.pID == self.pID):
                continue
            self.socket.writeByte(10)
            self.socket.writeByte(client.pID)
            self.socket.writeFloat(client.x)
            self.socket.writeFloat(client.y)
            #self.socket.sendMessage()
            client.socket.writeByte(10)
            client.socket.writeByte(self.pID)
            client.socket.writeFloat(self.x)
            client.socket.writeFloat(self.y)
            #client.socket.sendMessage()

    def disconnect(self):
        print("Lost client.")
        clients.remove(self)
        self.socket = None
        return

# This handles a new client.
# We need to hand them to an object
# so that we can read and write from it
def handle(socket):
    global pID, clients
    pID += 1
    client = Client(socket, pID)
    clients.append(client)

def main():
    global gameStarted
    global stage
    try:
        setupMessages()
        server = startServer(25001)
        while True:
            newClient = handleNetwork()
            if newClient:
                handle(newClient)
            for client in clients:
                client.handle()
            sleep(0.01)
    except KeyboardInterrupt:
        print(' received, closing server.')
        server.close()
    
clients = []
pID = 0

if __name__ == '__main__':
    main()
