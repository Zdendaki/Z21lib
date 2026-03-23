# Z21lib

A modern, asynchronous, and high-performance .NET library for communication with Roco/Fleischmann Z21 digital command stations.

The library fully implements the **Z21 LAN Protocol ([Firmware V1.43](https://www.z21.eu/media/Kwc_Basic_DownloadTag_Component/root-en-main_47-1652-959-downloadTag-download/default/d559b9cf/1699290380/z21-lan-protokoll-en.pdf))** specification and is designed with an emphasis on maximum throughput and minimum memory footprint.

## Features

* **Modern .NET:** Native support for **.NET 8, 9, and 10**.
* **High Performance:** Utilizes `ReadOnlySpan<byte>` and `ArrayPool<T>` to eliminate unnecessary allocations (zero-allocation message building).
* **Asynchronous I/O:** Non-blocking network communication over UDP using `ReceiveAsync` and asynchronous channels (`Channel<T>`).
* **Multi-instance:** Supports running multiple applications (clients) simultaneously on a single PC thanks to dynamic local port allocation.
* **Thread Safety:** Fully thread-safe message sending and processing.
* **Modern Resource Disposal:** Implements `IAsyncDisposable` for safe and clean connection termination.

## Installation

The library is available as a NuGet package.

```bash
dotnet add package Z21lib
```

## Basic usage

Example of connecting to the command station, subscribing to events, and sending basic commands:

```csharp
using Z21lib;
using Z21lib.Structs;
using Z21lib.Messages;

// Set the IP address and port of the command station (default port is 21105)
var info = new Z21Info("192.168.0.111", 21105);

// Using 'await using' ensures asynchronous and safe disconnection (IAsyncDisposable)
await using var client = new Z21Client(info);

// Subscribe to events
client.MessageReceived += message =>
{
    if (message is SerialNumberMessage serialMsg)
    {
        Console.WriteLine($"Command station serial number: {serialMsg.SerialNumber}");
    }
    else
    {
        Console.WriteLine($"Received message: {message.MessageType}");
    }
};

// Connect to the command station
if (client.Connect())
{
    Console.WriteLine("Successfully connected to Z21.");

    // Send a request for the serial number
    client.LanGetSerialNumber();

    // Turn on track power
    client.LanXSetTrackPower(true);

    // Wait for incoming messages (simulating application run)
    await Task.Delay(5000);

    // Disconnect
    await client.DisconnectAsync();
}
else
{
    Console.WriteLine("Connection failed.");
}
```

## Running Multiple Applications on a Single PC

By default, the library requests a dynamic local port from the operating system. This means you can run several applications using `Z21lib` on the same computer, and they will all communicate seamlessly with the same command station.

If you need to strictly bind the local port due to network configuration or firewall rules, pass the `useFixedLocalPort: true` parameter to the constructor:

```csharp
var client = new Z21Client(info, useFixedLocalPort: true);
```

## Advanced Commands

The library includes methods for all standard operations according to the specification, such as:

- Locomotive control (driving, functions)

- Reading and writing CV registers (POM and programming track)

- Controlling turnouts and accessories

- Processing RailCom data

- Forwarding messages from LocoNet and CAN bus

Additional examples can be found in the `TestClient` test project within the repository.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.