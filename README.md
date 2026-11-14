<h1 align="center">SerialChecker</h1>

<p align="center">
  Every hardware ID and network address on your PC, in one clean console scan.
</p>

<p align="center">
  <a href="https://github.com/slizsy/SerialChecker/releases/latest"><img alt="Latest release" src="https://img.shields.io/github/v/release/slizsy/SerialChecker?color=00b4d8"></a>
  <img alt="Platform: Windows 10 and 11" src="https://img.shields.io/badge/platform-Windows%2010%20%7C%2011-0078d4">
  <img alt=".NET 8" src="https://img.shields.io/badge/.NET-8-512bd4">
</p>

<p align="center">
  <img width="520" alt="SerialChecker scanning a PC" src="https://github.com/user-attachments/assets/f0e8f991-d81a-4813-af67-19f0ca8d06ff">
</p>

SerialChecker is a small Windows console tool, written in C# on .NET 8, that reads the serial numbers and identifiers of your own machine and prints them as one readable report. It is handy for warranty claims, inventory, checking a second-hand PC, or confirming that TPM and Secure Boot are on.

## Download

Grab `SerialChecker.exe` from the [latest release](https://github.com/slizsy/SerialChecker/releases/latest) and run it. It is a single self-contained file, so there is nothing to install and no .NET runtime needed.

Windows asks for administrator access when it starts. The TPM and Secure Boot checks only return full data when elevated.

## What it shows

| Section | Details |
|---|---|
| **Windows version** | OS name, version, build number |
| **Display** | Resolution and refresh rate |
| **Disks** | Model, serial number, size |
| **Volumes** | Drive letters and labels |
| **Motherboard** | Manufacturer, product, serial number |
| **BIOS** | Manufacturer, version, serial number |
| **SMBIOS UUID** | The system's unique ID |
| **CPU** | Name and processor ID |
| **Memory (RAM)** | Capacity, manufacturer and part number per stick |
| **GPU** | Name and driver version |
| **MAC addresses** | One per network interface, with its status |
| **TPM** | Present, activated, owned |
| **Secure Boot** | Enabled or disabled |
| **Local IP** | IPv4 addresses on this machine |
| **Public IP** | Your internet-facing IPv4 address |

## Privacy

Everything is read locally, through WMI and the Windows registry, and printed to the console. Nothing is saved to disk or uploaded.

The one network request is the public IP lookup, a plain call to [api.ipify.org](https://www.ipify.org). If you are offline or it is blocked, that section shows an error and the rest of the scan is unaffected.

## Build from source

You need Windows 10 or 11 and the [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).

```powershell
git clone https://github.com/slizsy/SerialChecker.git
cd SerialChecker
dotnet publish -c Release
```

The single-file build lands in `bin\Release\net8.0-windows\win-x64\publish\`.

## Project layout

| File | Purpose |
|---|---|
| `Program.cs` | Entry point; runs each section of the scan in order |
| `HardwareInfo.cs` | Queries for disks, board, BIOS, CPU, RAM, GPU, TPM, Secure Boot, display |
| `NetworkInfo.cs` | MAC addresses, local IP and public IP |
| `ConsoleHelper.cs` | Coloured headers, tables and key/value rows |
| `app.manifest` | Requests administrator access on launch |

---

<p align="center">Made by <a href="https://github.com/slizsy">@slizsy</a></p>
