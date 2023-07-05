from bluepy.btle import Scanner, Peripheral, DefaultDelegate
import keyboard

DEVICE_MAC = 'd7:4d:55:c4:c3:5c'
MEASUREMENT_CHAR_UUID = '00002a5b-0000-1000-8000-00805f9b34fb'
class BLEData(DefaultDelegate):
    def __init__(self):
        DefaultDelegate.__init__(self)
        self.path = 'output.txt'
        self.sensordata = [] # wheelRevolution,lastWheelTime,crankRevolution,lastCrankEventTime
        self.indices = [0,2,10,14,18] # 16bits indices => 03 01000000 90e40100 2de5
        self.startflag = False
        self.speed = 0
        self.cadence = 0
    
    def compute(self, data):
        newstr = ''
        ret = []
        for i in range(1,len(data)):
            for j in range(len(data[i])-2,-1,-2):
                newstr += (data[i][j]+data[i][j+1])
            ret.append(int(newstr,16))
            newstr = ''
        return ret
    
    def handleNotification(self, cHandle, data):
        #global sensordata, startflag, indices
        f=open(self.path,'a')
        f.write(data.hex()+"\n")
        print(f"Recevied notification on handle {cHandle}: {data.hex()}")
        parts = [str(data.hex())[i:j] for i,j in zip(self.indices, self.indices[1:]+[None])]

        if self.startflag==False:
            self.sensordata = self.compute(parts)
            self.startflag=True
        else:
            nowsensordata = self.compute(parts)
            if (nowsensordata[1]-self.sensordata[1]) <= 0:
                speed = 0
            else:
                speed = ((abs(nowsensordata[0]-self.sensordata[0])*2.1)/((nowsensordata[1]-self.sensordata[1])/1000))*3.6
            if (nowsensordata[3]-self.sensordata[3]) <= 0:
                cadence = 0
            else:
                cadence = (abs(nowsensordata[2]-self.sensordata[2])/((nowsensordata[3]-self.sensordata[3])/1000))*60
            self.speed = speed
            self.cadence = cadence
            #print("speed: ",speed)
            #print("cadence: ",cadence)
            self.sensordata = nowsensordata
            f.write("speed: "+str(speed)+"\n")
            f.write("cadence: "+str(cadence)+"\n")
        f.close()
        
def connectDevice():
    '''
    Connect to BLE sensor
    Return: peripheral device
    '''
    try:
        device = Peripheral(DEVICE_MAC, 'random')
        print(f"Connect success on {device.addr}")
        return device
    except:
        print("Connect error, try again...")
        return connectDevice()

def getCharacteristic(device):
    '''
    Get BLE sensor's characteristic
    Return: peripheral device
    '''
    try:
        characteristic = device.getCharacteristics(uuid=MEASUREMENT_CHAR_UUID)[0]
        device.writeCharacteristic(characteristic.valHandle+1, b"\x01\x00")
        print("Get characteristic success.")
        return device
    except:
        print("Get characteristic failed, try again...")
        return getCharacteristic(device)

def setNotificationDelegate(device):
    '''
    Set BLE sensor's delegate
    Return: peripheral device
    '''
    try:
        bleData = BLEData()
        device.setDelegate(bleData)
        print("Set NotificationDelegate success.")
        return device, bleData
    except:
        print("Set NotificationDelegate failed, try again...")
        return setNotificationDelegate(device)

def initializeSensor():
    '''
    Initialize function
    '''
    print("BLE connecting...")
    device = connectDevice()
    device = getCharacteristic(device)
    device, bleData = setNotificationDelegate(device)
    print("Service OK!")
    return device, bleData
    
def getSensorData(device):
    '''
    Main function
    Call this function to send notification and get updated sensor data
    '''
    #while True: #Play Video end
    if device.waitForNotifications(3.0):
        #Send data to unity
        #print(bleData.speed)
        #print(bleData.cadence)
        if keyboard.is_pressed('q'):
            device.disconnect()
            return False
        return True
    else:
        device.disconnect()
        return False
    
