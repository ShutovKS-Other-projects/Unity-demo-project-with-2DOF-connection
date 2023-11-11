#region

using System;

#endregion

namespace DOF.Data.Dynamic
{
    [Serializable]
    public class AxisDofData
    {
        public AxisDofData(byte axisIndex)
        {
            AxisIndex = axisIndex;
            Dir = false;
            Force = "";
            Proc = 0;
            Nonlinear = 0;
            Antiroll = 0;
            Deathzone = 0;
            DeathToZero = 0;
        }

        public byte AxisIndex { get; set; }

        public bool Dir { get; set; }

        public string Force { get; set; }

        public int Proc { get; set; }

        public int Smoothing { get; set; }

        public int Nonlinear { get; set; }

        public int Antiroll { get; set; }

        public int Deathzone { get; set; }

        public int DeathToZero { get; set; }

        public int SmoothingSim { get; set; }

        public int DeathToZeroTime { get; set; }

        public int DeathToZeroInterval { get; set; }
    }
}