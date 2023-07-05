import smbus
import time

address = 0x48
A0 = 0x40

angle_max = 45.0
angle_min = -45.0
angle_mid = 0.0

elapsedTime = 0.1

# params
voltage_max = 105 # 80
voltage_min = 5 # 5
voltage_mid = 70 # 55

def initializeResistance():
    bus = smbus.SMBus(1)
    bus.write_byte(address, A0)
    return bus    

def getAngle(bus):
    voltage = bus.read_byte(address)
    #print("voltage:", voltage)
    if voltage < voltage_mid:
        angle = (voltage - voltage_min) * (angle_mid - angle_min) / (voltage_mid - voltage_min) + angle_min
    elif voltage > voltage_mid:
        angle = (voltage - voltage_mid) * (angle_max - angle_mid) / (voltage_max - voltage_mid) + angle_mid
    else:
        angle = 0       
    # angle = (voltage - voltage_min) * (angle_max - angle_min) / (voltage_max - voltage_min) + angle_min
    angle = round(angle, 1)
    
    if angle <= 10 and angle >=-10:
        angle = 0.0
    
    time.sleep(elapsedTime)
    return angle

'''
while True:
    bus = initializeResistance()
    angle = getAngle(bus)
    print(angle)
'''