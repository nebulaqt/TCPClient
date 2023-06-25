using System.Threading;
using TcpClient.Utilities;

// Start the Heartbeat thread as a background thread
new Thread(Heartbeat.Run) { IsBackground = true }.Start();

// Keep the main thread alive so that the application doesn't exit immediately
while (true) Thread.Sleep(1000);