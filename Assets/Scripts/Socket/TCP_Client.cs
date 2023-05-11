using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class TCP_Client : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private byte[] receiveBuffer = new byte[1024];

    private VideoController videoController;
    private bool isPath = false;

    private void Start()
    {
        IPAddress localIpAddress = IPAddress.Parse("127.0.0.1");
        client = new TcpClient();
        client.Client.Bind(new IPEndPoint(localIpAddress, 14786));
        client.Connect("127.0.0.1", 30000);
        stream = client.GetStream();

        videoController = GameObject.FindObjectOfType<VideoController>();

        StartReceiving();
    }

    private void Update()
    {
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
                SendData("ready to receive");
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
                Debug.Log("Received data: " + receivedData);
                // SendData("Received data.");

                StartReceiving();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error receiving data: " + e.Message);
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