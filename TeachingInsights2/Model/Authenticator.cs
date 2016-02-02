using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TeachingInsights2.Model
{
    public class Authenticator
    {
        public static bool Authorize(String username, String password)
        {
            using(var context = new TeachingInsightsContext())
            {
                var user = context.Users.Find(username);
                if (user != null)
                    if (user.password.Equals(password))
                    {
                        TeachingInsights2.Settings.Default.username = user.username;
                        user.isConnected = true;
                        user.currentIpAddress = GetLocalIPAddress();
                        Console.WriteLine(user.currentIpAddress);
                        return true;
                    }

                return false;
            }
        }

        public static string GetLocalIPAddress()
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                throw new Exception("Local IP Address Not Found!");
            }
            else
                return "";
        }
    }
}
