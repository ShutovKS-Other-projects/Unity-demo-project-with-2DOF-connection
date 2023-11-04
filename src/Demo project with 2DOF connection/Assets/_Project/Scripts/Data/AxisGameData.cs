using System;

namespace Data
{
    [Serializable]
    public class AxisGameData
    {
        public string GameName { get; set; }
        public byte AxisIndex { get; set; }
        public int WindProc { get; set; }
        public int GamePort { get; set; }
        public int AxisMode { get; set; }

        public AxisGameData(
            string gameName,
            byte axisIndex,
            int gamePort,
            int windProc,
            int axisMode)
        {
            GameName = gameName;
            AxisIndex = axisIndex;
            GamePort = gamePort;
            AxisMode = axisMode;
            WindProc = windProc;
        }
    }
}