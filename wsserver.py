import string, cgi, time
import socket
import base64
import hashlib
import select
import random
import struct
from time import sleep
from os import curdir, sep
from http.server import BaseHTTPRequestHandler, HTTPServer

_outMsgStructs = {}
_inMsgStructs = {}
_sockets = []
_clients = []
cID = 0
_server = None

def createMsgStruct(msgID, outgoing):
    newMsg = MsgStruct(msgID)
    if outgoing:
        _outMsgStructs[msgID] = newMsg
    else:
        _inMsgStructs[msgID] = newMsg
    return newMsg


class MsgStruct:

    def __init__(self, msgID):
        self.msgID = msgID
        self.numParts = 0
        self.nextPart = 0
        self.parts = {}
        self.sizes = {}

    def reset(self):
        self.data = extend(self.msgID, 3)
        self.nextPart = 0
        return self

    def size(self):
        return len(self.data)

    def canHandle(self, data):
        part = 1
        ind = 3
        while part <= self.numParts:
            mType = self.parts[part]
            mLen = self.sizes[part]
            if len(data[ind:]) < mLen:
                return False
            if (mType == "C"):
                ind += mLen
            elif (mType == "S"):
                size = int(data[ind:ind+mLen])
                ind += mLen
                if (size > len(data[ind:])):
                    return False
                ind += size
            part += 1
        return True

    def read(self):
        self.nextPart += 1
        mType = self.parts[self.nextPart]
        mLen = self.sizes[self.nextPart]
        out = ''
        if (mType == "C"):
            out = self.data[0:mLen]
        elif (mType == "S"):
            size = int(self.data[0:mLen])
            out = self.data[mLen:mLen+size]
            mLen += size
        self.data = self.data[mLen:]
        return out

    def readInt(self):
        return int(self.read())

    def fillFromData(self, data):
        part = 1
        ind = 3
        while part <= self.numParts:
            mType = self.parts[part]
            mLen = self.sizes[part]
            if mType == "C":
                ind += mLen
            elif mType == "S":
                size = int(data[ind:ind+mLen])
                ind += mLen
                ind += size
            part += 1
        self.data = data[3:ind]
        self.nextPart = 0
        data = data[ind:]
        return self

    def write(self, data):
        self.nextPart += 1
        mType = self.parts[self.nextPart]
        mLen = self.sizes[self.nextPart]
        if mType == "C":
            dataS = str(data)
            if len(dataS) > mLen:
                print("Incorrect MSG write size:", dataS, "| max size", mLen)
                return
            dataS = extend(dataS, mLen)
            self.data += dataS
        elif mType == "S":
            dataS = str(data)
            sLen = len(dataS)
            dataS = extend(sLen, mLen) + dataS
            self.data += dataS
        return self

    def readyToSend(self):
        return self.nextPart == self.numParts

    def getData(self):
        return self.data

    def addChars(self, numChars):
        self.numParts += 1
        self.parts[self.numParts] = "C"
        self.sizes[self.numParts] = numChars
        return self

    def addString(self):
        self.numParts += 1
        self.parts[self.numParts] = "S"
        self.sizes[self.numParts] = 3
        return self

def extend(n, l):
    n = str(n)
    while len(n) < l:
        n = "0" + n
    return n

class Socket:

    def __init__(self, socket, webSocket, cID):
        self.socket = socket
        self.webSocket = webSocket
        self.cID = cID
        self.data = []
        self.hostCode = ""

        self.packet = None

    def newPacket(self, msgID):
        self.packet = _outMsgStructs[msgID].reset()

    def write(self, data):
        self.packet.write(data)

    def read(self):
        self.packet.read(data)

    def readPacket(self):
        msgID = int(self.data[0:3])
        packet = _inMsgStructs[msgID].fillFromData(self.data)
        self.data = self.data[3+packet.size():]
        return packet

    def send(self, data=None):
        if (data == None):
            data = self.packet.data
        length = len(data)
        if self.webSocket:
            ret = bytearray([129, length])
        else:
            ret = bytearray([])
        for byte in data.encode("utf-8"):
            ret.append(byte)
        self.socket.send(ret)

    def writeByte(self, byte):
        self.socket.send(bytearray([byte]))

    def writeString(self, string):
        ret = bytearray([len(string)])
        for byte in string.encode("utf-8"):
            ret.append(byte)
        self.socket.send(ret)

    def writeFloat(self, flo):
        self.socket.send(struct.pack('f',flo))

    def peekByte(self):
        return self.data[0]

    def readByte(self):
        byte = self.data[0]
        self.data = self.data[1:]
        return byte

    def readString(self):
        l = self.readByte()
        string = self.data[0:l]
        self.data = self.data[l:]
        return string.decode('utf-8')

    def readFloat(self):
        flo = self.data[0:4]
        self.data = self.data[4:]
        return struct.unpack('f', bytearray(flo))[0]

    def hasData(self):
        return len(self.data) > 0

    def canHandleMsg(self):
        if len(self.data) < 3:
            return False
        rawMsgID = self.data[0:3]
        msgID = int(rawMsgID)
        if msgID not in _inMsgStructs:
            print(_inMsgStructs)
            print("Invalid MsgID", rawMsgID)
            return False
        return _inMsgStructs[msgID].canHandle(self.data)

    def getMsgSize(self, msgID):
        if (msgID == 0):
            return 3
        elif (msgID == 1):
            return 3
        elif (msgID == 2):
            return 5
        elif (msgID == 3):
            return 3
        elif (msgID == 5):
            s = 0
            if len(self.data) >= 6:
                s = int(self.data[3:6])
            return 3 + 3 + s
        elif (msgID == 10):
            return 3 + 4
        else:
            print("No message size for msgID", msgID)
        return 3

    def recv(self):
        try:
            data = bytearray(self.socket.recv(4096))
        except:
            self.disconnect()
            return False
        if (len(data) == 0):
            self.disconnect()
            return False
        self.parseData(data)
        return True
        
    def parseData(self, data):
        if (len(data) < 1):
            return
        #print("New data:", data)
        self.data.extend(data);
        #print("Add data:", self.data)

    def disconnect(self):
        print("Lost client.")
        _sockets.remove(self.socket)
        _clients.remove(self)
        self.playing = False
        self.socket = None
        return


def acceptClient(s):
    global cID
    global _sockets
    print("Accepting client...")
    rec = s.recv(4096)
    print("Handshake:", rec)
    
    s.send(bytearray([254]))
    webSocket = False

    _sockets.append(s)
    client = Socket(s, webSocket, cID)
    _clients.append(client)
    print("Client accepted. Websocket", webSocket)
    cID += 1
    return client

def handleNetwork():
    global _server
    clientWaiting, _, _ = select.select([_server], [], [], 0)
    if (len(clientWaiting) > 0):
            (clientsocket, address) = _server.accept()
            print("New connection from", address)
            return acceptClient(clientsocket)
    clientsReady, _, _ = select.select(_sockets, [], [], 0)
    for client in clientsReady:
        for c in _clients:
            if c.socket == client:
                c.recv()
    return None

def startServer(port=8886):
    global _server
    _server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    _server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    _server.bind(('', port))
    _server.listen(5)
    print("Websocket server has been started.")
    print("Listening on port " + str(port) + "...")
    return _server
