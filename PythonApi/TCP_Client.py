import numpy
import cv2
import socket
import json
import time
import lane_line_detect as lld

TCP_IP = '127.0.0.1'
TCP_PORT = 30000

def TestTcpConnect():
    try:
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        sock.connect((TCP_IP, TCP_PORT))

        print("Testing...")
        sock.sendall('Connect Check'.encode('utf-8'))
        recdata = sock.recv(1024)
        print("data: ", recdata.decode())
        if recdata.decode() == "Server check":
            print("Server OK")
            sock.close()
            return True
        else:
            print("Server Error")
            sock.close()
            return False
    except Exception as e:
        print("Function TestTcpConnect: ", e)


if __name__ == '__main__':
    resolutionRatio = 30
    testpoint = [[0,0],[50,50],[100,100],[200,200]]
    if TestTcpConnect():
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        sock.connect((TCP_IP, TCP_PORT))
        while True:
            senddata = {'testdata': testpoint}
            data = json.dumps(senddata)
            
            print("go")
            sock.send(bytes(data, encoding='utf-8'))
            print("receive")
            recdata = sock.recv(1024)
            print(recdata)
            print(int.from_bytes(recdata, byteorder='little'))
            # print("data: ", recdata.decode("utf-8"))
        sock.close()
            
        # cap = cv2.VideoCapture('D:/Banana/coding/Unity/video_Trim.mp4')
        # params = [cv2.IMWRITE_JPEG_QUALITY, resolutionRatio]
        # print(cap.get(cv2.CAP_PROP_FPS))
        # framelist = []
        # while cap.isOpened():
        #     ret, img = cap.read()
        #     if not ret:
        #         break
        #     img = lld.pipeline(img)
        #     img = cv2.resize(img,(1280,720))
        #     # framelist.append(img)
        #     # img_data = {'image':cv2.imencode('.jpg', img, params)[1].ravel().tolist()}# 'image':cv2.imencode('.jpg', img, params)[1].ravel().tolist(),'testdata':index
        #     img_data = {'testdata':testpoint}# 'image':cv2.imencode('.jpg', img, params)[1].ravel().tolist(),'testdata':index
        #     data = json.dumps(img_data)
        #     print(len(data))
        #     sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        #     sock.connect((TCP_IP, TCP_PORT))
            
        #     print("go")
        #     sock.send(bytes(data, encoding='utf-8'))

        #     print("receive")
        #     recdata = sock.recv(1024)
        #     if recdata.decode("utf-8") != "ACK":
        #         print("data: ", recdata.decode("utf-8"))
        #     sock.close()
        # print("done")
        # cap.release()

# print(len(framelist))
# for i in framelist:
#     cv2.imshow("Image", i)
#     time.sleep(1/60)
#     cv2.waitKey(1)
# cv2.destroyAllWindows()
