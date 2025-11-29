# Network Scanner

Sleek Windows Forms utility for fast LAN sweeps. Pick an interface, set a host range, and get a responsive host list with round-trip times and optional DNS lookups. Built for my software dev portfolio.

<img width="902" height="719" alt="image" src="https://github.com/user-attachments/assets/927c5e92-4935-466a-8c6e-3ff613fbfa51" />


## Highlights
- Ping a custom host range (default 1-254) with cancellable progress
- Optional hostname resolution (DNS + NetBIOS + multicast fallback) to keep scans useful even when PTR records are missing
- Adjustable timeout for noisy networks
- Export responsive hosts to CSV in one click
- Designed for Windows with .NET 9 and modern WinForms styling

## Quick Start

### Prerequisites
- Windows 10/11 with .NET 9 SDK installed
- ICMP allowed by your firewall for ping requests

### Build with .NET SDK
```bash
dotnet restore
dotnet build "Network Scanner.sln"
```

### Build with Visual Studio
1. Open `Network Scanner.sln` in Visual Studio 2022 or later.
2. Choose Debug or Release.
3. Build the solution (`Build > Build Solution`).

### Run
```bash
dotnet run --project "Network Scanner/Network Scanner.csproj"
```
The executable also appears at `Network Scanner/bin/<Configuration>/net9.0-windows/` after a build.

## Using the App
- Select a network interface to auto-fill the base IPv4 address.
- Set **Start** and **End** host numbers to narrow the scan window.
- Toggle **Resolve hosts** and adjust **Timeout (ms)** to balance speed vs accuracy.
- Click **Start** to scan or **Cancel** to stop midway; watch progress and live stats update.
- Export results to CSV once hosts are found.

## Project Map
- `Network Scanner.sln` — solution entry point
- `Network Scanner/` — WinForms UI (`Form1.*`) and app entry (`Program.cs`)
- `Network Scanner/bin/<Configuration>/net9.0-windows/` — build outputs

## Notes for Reviewers
- Manual test ideas: scan a small range (e.g., `192.168.1.1-20`), try canceling mid-run, toggle hostname resolution, and export results.

## License
MIT License. See `LICENSE` for details.
