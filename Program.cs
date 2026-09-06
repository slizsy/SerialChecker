using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HwidChecker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "SerialChecker"; 
            var stopwatch = Stopwatch.StartNew();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"   _____ __    _________  _______  __");
            Console.WriteLine(@"  / ___// /   /  _/__  / / ___/\ \/ /");
            Console.WriteLine(@"  \__ \/ /    / /   / /  \__ \  \  / ");
            Console.WriteLine(@" ___/ / /____/ /   / /_____/ /  / /  ");
            Console.WriteLine(@"/____/_____/___/  /____/____/  /_/   ");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("Checker made by @slizsy");
            Console.WriteLine();

            System.Threading.Thread.Sleep(2000); // pause before loading, adjust as needed

            ConsoleHelper.PrintInfo("Scanning local system...");

            ConsoleHelper.PrintHeader("Windows Version");
            HardwareInfo.PrintWindowsVersion();

            ConsoleHelper.PrintHeader("Display");
            HardwareInfo.PrintDisplay();

            ConsoleHelper.PrintHeader("Disks");
            HardwareInfo.PrintDisks();

            ConsoleHelper.PrintHeader("Volumes");
            HardwareInfo.PrintVolumes();

            ConsoleHelper.PrintHeader("Motherboard");
            HardwareInfo.PrintWmi(
                "Win32_BaseBoard",
                new[] { "Manufacturer", "Product", "SerialNumber" },
                new[] { "Manufacturer", "Product", "Serial Number" });

            ConsoleHelper.PrintHeader("BIOS");
            HardwareInfo.PrintWmi(
                "Win32_BIOS",
                new[] { "Manufacturer", "SMBIOSBIOSVersion", "SerialNumber" },
                new[] { "Manufacturer", "Version", "Serial Number" });

            ConsoleHelper.PrintHeader("SMBIOS UUID");
            HardwareInfo.PrintWmi("Win32_ComputerSystemProduct", new[] { "UUID" });

            ConsoleHelper.PrintHeader("CPU");
            HardwareInfo.PrintWmi(
                "Win32_Processor",
                new[] { "Name", "ProcessorId" },
                new[] { "Name", "Processor ID" });

            ConsoleHelper.PrintHeader("Memory (RAM)");
            HardwareInfo.PrintRam();

            ConsoleHelper.PrintHeader("GPU");
            HardwareInfo.PrintGpu();

            ConsoleHelper.PrintHeader("MAC Addresses");
            NetworkInfo.PrintMacAddresses();

            ConsoleHelper.PrintHeader("TPM");
            HardwareInfo.PrintTpm();

            ConsoleHelper.PrintHeader("Secure Boot");
            HardwareInfo.PrintSecureBoot();

            ConsoleHelper.PrintHeader("Local IP");
            NetworkInfo.PrintLocalIp();

            ConsoleHelper.PrintHeader("Public IP");
            await NetworkInfo.PrintPublicIpAsync();

            stopwatch.Stop();
            Console.WriteLine();
            ConsoleHelper.PrintInfo($"Scan complete in {stopwatch.Elapsed.TotalSeconds:0.0}s.");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Press any key to exit...");
            Console.ResetColor();
            Console.ReadLine();
        }
    }
}
