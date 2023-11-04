using System;

namespace Data
{
    [Serializable]
    public class AxisDofData
    {
        public string GameName { get; set; }
        public byte AxisIndex { get; set; }
        public bool Dir { get; set; }
        public string Force { get; set; }
        public int Proc { get; set; }
        public int Smoothing { get; set; }
        public int Nonlinear { get; set; }
        public int AntiRoll { get; set; }
        public int DeathZone { get; set; }
        public int DeathToZero { get; set; }
        public int SmoothingSim { get; set; }
        public int DeathToZeroTime { get; set; }
        public int DeathToZeroInterval { get; set; }

        public AxisDofData(string gameName, byte axisIndex)
        {
            GameName = gameName;
            AxisIndex = axisIndex;
            Dir = false;
            Force = "";
            Proc = 0;
            Nonlinear = 0;
            AntiRoll = 0;
            DeathZone = 0;
            DeathToZero = 0;
        }
    }
}