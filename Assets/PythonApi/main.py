from BLE_GetSensorData import initializeSensor, getSensorData
from resistance import initializeResistance, getAngle
# from mpu6050_SensorData import initializeMPU6050, getAngle, calibration
import socket
import time
import json
import threading
import argparse

'''
# mpu 6050
def mpu():
    global angle
    bus = initializeMPU6050()
    gyro_error = calibration(bus)
    while True:
        start = time.time()
        angle = getAngle(bus, gyro_error)
        end = time.time()

thread_mpu = threading.Thread(target = mpu)
thread_mpu.start()
'''

def getParser():
    parser = argparse.ArgumentParser(description = "ip setting")
    parser.add_argument('-i','--host',default='192.168.100.145',type=str)
    parser.add_argument('-p','--port',default=30000,type=int)
    return parser

def Resistance():
    global angle
    bus = initializeResistance()
    while True:
        angle = getAngle(bus)
        # print(angle)

parser = getParser()
args = parser.parse_args()
SERVER_HOST = args.host
SERVER_PORT = args.port

thread_resistance = threading.Thread(target = Resistance)
thread_resistance.start()
device, bleData = initializeSensor()

data = {
    'speed': 0.0,
    'cadence': 0.0,
    'angle': 0.0,
}
angle = 0

connected = False
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

while True:
    while not connected:
        try:
            s.connect((SERVER_HOST, SERVER_PORT))
            connected = True
        except Exception as e:
            time.sleep(0.2)
    try:            
        print("connected")
        while True:
            if getSensorData(device):
                data['speed'] = bleData.speed
                data['cadence'] = bleData.cadence
                data['angle'] = angle
                json_data = json.dumps(data)
                print("Angle: ", angle)        
                print("Speed: ", bleData.speed)
                print("Cadence: ", bleData.cadence)
                print("----------------------")
                s.send(json_data.encode())
            else:
                # reconnect ble
                device, bleData = initializeSensor()
    except socket.error as e:
        print("Disconnected...")
        connected = False
        s.close()
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

print("Disconnected...")
s.close()
