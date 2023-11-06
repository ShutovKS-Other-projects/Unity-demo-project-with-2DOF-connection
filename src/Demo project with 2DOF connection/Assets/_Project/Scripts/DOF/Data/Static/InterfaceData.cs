namespace DOF.Data.Static
{
    public static class InterfaceData
    {
        public const int comPort = 3;
        public const string bitsPerSec = "115200";
        public const string stopBits = "1";
        public const string dataBits = "8";
        public const string startUp = default; // ???
        public const string interfaceData = "L<Motor1a>R<Motor2a>Z<Motor3a>Y<Motor4a>";
        public const string shutDown = default; // ???
        public const int startUp_msec = 0;
        public const int interfaceData_msec = 20;
        public const int shutDown_msec = 0;
    }
}