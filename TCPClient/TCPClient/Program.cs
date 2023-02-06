using System;
using System.Net.Sockets;
using System.Text;

namespace TCPClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    // Connect to the server
                    TcpClient client = new TcpClient("IP OF TCP Server", 1252);

                    // Get the client's network stream
                    NetworkStream stream = client.GetStream();

                    // Send the "keep alive" message
                    byte[] message = Encoding.ASCII.GetBytes("keep alive");
                    stream.Write(message, 0, message.Length);

                    // Read the response from the server with a timeout
                    int timeout = 1000; // timeout in milliseconds
                    stream.ReadTimeout = timeout;
                    byte[] response = new byte[1024];
                    int bytesRead = stream.Read(response, 0, response.Length);
                    string responseMessage = Encoding.ASCII.GetString(response, 0, bytesRead);

                    // Check if the response is "ack"
                    if (responseMessage.Trim() != "ack")
                    {
                        Console.WriteLine("Received invalid response: " + responseMessage);
                        Environment.Exit(0);
                    }

                    // Handle the response as needed
                    Console.WriteLine("Received response: " + responseMessage);

                    // Close the client
                    client.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to connect to server: " + ex.Message);
                    Environment.Exit(0);
                }

                // Wait before attempting to connect again
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}

