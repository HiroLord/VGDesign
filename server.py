# This file runs the websockets.

import string, cgi, time

from wsserver import *

from time import sleep

games = {}

class Game:

    def __init__(self, hostCode):
        self.hostCode = hostCode
        self.players = []
        self.playerID = 0
        self.accepting = True

    def addPlayer(self, client):
        if not self.accepting:
            return False
        self.players.append(client)
        self.playerID += 1
        client.pID = self.playerID
        return True

class Client:

    def __init__(self, socket, gID):
        self.socket = socket
        self.gID = gID
        self.pID = 0

        self.x = 0
        self.y = 0
        self.confirmed = False
        self.host = None
    
    def handle(self):
        #if (self.socket.canHandleMsg() == False):
        #    return
        if self.socket.socket == None:
            self.disconnect()
            return
        while self.canHandle():
            msgID = self.socket.readByte()
            if (msgID == 1):
                hostCode = ""
                for _ in range(4):
                    hostCode += str(self.socket.readByte())
                print("Host code:", hostCode);
                self.host = games[int(hostCode)]
                if (self.host.addPlayer(self)):
                    self.confirm()
                else:
                    self.deny()
            elif (msgID == 2):
                # Player position
                self.x = self.socket.readFloat();
                self.y = self.socket.readFloat();
                for client in self.host.players:
                    if client.pID != self.pID and client.confirmed:
                        client.socket.writeByte(2)
                        client.socket.writeByte(self.pID)
                        client.socket.writeFloat(self.x)
                        client.socket.writeFloat(self.y)
            elif (msgID == 3):
                self.h = self.socket.readFloat();
                self.v = self.socket.readFloat();
                self.shoot = self.socket.readByte();
                for client in self.host.players:
                    if client.pID != self.pID and client.confirmed:
                        client.socket.writeByte(3)
                        client.socket.writeByte(self.pID)
                        client.socket.writeFloat(self.h)
                        client.socket.writeFloat(self.v)
                        client.socket.writeByte(self.shoot)

            elif (msgID == 4):
                self.rot = self.socket.readFloat()
                for client in self.host.players:
                    if (client.pID != self.pID and client.confirmed):
                        client.socket.writeByte(4)
                        client.socket.writeByte(self.pID)
                        client.socket.writeFloat(self.rot)

            elif (msgID == 5):
                enemyID = self.socket.readByte()
                enemyEState = self.socket.readByte()
                for client in self.host.players:
                    if (client.pID != self.pID and client.confirmed):
                        client.socket.writeByte(5)
                        client.socket.writeByte(enemyID)
                        client.socket.writeByte(enemyEState)

            elif (msgID == 6):
                enemyHID = self.socket.readByte()
                enemyDHealth = self.socket.readFloat()
                for client in self.host.players:
                    if (client.pID != self.pID and client.confirmed):
                        client.socket.writeByte(6)
                        client.socket.writeByte(enemyHID)
                        client.socket.writeFloat(enemyDHealth)

            elif (msgID == 7):
                enemyPID = self.socket.readByte()
                enemyX = self.socket.readFloat()
                enemyZ = self.socket.readFloat()
                for client in self.host.players:
                    if (client.pID != self.pID and client.confirmed):
                        client.socket.writeByte(7)
                        client.socket.writeByte(enemyPID)
                        client.socket.writeFloat(enemyX)
                        client.socket.writeFloat(enemyZ)

            elif (msgID == 8):
                revID = self.socket.readByte()
                for client in self.host.players:
                    if (client.pID != self.pID and client.confirmed):
                        client.socket.writeByte(8)
                        client.socket.writeByte(revID)

            elif (msgID == 9):
                enemyAID = self.socket.readByte()
                enemyTarget = self.socket.readByte()
                for client in self.host.players:
                    if (client.pID != self.pID and client.confirmed):
                        client.socket.writeByte(9)
                        client.socket.writeByte(enemyAID)
                        client.socket.writeByte(enemyTarget)

            elif (msgID == 11):
                for client in self.host.players:
                    if (client.pID != self.pID and client.confirmed):
                        client.socket.writeByte(11)



    def canHandle(self):
        if (self.socket.hasData() == False):
            return False
        msgID = self.socket.peekByte()
        size = 256
        if (msgID == 1):
            size = 5
        elif(msgID == 2):
            size = 9
        elif(msgID == 3):
            size = 10
        elif(msgID == 4):
            size = 4
        elif(msgID == 5):
            size = 2
        elif(msgID == 6):
            size = 5
        elif(msgID == 7):
            size = 9
        elif(msgID == 8):
            size = 1
        elif(msgID == 9):
            size = 2
        elif(msgID == 11):
            size = 0
        else:
            print("MSG id", msgID, "does not exist.")
        if size <= len(self.socket.data):
            print("Parsing MsgID", msgID)
            return True
        print("Waiting to parse MsgID", msgID)
        return False

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

        self.confirmed = True

    def deny(self):
        self.socket.writeByte(1)
        self.socket.writeByte(0)
        self.socket.socket.close()

    def disconnect(self):
        print("Removing client.")
        clients.remove(self)
        self.socket = None
        if self.host:
            self.host.players.remove(self)
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
    try:
        server = startServer(25001)

        games[9218]= Game(9218)

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
