using System;

namespace Data
{
    [Serializable]
    public class InterfaceSerialData
    {
        public string ComPort { get; set; }
        public string BitsPerSec { get; set; }
        public string StopBits { get; set; }
        public string DataBits { get; set; }
        public string StartUp { get; set; }
        public string InterfaceData { get; set; }
        public string ShutDown { get; set; }
        public int StartUpMsec { get; set; }
        public int InterfaceDataMsec { get; set; }
        public int ShutDownMsec { get; set; }
        public string Vid { get; set; }
        public string Pid { get; set; }

        public InterfaceSerialData(string comPort, string bitsPerSec, string stopBits, string dataBits, string startUp,
            string interfaceData, string shutDown, int startUpMsec, int interfaceDataMsec, int shutDownMsec)
        {
            ComPort = comPort;
            BitsPerSec = bitsPerSec;
            StopBits = stopBits;
            DataBits = dataBits;
            StartUp = startUp;
            InterfaceData = interfaceData;
            ShutDown = shutDown;
            StartUpMsec = startUpMsec;
            InterfaceDataMsec = interfaceDataMsec;
            ShutDownMsec = shutDownMsec;
        }

        public InterfaceSerialData()
        {
            ComPort = "";
            BitsPerSec = "";
            StopBits = "";
            DataBits = "";
            StartUp = "";
            InterfaceData = "";
            ShutDown = "";
            StartUpMsec = 0;
            InterfaceDataMsec = 0;
            ShutDownMsec = 0;
        }
    }
}