using System;

namespace Room
{
    public class Settings
    {
        public int Port { get; set; }
        public bool LampIsOn { get; set; }

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

            if (bool.TryParse(Environment.GetEnvironmentVariable("lamp"), out var lampIsOn))
            {
                LampIsOn = lampIsOn;
            }
            else
            {
                LampIsOn = true;
            }
        }
    }
}