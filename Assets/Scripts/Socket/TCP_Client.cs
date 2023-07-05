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
    private static TCP_Client instance;
    private Thread receiveThread;
    private static TcpClient client;
    private NetworkStream stream;
    private bool isRunning = true;
    public int conn_state = 0;
    private byte[] receiveBuffer = new byte[1024];
    public String processPath;
    private PlayerController player;

    public float speed;
    public float cadence;
    public float angle;

    public static TCP_Client Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            IPAddress localIpAddress = IPAddress.Parse("192.168.100.145");
            client = new TcpClient();
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            // client.Client.Bind(new IPEndPoint(localIpAddress, 14786));
            client.Connect("192.168.100.145", 30000);
        }
        
        player = GameObject.FindObjectOfType<PlayerController>();
        processPath = null;

        speed = 0.0f;
        cadence = 0.0f;
        speed = 0.0f;
    }

    private void Start()
    {
        receiveThread = new Thread(SocketClientThread);
        receiveThread.Start();
    }

    private void OnApplicationQuit()
    {
        DisconnectFromServer();
        isRunning = false;
        AbortReceiveThread();
        Debug.Log("OnApplicationQuit");
    }

    private void SocketClientThread()
    {
        try{
            stream = client.GetStream();
            int bytesRead;
            string receivedData;
            while (isRunning)
            {
                SendData();
                bytesRead = stream.Read(receiveBuffer, 0, receiveBuffer.Length);
                receivedData = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);

                if (receivedData == "OK")
                {
                    conn_state = 1;
                    Debug.Log("conn_state: " + conn_state);
                }
                else if(receivedData == "Respberry Pi Connect Failed"){
                    conn_state = 0;
                }
                else{
                    ProcessRecData(receivedData);
                }
            }
        }
        finally{
            // DisconnectFromServer();
            Debug.Log("ReceiveThread Close");
        }
       
    }

    private void ProcessRecData(string jsonData)
    {
        try
        {
            RecData dataObject = JsonUtility.FromJson<RecData>(jsonData);
            // float speed = dataObject.speed;
            // float cadence = dataObject.cadence;
            // float angle = dataObject.angle;
            speed = dataObject.speed;
            cadence = dataObject.cadence;
            angle = dataObject.angle;

            Debug.Log("Return Speed: " + speed);
            Debug.Log("Return Cadence: " + cadence);
            Debug.Log("Return Angle: " + angle);
            // player.getSensorData(speed, cadence, angle);
        }
        catch (Exception)
        {
            Debug.Log("Received msg is not Json format: " + jsonData);
        }
    }

    private void SendData(string message = "")
    {
        byte[] sendData;
        if (processPath == null)
        {
            sendData = Encoding.ASCII.GetBytes(message);
        }
        else
        {
            sendData = Encoding.ASCII.GetBytes(processPath);
            processPath = null;
        }

        try
        {
            stream.Write(sendData, 0, sendData.Length);
            stream.Flush();
        }
        catch (Exception e)
        {
            Debug.LogError("Error sending data: " + e.Message);
        }
    }

    private void DisconnectFromServer()
    {
        if (client != null && client.Connected)
        {
            SendData("Disconnect");
            client.Client.Shutdown(SocketShutdown.Both);
            client.Client.Close();
            client.Close();
            client = null;
        }
    }

    private void AbortReceiveThread()
    {
        if (receiveThread != null && receiveThread.IsAlive)
        {
            stream.Close();
            receiveThread.Abort();
            receiveThread.Join();
        }
    }

}
