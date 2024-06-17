
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace SeraphUtil
{
    public static class Extensions
    {
        public static List<string> GetStringKeys(this Dictionary<object, object> dict)
        {
            List<string> keys = new List<string>();

            foreach (var key in dict.Keys)
            {
                if (key is string)
                    keys.Add(key as string);
            }

            return keys;
        }
    }
}