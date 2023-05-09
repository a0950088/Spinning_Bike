using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

// // Add json key from client data
// public class Data{
//     // public byte[] image;
//     public int[,] testdata;
// }

public class TCPserver : MonoBehaviour
{
    private VideoController videocontroller;
    private TcpListener server;
    private bool isRunning;

    public int Port = 30000;

    private void Start()
    {
        print("TCP server start");
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

        server = new TcpListener(ipAddress, Port);
        server.Start();

        isRunning = true;

        videocontroller = GameObject.Find("Screen").GetComponent<VideoController>();

        StartListening();
    }

    private void FixedUpdate(){
        StartListening();
    }

    private void StartListening()
    {
        server.BeginAcceptTcpClient(HandleClientAccepted, server);
    }

    private void HandleClientAccepted(IAsyncResult result)
    {
        TcpClient client = server.EndAcceptTcpClient(result);

        UnityEngine.Debug.Log("Client connected");

        NetworkStream stream = client.GetStream();

        while (isRunning)
        {
            print("Running...");

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            UnityEngine.Debug.Log("Received message: " + message);
            if (message == "Connect Check")
            {
                byte[] check = Encoding.ASCII.GetBytes("Server check");
                stream.Write(check, 0, check.Length);
                UnityEngine.Debug.Log("Sent check: Server check");
                continue;
            }
            
            // byte[] response = BitConverter.GetBytes(videocontroller.videoFilePath);
            byte[] response = Encoding.ASCII.GetBytes(videocontroller.videoFilePath);
            stream.Write(response, 0, response.Length);
        }
    }

    private void OnApplicationQuit()
    {
        UnityEngine.Debug.Log("isRunning: false");
        isRunning = false;

        if (server != null)
        {
            print("TCP server stop");
            server.Stop();
        }
    }
}