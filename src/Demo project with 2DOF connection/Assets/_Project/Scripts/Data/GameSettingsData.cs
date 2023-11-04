using System;

namespace Data
{
    [Serializable]
    public class GameSettingsData
    {
        public string GameName { get; set; }
        public double MaxRoll { get; set; }
        public double MinRoll { get; set; }
        public double MaxPitch { get; set; }
        public double MinPitch { get; set; }
        public double MaxYaw { get; set; }
        public double MinYaw { get; set; }
        public double MaxHeave { get; set; }
        public double MinHeave { get; set; }
        public double MaxSway { get; set; }
        public double MinSway { get; set; }
        public double MaxSurge { get; set; }
        public double MinSurge { get; set; }
        public double MaxExtra1 { get; set; }
        public double MinExtra1 { get; set; }
        public double MaxExtra2 { get; set; }
        public double MinExtra2 { get; set; }
        public double MaxExtra3 { get; set; }
        public double MinExtra3 { get; set; }

        public GameSettingsData(string gameName, double maxRoll, double minRoll, double maxPitch, double minPitch,
            double maxYaw, double minYaw, double maxHeave, double minHeave, double maxSway, double minSway,
            double maxSurge, double minSurge, double maxExtra1, double minExtra1, double maxExtra2, double minExtra2,
            double maxExtra3, double minExtra3)
        {
            GameName = gameName;
            MaxRoll = maxRoll;
            MinRoll = minRoll;
            MaxPitch = maxPitch;
            MinPitch = minPitch;
            MaxYaw = maxYaw;
            MinYaw = minYaw;
            MaxHeave = maxHeave;
            MinHeave = minHeave;
            MaxSway = maxSway;
            MinSway = minSway;
            MaxSurge = maxSurge;
            MinSurge = minSurge;
            MaxExtra1 = maxExtra1;
            MinExtra1 = minExtra1;
            MaxExtra2 = maxExtra2;
            MinExtra2 = minExtra2;
            MaxExtra3 = maxExtra3;
            MinExtra3 = minExtra3;
        }
    }
}