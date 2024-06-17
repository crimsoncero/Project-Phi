using System.Linq;
using System.Net.NetworkInformation;

namespace SeraphUtil
{
    public static class Utility
    {
        public static string GetMacAddress()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var networkInterface in networkInterfaces)
            {
                if (networkInterface.OperationalStatus != OperationalStatus.Up)
                {
                    continue;
                }
                var addressBytes = networkInterface.GetPhysicalAddress().GetAddressBytes();
                if (addressBytes.Length == 6)
                {
                    return string.Join(":", addressBytes.Select(b => b.ToString("X2")));
                }
            }
            return null;
        }
    }
}
