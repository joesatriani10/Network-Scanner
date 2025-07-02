# Network Scanner

Network Scanner is a simple Windows Forms application that scans your local network by sending ping requests to IP addresses in the selected interface's subnet. Active hosts and their response times are displayed in a table.

## Building

The project targets **.NET 9.0 for Windows**. You can build it with the .NET SDK or with Visual Studio 2022 or later.

### Using the .NET SDK

```bash
# Restore NuGet packages
 dotnet restore

# Build the application
 dotnet build "Network Scanner.sln"
```

### Using Visual Studio

1. Open `Network Scanner.sln` in Visual Studio.
2. Choose the desired configuration (Debug or Release).
3. Build the solution (`Build > Build Solution`).

## Running

After building, the executable can be found in `Network Scanner/bin/<Configuration>/net9.0-windows/`.

You can also run the application directly with the .NET CLI:

```bash
 dotnet run --project "Network Scanner/Network Scanner.csproj"
```

Select a network interface from the drop-down, verify the starting IP address, and click **Scan** to discover hosts on your network. The **Scan** button changes to **Cancel** while scanning so you can stop the operation early.
