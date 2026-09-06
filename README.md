# SerialChecker

A Windows console tool that pulls together key hardware and network 
identifiers from your own PC in one clean, readable scan — built with 
C# and .NET 8.

## What it shows
- **Disks** — model, serial number, size
- **Volumes** — drive letters and labels
- **Motherboard** — manufacturer, product, serial number
- **BIOS** — manufacturer, version, serial number
- **SMBIOS UUID**
- **CPU** — name and processor ID
- **RAM** — capacity, manufacturer, part number per stick
- **GPU** — name and driver version
- **MAC Addresses** — per network interface
- **TPM** — presence, activation, ownership status
- **Secure Boot** — enabled/disabled
- **Windows Version** — OS name, version, build number
- **Display** — resolution and refresh rate
- **Local & Public IP**

## Requirements
- Windows 10/11
- .NET 8 SDK (for building)

## Building
Open `HwidChecker.csproj` in Visual Studio and Publish, or run:
