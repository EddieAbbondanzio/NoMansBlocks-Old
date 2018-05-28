# LiteNetLib 

Lite reliable UDP library for .NET, Mono, .NET Core, .NET Standart, and UWP.
Minimal .NET version - 3.5

## Build

### [NuGet](https://www.nuget.org/packages/LiteNetLib/) [![NuGet](https://img.shields.io/nuget/v/LiteNetLib.svg)](https://www.nuget.org/packages/LiteNetLib/) [![NuGet](https://img.shields.io/nuget/dt/LiteNetLib.svg)](https://www.nuget.org/packages/LiteNetLib/)

### [Release builds](https://github.com/RevenantX/LiteNetLib/releases) [![GitHub (pre-)release](https://img.shields.io/github/release/RevenantX/LiteNetLib/all.svg)](https://github.com/RevenantX/LiteNetLib/releases)

### [DLL build from master](https://ci.appveyor.com/project/RevenantX/litenetlib/branch/master/artifacts) [![](https://ci.appveyor.com/api/projects/status/354501wnvxs8kuh3/branch/master?svg=true)](https://ci.appveyor.com/project/RevenantX/litenetlib/branch/master)
( Warning! Master branch can be unstable! )

### Donations are welcome and will help further development of this project.
[![Bountysource](https://img.shields.io/badge/bountysource-donate-green.svg)](https://salt.bountysource.com/checkout/amount?team=litenetlib)

## Features

* Lightweight
  * Small CPU and RAM usage
  * Small packet size overhead ( 1 byte for unrealiable, 3 bytes for reliable packets )
* Simple connection handling
* Peer to peer connections
* Helper classes for sending and reading messages
* Different send mechanics
  * Reliable with order
  * Reliable without order
  * Ordered but unreliable with duplication prevention
  * Simple UDP packets without order and reliability
* Fast packet serializer [(Usage manual)](https://github.com/RevenantX/LiteNetLib/wiki/NetSerializer-usage)
* Automatic small packets merging ( if enabled )
* Automatic fragmentation of reliable packets
* Automatic MTU detection
* UDP NAT hole punching
* NTP time requests
* Packet loss and latency simulation
* IPv6 support (dual mode)
* Connection statisitcs (need DEBUG or STATS_ENABLED flag)
* Multicasting (for discovering hosts in local network)
* Unity3d support
* Supported platforms:
  * Windows/Mac/Linux (.net framework, Mono, .net core)
  * Android (unity3d)
  * iOS (unity3d)
  * Universal Windows (Windows 8.1 and Windows 10 including phones)

## Unity3d notes!!!
* Always use library sources insted of precompiled DLL files. 
* Add to define symbols UNITY.

## Usage samples

### Server
```csharp
EventBasedNetListener listener = new EventBasedNetListener();
NetManager server = new NetManager(listener, 2 /* maximum clients */, "SomeConnectionKey");
server.Start(9050 /* port */);

listener.PeerConnectedEvent += peer =>
{
    Console.WriteLine("We got connection: {0}", peer.EndPoint); // Show peer ip
    NetDataWriter writer = new NetDataWriter();                 // Create writer class
    writer.Put("Hello client!");                                // Put some string
    peer.Send(writer, SendOptions.ReliableOrdered);             // Send with reliability
};

while (!Console.KeyAvailable)
{
    server.PollEvents();
    Thread.Sleep(15);
}

server.Stop();
```
### Client
```csharp
EventBasedNetListener listener = new EventBasedNetListener();
NetManager client = new NetManager(listener, "SomeConnectionKey");
client.Start();
client.Connect("localhost" /* host ip or name */, 9050 /* port */);
listener.NetworkReceiveEvent += (fromPeer, dataReader) =>
{
    Console.WriteLine("We got: {0}", dataReader.GetString(100 /* max length of string */));
};

while (!Console.KeyAvailable)
{
    client.PollEvents();
    Thread.Sleep(15);
}

client.Stop();
```

## NetManager settings description

* **UnconnectedMessagesEnabled**
  * enable messages receiving without connection. (with SendUnconnectedMessage method)
  * default value: **false**
* **NatPunchEnabled**
  * enable nat punch messages
  * default value: **false**
* **UpdateTime**
  * library logic update (and send) period in milliseconds
  * default value: **100 msec**. For games you can use 15 msec (66 ticks per second)
* **PingInterval**
  * Interval for latency detection and checking connection
  * default value: **1000 msec**.
* **DisconnectTimeout**
  * if client or server doesn't receive any packet from remote peer during this time then connection will be closed
  * (including library internal keepalive packets)
  * default value: **5000 msec**.
* **SimulatePacketLoss**
  * simulate packet loss by dropping random amout of packets. (Works only in DEBUG mode)
  * default value: **false**
* **SimulateLatency**
  * simulate latency by holding packets for random time. (Works only in DEBUG mode)
  * default value: **false**
* **SimulationPacketLossChance**
  * chance of packet loss when simulation enabled. value in percents.
  * default value: **10 (%)**
* **SimulationMinLatency**
  * minimum simulated latency
  * default value: **30 msec**
* **SimulationMaxLatency**
  * maximum simulated latency
  * default value: **100 msec**
* **DiscoveryEnabled**
  * Allows receive DiscoveryRequests
  * default value: **false**
* **MergeEnabled**
  * Merge small packets into one before sending to reduce outgoing packets count. (May increase a bit outgoing data size)
  * default value: **false**
* **ReconnectDelay**
  * delay betwen connection attempts
  * default value: **500 msec**
* **MaxConnectAttempts**
  * maximum connection attempts before client stops and call disconnect event.
  * default value: **10**
* **UnsyncedEvents**
  * Experimental feature. Events automatically will be called without PollEvents method from another thread
  * default value: **false**
