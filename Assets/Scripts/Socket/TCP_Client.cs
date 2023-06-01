using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

[Serializable]
public class RecData{
    public float speed;
    public float cadence;
    public float angle;
}

public class TCP_Client : MonoBehaviour
{
    private Thread receiveThread;
    public static int conn_state = 0; //pie+unity are connected,conn_state=1
    private TcpClient client;
    private NetworkStream stream;
    private byte[] receiveBuffer = new byte[1024];
    private bool isRunning = true;
    public String processPath;
    // private bool isPath = false;
    private PlayerController player;

    private void Awake() {
        Debug.Log("Socket Awake");
        IPAddress localIpAddress = IPAddress.Parse("127.0.0.1");    //192.168.100.166
        client = new TcpClient();
        client.Client.Bind(new IPEndPoint(localIpAddress, 14786));
        client.Connect("127.0.0.1", 30000);
        
        player = GameObject.FindObjectOfType<PlayerController>();
        processPath = null;
    }

    private void Start()
    {
        Debug.Log("Socket Start");
        receiveThread = new Thread(SocketClientThread);
        receiveThread.Start();
        // sendThread = new Thread(SendData);
        // sendThread.Start();
        // IPAddress localIpAddress = IPAddress.Parse("192.168.100.166");
        // client = new TcpClient();
        // client.Client.Bind(new IPEndPoint(localIpAddress, 14786));
        // client.Connect("192.168.100.166", 30000);
        // stream = client.GetStream();

        // videoController = GameObject.FindObjectOfType<VideoController>();
        // player = GameObject.FindObjectOfType<PlayerController>();
        // StartReceiving();
    }

    // private void Update()
    // {
    //     Debug.Log("Socket update");
    //     // Send data to the server every second
    //     if (Time.time >= 1f)
    //     {
    //         if (isPath)
    //         {
    //             SendData(videoController.videoFilePath);
    //             isPath = false;
    //         }
    //         else
    //         {
    //             SendData("path: " + isPath);
    //             // Debug.Log("Socket log");
    //         }
    //         Time.timeScale = 0f; // Pause the game after sending the data
    //         StartReceiving();
    //     }
    // }

    void SocketClientThread(){
        stream = client.GetStream();
        // StartReceiving();
        int bytesRead;
        string receivedData;
        while (isRunning)
        {
            // 接收數據
            // StartReceiving();
            SendData();
            bytesRead = stream.Read(receiveBuffer, 0, receiveBuffer.Length);
            receivedData = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);
            //check connection state
            Debug.Log("receivedData: " + receivedData);

            if(receivedData=="OK"){
                conn_state=1;
                Debug.Log("conn_state: " + conn_state);
            }
            
            ProcessRecData(receivedData);
            // int bytesRead = stream.Read(receiveBuffer, 0, receiveBuffer.Length);
            // if(bytesRead > 0){
            //     string receivedData = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);
            //     ProcessRecData(receivedData);
            // }
            // stream.Read(receiveBuffer, 0, receiveBuffer.Length);
        }
        stream.Close();
        client.Close();
    }

/* 非同步socket
    private void StartReceiving()
    {
        // Asynchronously receive data from the server
        stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            int bytesRead = stream.EndRead(result);

            Debug.Log("bytesRead: "+bytesRead);

            if (bytesRead > 0)
            {
                string receivedData = Encoding.ASCII.GetString(receiveBuffer, 0, bytesRead);             
                Debug.Log(receivedData);
                
                    // Use the received data in the main Unity thread
                ProcessRecData(receivedData);
                    // ProcessReceivedData(receivedMessage);
                
                
                // float.Parse(receivedData)
                // SendData("Received data.");
                //check connection state
                if(receivedData=="OK"){
                    conn_state=1;
                    Debug.Log("conn_state: " + conn_state);
                }
                //
                StartReceiving();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error receiving data! "+ e.Message);
        }
    }
*/
    private void ProcessRecData(string jsonData)
    {
        try
        {
            // Deserialize the JSON data into a C# object
            RecData dataObject = JsonUtility.FromJson<RecData>(jsonData);
            
            // Access the values from the deserialized object
            float speed = dataObject.speed;
            float cadence = dataObject.cadence;
            float angle = dataObject.angle;

            // Process or use the received data as needed
            Debug.Log("Return Speed: " + speed);
            Debug.Log("Return Cadence: " + cadence);
            Debug.Log("Return Angle: " + angle);
            player.getSensorData(speed, cadence, angle);
        }
        catch (Exception)
        {
            Debug.Log("Received msg is not Json format: " + jsonData);
        }
    }

    private void SendData()
    {
        byte[] sendData;
        if(processPath == null){
            sendData = Encoding.ASCII.GetBytes("");
        }
        else{
            sendData = Encoding.ASCII.GetBytes(processPath);
            processPath = null;
        }

        try
        {
            stream.Write(sendData, 0, sendData.Length);
            stream.Flush();
            // Debug.Log("Sent data: " + msg);
        }
        catch (Exception e)
        {
            Debug.LogError("Error sending data: " + e.Message);
        }
    }

    private void OnDestroy()
    {
        print("Destroy..");
        isRunning = false;
        receiveThread.Abort();
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Join();
        }
    }
    private void OnApplicationQuit(){
        isRunning = false;
        receiveThread.Abort();
        if (client != null)
        {
            stream.Close();
            client.Close();
        }
    }
}
