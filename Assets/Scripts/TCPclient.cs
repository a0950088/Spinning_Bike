using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class TCPclient : MonoBehaviour
{
	private VideoController videoController;

    private Socket clientSocket;
    private byte[] receiveBuffer = new byte[1024];
    private byte[] sendBuffer = new byte[1024];

    private void Start()
    {
    	videoController = GameObject.FindObjectOfType<VideoController>();
        // Create a TCP/IP socket
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // Connect to the server
        string serverIP = "127.0.0.1"; // Replace with your server's IP address
        int serverPort = 30000; // Replace with your server's port number
        IPAddress serverAddress = IPAddress.Parse(serverIP);
        IPEndPoint serverEndPoint = new IPEndPoint(serverAddress, serverPort);

        clientSocket.Connect(serverEndPoint);
        
        sendBuffer = System.Text.Encoding.UTF8.GetBytes(videoController.videoFilePath);
        clientSocket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, OnSend, null);

        // Start receiving data from the server
        clientSocket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, OnReceive, null);
    }

    private void OnReceive(IAsyncResult result)
    {
        // Receive data from the server
        int bytesRead = clientSocket.EndReceive(result);
        string receivedData = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);
        Debug.Log("Received data: " + receivedData);
    }

    private void OnSend(IAsyncResult result)
    {
    	try
        {
            int bytesSent = clientSocket.EndSend(result);
            Debug.Log("Sent " + bytesSent + " bytes to the server.");

            // 關閉Socket
            clientSocket.Close();
        }
        catch (Exception e)
        {
            Debug.LogError("Error while sending data: " + e.Message);
        }
    }

    private void OnDestroy()
    {
        // Close the socket when the script is destroyed
        clientSocket.Close();
    }
}