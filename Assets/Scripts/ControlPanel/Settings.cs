using System;
using System.Net;

namespace ControlPanel
{
    public class Settings
    {
        public int Port { get; set; }
        public IPAddress Address { get; set; }

        public Settings()
        {
            if (int.TryParse(Environment.GetEnvironmentVariable("port"), out var port))
            {
                Port = port;
            }
            else
            {
                Port = 8080;
            }

            if (IPAddress.TryParse(Environment.GetEnvironmentVariable("address"), out var address))
            {
                Address = address;
            }
            else
            {
                Address = IPAddress.Parse("127.0.0.1");
            }
        }
    }
}