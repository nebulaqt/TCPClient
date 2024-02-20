# TcpClient Heartbeat Utility

This utility provides a simple TCP client implementation with heartbeat functionality to maintain a connection with a server. It sends a keep-alive message to the server at regular intervals and expects an acknowledgement response to ensure the connection is active.

## Usage

1. Ensure that you have the required .NET environment installed.
2. Add the `Heartbeat.cs` file to your project.
3. Call the `Run()` method from your code to start the heartbeat functionality.

```csharp
using TcpClient.Utilities;

class Program
{
    static void Main(string[] args)
    {
        Heartbeat.Run();
    }
}
```
## Configuration

- **ServerIpAddress:** IP address of the server to connect to.
- **ServerPort:** Port number of the server.
- **KeepAliveMessage:** Message to send as a keep-alive signal to the server.
- **AcknowledgementMessage:** Expected acknowledgement message from the server.
- **KeepAliveIntervalMilliseconds:** Interval between consecutive keep-alive messages.
- **NoResponseTimeoutMilliseconds:** Timeout duration for waiting for a response from the server.

## Error Handling

- If no response is received from the server within the timeout period, the application exits.
- If an invalid response is received from the server, the application exits.
- If there's a connection error, the application exits.

## Dependencies

- .NET Standard Library

