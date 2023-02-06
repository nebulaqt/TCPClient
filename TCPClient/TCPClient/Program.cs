using System;
using System.Net.Sockets;
using System.Text;

namespace TCPClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Loop infinitely to keep the connection alive
            while (true)
            {
                try
                {
                    using (TcpClient client = new TcpClient("TCP SERVER IP", 1252))
                    {
                        using (NetworkStream stream = client.GetStream())
                        {
                            // Prepare and send the "keep alive" message as a byte array
                            byte[] message = Encoding.ASCII.GetBytes("keep alive");
                            stream.Write(message, 0, message.Length);

                            // Set the timeout for reading the response from the server
                            int timeout = 1000; // timeout in milliseconds
                            stream.ReadTimeout = timeout;

                            // Read the response from the server into a byte array
                            byte[] response = new byte[4]; // Change the size to the minimum required for "ack" message
                            int bytesRead = stream.Read(response, 0, response.Length);

                            // Convert the response byte array into a string
                            string responseMessage = Encoding.ASCII.GetString(response, 0, bytesRead);

                            // Check if the response message is "ack"
                            if (responseMessage.Trim() != "ack")
                            {
                                Console.WriteLine("Received invalid response: " + responseMessage);
                                Environment.Exit(0);
                            }

                            // Print the received response
                            Console.WriteLine("Received response: " + responseMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Print the error message if connection to the server fails
                    Console.WriteLine("Failed to connect to server: " + ex.Message);
                    Environment.Exit(0);
                }

                // Wait for 1 second before attempting to connect again
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
