using System;

namespace Room
{
    public class TypeMessage
    {
        private enum Commnand { LightSwitch = 1, BlowUp }

        public static Func<byte[]> BuildBlowUpMsg =>
            () => new byte[] { (byte)Commnand.BlowUp };

        public static Func<bool, byte[]> BuildLightSwitchMsg =>
          mode => new byte[] { (byte)Commnand.LightSwitch, Convert.ToByte(mode) };

        public static Func<byte[], bool> ConvertLightSwitchMsg =>
            data => Convert.ToBoolean(data[1]);

        public static Func<byte[], bool> IsBlowUpMessage =>
           data => data?.Length == 1 && data[0] == (byte)Commnand.BlowUp;

        public static Func<byte[], bool> IsLightSwitchMsg =>
           data => data?.Length == 2 && data[0] == (byte)Commnand.LightSwitch;
    }
}