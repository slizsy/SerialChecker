using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace HwidChecker
{
    static class NetworkInfo
    {
        public static void PrintMacAddresses()
        {
            try
            {
                ConsoleHelper.PrintColumnTitles(("Interface", 34), ("Status", 10), ("MAC Address", 18));
                bool any = false;
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;
                    string mac = nic.GetPhysicalAddress().ToString();
                    if (string.IsNullOrEmpty(mac)) continue;
                    any = true;
                    string formattedMac = string.Join("-", System.Text.RegularExpressions.Regex.Matches(mac, ".{1,2}"));
                    ConsoleHelper.PrintRow((nic.Description, 34), (nic.OperationalStatus.ToString(), 10), (formattedMac, 18));
                }
                if (!any) ConsoleHelper.PrintInfo("No active network interfaces found.");
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError($"Could not read MAC addresses: {ex.Message}");
            }
        }

        public static void PrintLocalIp()
        {
            try
            {
                bool any = false;
                foreach (var ip in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (ip.AddressFamily != AddressFamily.InterNetwork) continue;
                    any = true;
                    ConsoleHelper.PrintKeyValue("Local IPv4", ip.ToString());
                }
                if (!any) ConsoleHelper.PrintInfo("No local IPv4 address found.");
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError($"Could not read local IP: {ex.Message}");
            }
        }

        public static async Task PrintPublicIpAsync()
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);
                var ip = await client.GetStringAsync("https://api.ipify.org");
                ConsoleHelper.PrintKeyValue("Public IPv4", ip.Trim());
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError($"Could not fetch public IP (no internet or blocked): {ex.Message}");
            }
        }
    }
}
