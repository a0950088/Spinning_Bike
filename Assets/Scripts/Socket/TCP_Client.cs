using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

[Serializable]
public class RecData{
    public float speed;
    public float cadence;
    public float angle;
}

public class TCP_Client : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private byte[] receiveBuffer = new byte[1024];

    private VideoController videoController;
    private bool isPath = false;
    private PlayerController player;

    private void Start()
    {
        Debug.Log("Socket Start");
        IPAddress localIpAddress = IPAddress.Parse("192.168.100.166");
        client = new TcpClient();
        client.Client.Bind(new IPEndPoint(localIpAddress, 14786));
        client.Connect("192.168.100.166", 30000);
        stream = client.GetStream();

        videoController = GameObject.FindObjectOfType<VideoController>();
        player = GameObject.FindObjectOfType<PlayerController>();
        StartReceiving();
    }

    private void Update()
    {
        Debug.Log("Socket update");
        // Send data to the server every second
        if (Time.time >= 1f)
        {
            if (isPath)
            {
                SendData(videoController.videoFilePath);
                isPath = false;
            }
            else
            {
                SendData("path: " + isPath);
                // Debug.Log("Socket log");
            }
            Time.timeScale = 0f; // Pause the game after sending the data
            StartReceiving();
        }
    }

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

            if (bytesRead > 0)
            {
                string receivedData = Encoding.ASCII.GetString(receiveBuffer, 0, bytesRead);             
                ProcessRecData(receivedData);
                
                // float.Parse(receivedData)
                // SendData("Received data.");
                StartReceiving();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error receiving data: " + e.Message);
        }
    }

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
            Debug.Log("Speed: " + speed);
            Debug.Log("Cadence: " + cadence);
            Debug.Log("Angle: " + angle);
            player.getSensorData(speed, cadence, angle);
        }
        catch (Exception)
        {
            Debug.Log("Received msg is not Json format: " + jsonData);
        }
    }

    private void SendData(string data)
    {
        byte[] sendData = Encoding.ASCII.GetBytes(data);

        try
        {
            stream.Write(sendData, 0, sendData.Length);
            stream.Flush();
            Debug.Log("Sent data: " + data);
        }
        catch (Exception e)
        {
            Debug.LogError("Error sending data: " + e.Message);
        }
    }

    private void OnDestroy()
    {
        print("Destroy..");
        if (client != null)
        {
            stream.Close();
            client.Close();
        }
    }
}