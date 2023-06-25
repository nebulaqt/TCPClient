using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpClient.Utilities;

public static class Heartbeat
{
    private const string ServerIpAddress = "127.0.0.1";
    private const int ServerPort = 1252;
    private const string KeepAliveMessage = "keep alive";
    private const string AcknowledgementMessage = "ack";
    private const int KeepAliveIntervalMilliseconds = 1000;
    private const int NoResponseTimeoutMilliseconds = 10000;

    public static void Run()
    {
        using var client = new System.Net.Sockets.TcpClient();
        try
        {
            // Connect to the server
            client.Connect(ServerIpAddress, ServerPort);
            // Enable keep-alive functionality on the client socket
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            // Get the network stream to read and write data
            using var stream = client.GetStream();
            var keepAliveMessageBytes = Encoding.ASCII.GetBytes(KeepAliveMessage);
            var responseBuffer = new byte[4];

            while (true)
            {
                // Send the keep-alive message to the server
                stream.Write(keepAliveMessageBytes, 0, keepAliveMessageBytes.Length);

                // Read the response from the server
                if (!ReadResponse(stream, responseBuffer, out var responseMessage))
                {
                    // Handle no response from the server
                    HandleNoResponse();
                    break;
                }

                // Check if the response is the expected acknowledgement message
                if (responseMessage.Trim() != AcknowledgementMessage)
                {
                    // Handle invalid response from the server
                    HandleInvalidResponse();
                    break;
                }

                // Update the last response time
                var lastResponseTime = DateTime.UtcNow;
                // Sleep for the specified interval before sending the next keep-alive message
                Thread.Sleep(KeepAliveIntervalMilliseconds);

                // Check for no response after the timeout period
                if (DateTime.UtcNow - lastResponseTime <= TimeSpan.FromMilliseconds(NoResponseTimeoutMilliseconds))
                    continue;
                // Handle no response within the timeout period
                HandleNoResponse();
                break;
            }
        }
        catch (Exception)
        {
            // Handle connection error
            HandleConnectionError();
        }
    }

    private static bool ReadResponse(Stream stream, byte[] buffer, out string responseMessage)
    {
        // Read bytes from the stream into the buffer
        var bytesRead = stream.Read(buffer, 0, buffer.Length);

        if (bytesRead > 0)
        {
            // Convert the received bytes to a string
            responseMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            return true;
        }

        // No bytes read, empty response
        responseMessage = string.Empty;
        return false;
    }

    private static void HandleNoResponse()
    {
        // Exit the application or handle the scenario as needed
        Console.WriteLine("No response from the server. Exiting the application.");
        Environment.Exit(0);
    }

    private static void HandleInvalidResponse()
    {
        // Exit the application or handle the scenario as needed
        Console.WriteLine("Invalid response from the server. Exiting the application.");
        Environment.Exit(0);
    }

    private static void HandleConnectionError()
    {
        // Exit the application or handle the scenario as needed
        Console.WriteLine("Connection error. Exiting the application.");
        Environment.Exit(0);
    }
}