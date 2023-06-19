using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPClient
{
    internal static class Program
    {
        private static void Main()
        {
            using (var client = new System.Net.Sockets.TcpClient())
            {
                try
                {
                    client.Connect("127.0.0.1", 1252); // Connect to the server at IP address 127.0.0.1 and port 1252

                    // Enable TCP keep-alive on the client socket
                    client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                    using (var stream = client.GetStream()) // Get the network stream for reading and writing data
                    {
                        var keepAliveMessage = Encoding.ASCII.GetBytes("keep alive"); // Convert the keep-alive message to bytes
                        var response = new byte[4]; // Buffer to hold the response message

                        while (true)
                        {
                            stream.Write(keepAliveMessage, 0, keepAliveMessage.Length); // Send the keep-alive message to the server

                            var bytesRead = stream.Read(response, 0, response.Length); // Read the response from the server
                            var responseMessage = Encoding.ASCII.GetString(response, 0, bytesRead); // Convert the response bytes to a string

                            if (responseMessage.Trim() != "ack") // Check if the response is not "ack"
                            {
                                Console.WriteLine("Received invalid response: " + responseMessage);
                                break;
                            }

                            Console.WriteLine("Received response: " + responseMessage);
                            Thread.Sleep(1000); // Sleep for 1 second before sending the next keep-alive message
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to connect to server: " + ex.Message);
                }
            }
        }
    }
}
