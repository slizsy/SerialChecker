using System;
using System.IO;
using System.Management;

namespace HwidChecker
{
    static class HardwareInfo
    {
        public static void PrintWmi(string wmiClass, string[] properties, string[]? labels = null)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher($"SELECT * FROM {wmiClass}");
                bool any = false;
                foreach (ManagementObject obj in searcher.Get())
                {
                    any = true;
                    for (int i = 0; i < properties.Length; i++)
                    {
                        var val = obj[properties[i]]?.ToString()?.Trim();
                        string label = labels != null && i < labels.Length ? labels[i] : properties[i];
                        ConsoleHelper.PrintKeyValue(label, string.IsNullOrEmpty(val) ? "N/A" : val);
                    }
                }
                if (!any) ConsoleHelper.PrintInfo("No data returned.");
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError($"Could not read {wmiClass}: {ex.Message}");
            }
        }

        public static void PrintDisks()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT Model, SerialNumber, Size FROM Win32_DiskDrive");
                ConsoleHelper.PrintColumnTitles(("Model", 26), ("Serial Number", 24), ("Size", 14));
                bool any = false;
                foreach (ManagementObject disk in searcher.Get())
                {
                    any = true;
                    string model = disk["Model"]?.ToString() ?? "N/A";
                    string serial = disk["SerialNumber"]?.ToString()?.Trim() ?? "N/A";
                    string sizeRaw = disk["Size"]?.ToString() ?? "0";
                    string size = FormatBytes(sizeRaw);
                    ConsoleHelper.PrintRow((model, 26), (serial, 24), (size, 14));
                }
                if (!any) ConsoleHelper.PrintInfo("No disks found.");
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError($"Could not read disks: {ex.Message}");
            }
        }

        static string FormatBytes(string rawBytes)
        {
            if (!long.TryParse(rawBytes, out long bytes) || bytes <= 0) return "N/A";
            double gb = bytes / 1_000_000_000.0;
            return gb >= 1000 ? $"{gb / 1000:0.##} TB" : $"{gb:0.##} GB";
        }

        public static void PrintVolumes()
        {
            try
            {
                var drives = DriveInfo.GetDrives();
                bool any = false;
                foreach (var d in drives)
                {
                    if (!d.IsReady) continue;
                    any = true;
                    string label = string.IsNullOrEmpty(d.VolumeLabel) ? "(no label)" : d.VolumeLabel;
                    ConsoleHelper.PrintKeyValue(d.Name.TrimEnd('\\'), label);
                }
                if (!any) ConsoleHelper.PrintInfo("No ready volumes found.");
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError($"Could not read volumes: {ex.Message}");
            }
        }

        public static void PrintRam()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT Capacity, Manufacturer, PartNumber, SerialNumber FROM Win32_PhysicalMemory");
                bool any = false;
                int i = 1;
                foreach (ManagementObject mem in searcher.Get())
                {
                    any = true;
                    string cap = mem["Capacity"] != null ? FormatBytes(mem["Capacity"].ToString() ?? "0") : "N/A";
                    string mfr = mem["Manufacturer"]?.ToString()?.Trim() ?? "N/A";
                    string part = mem["PartNumber"]?.ToString()?.Trim() ?? "N/A";
                    string serial = mem["SerialNumber"]?.ToString()?.Trim() ?? "N/A";
                    ConsoleHelper.PrintKeyValue($"Stick {i}", $"{cap}  {mfr}  {part}  S/N:{serial}");
                    i++;
                }
                if (!any) ConsoleHelper.PrintInfo("No RAM data returned.");
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError($"Could not read RAM: {ex.Message}");
            }
        }

        public static void PrintGpu()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT Name, AdapterRAM, DriverVersion FROM Win32_VideoController");
                bool any = false;
                foreach (ManagementObject gpu in searcher.Get())
                {
                    any = true;
                    string name = gpu["Name"]?.ToString() ?? "N/A";
                    string driver = gpu["DriverVersion"]?.ToString() ?? "N/A";
                    ConsoleHelper.PrintKeyValue(name, $"Driver {driver}");
                }
                if (!any) ConsoleHelper.PrintInfo("No GPU data returned.");
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError($"Could not read GPU info: {ex.Message}");
            }
        }

        public static void PrintTpm()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher(@"root\CIMV2\Security\MicrosoftTpm", "SELECT * FROM Win32_Tpm");
                bool found = false;
                foreach (ManagementObject obj in searcher.Get())
                {
                    found = true;
                    ConsoleHelper.PrintKeyValue("Manufacturer", obj["ManufacturerIdTxt"]?.ToString() ?? "N/A");
                    ConsoleHelper.PrintKeyValue("Present", obj["IsEnabled_InitialValue"]?.ToString() ?? "N/A");
                    ConsoleHelper.PrintKeyValue("Activated", obj["IsActivated_InitialValue"]?.ToString() ?? "N/A");
                    ConsoleHelper.PrintKeyValue("Owned", obj["IsOwned_InitialValue"]?.ToString() ?? "N/A");
                }
                if (!found) ConsoleHelper.PrintInfo("No TPM detected on this system.");
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError($"TPM info unavailable: {ex.Message}");
            }
        }

        public static void PrintSecureBoot()
        {
            try
            {
                using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\SecureBoot\State");
                var val = key?.GetValue("UEFISecureBootEnabled");
                bool enabled = val != null && val.ToString() == "1";
                ConsoleHelper.PrintKeyValue("SecureBoot (UEFI)", enabled.ToString(), enabled ? ConsoleColor.Green : ConsoleColor.Red);
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError($"Secure Boot info unavailable (may require admin rights): {ex.Message}");
            }
        }

        public static void PrintDisplay()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT CurrentHorizontalResolution, CurrentVerticalResolution, CurrentRefreshRate FROM Win32_VideoController");
                bool any = false;
                foreach (ManagementObject obj in searcher.Get())
                {
                    var w = obj["CurrentHorizontalResolution"];
                    var h = obj["CurrentVerticalResolution"];
                    var refresh = obj["CurrentRefreshRate"];
                    if (w == null || h == null) continue;
                    any = true;
                    ConsoleHelper.PrintKeyValue("Resolution", $"{w} x {h}  @ {refresh ?? "N/A"}Hz");
                }
                if (!any) ConsoleHelper.PrintInfo("No display data returned.");
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError($"Could not read display info: {ex.Message}");
            }
        }

        public static void PrintWindowsVersion()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT Caption, Version, BuildNumber FROM Win32_OperatingSystem");
                foreach (ManagementObject os in searcher.Get())
                {
                    ConsoleHelper.PrintKeyValue("OS", os["Caption"]?.ToString()?.Trim() ?? "N/A");
                    ConsoleHelper.PrintKeyValue("Version", os["Version"]?.ToString() ?? "N/A");
                    ConsoleHelper.PrintKeyValue("Build", os["BuildNumber"]?.ToString() ?? "N/A");
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError($"Could not read Windows version: {ex.Message}");
            }
        }
    }
}
