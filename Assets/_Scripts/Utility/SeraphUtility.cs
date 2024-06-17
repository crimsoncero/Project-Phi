
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

public static class SeraphUtility
{
    public static List<string> GetStringKeys(this Dictionary<object, object> dict)
    {
        List<string> keys =  new List<string>();

        foreach (var key in dict.Keys)
        {
            if(key is string)
                keys.Add(key as string);
        }

        return keys;
    }

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
