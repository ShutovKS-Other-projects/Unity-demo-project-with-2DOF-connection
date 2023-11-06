#region

using System;

#endregion

namespace DOF.Data
{
    /// <summary>
    ///     Класс `GameSettingsData` представляет настройки игры, связанные с движением объектов.
    ///     Этот класс используется для хранения параметров, таких как ограничения на вращение и движение объектов в игре.
    /// </summary>
    [Serializable]
    public class GameSettingsData
    {
        /// <summary>
        ///     Конструктор класса `GameSettingsData`, принимающий параметры настроек игры.
        /// </summary>
        /// <param name="maxRoll">Максимальное значение угла крена (Roll).</param>
        /// <param name="minRoll">Минимальное значение угла крена (Roll).</param>
        /// <param name="maxPitch">Максимальное значение угла тангажа (Pitch).</param>
        /// <param name="minPitch">Минимальное значение угла тангажа (Pitch).</param>
        /// <param name="maxYaw">Максимальное значение угла рысканья (Yaw).</param>
        /// <param name="minYaw">Минимальное значение угла рысканья (Yaw).</param>
        /// <param name="maxHeave">Максимальное значение вертикального движения (Heave).</param>
        /// <param name="minHeave">Минимальное значение вертикального движения (Heave).</param>
        /// <param name="maxSway">Максимальное значение бокового движения (Sway).</param>
        /// <param name="minSway">Минимальное значение бокового движения (Sway).</param>
        /// <param name="maxSurge">Максимальное значение продольного движения (Surge).</param>
        /// <param name="minSurge">Минимальное значение продольного движения (Surge).</param>
        /// <param name="maxExtra1">Максимальное значение дополнительного параметра 1.</param>
        /// <param name="minExtra1">Минимальное значение дополнительного параметра 1.</param>
        /// <param name="maxExtra2">Максимальное значение дополнительного параметра 2.</param>
        /// <param name="minExtra2">Минимальное значение дополнительного параметра 2.</param>
        /// <param name="maxExtra3">Максимальное значение дополнительного параметра 3.</param>
        /// <param name="minExtra3">Минимальное значение дополнительного параметра 3.</param>
        /// <param name="windProc">Количество процессоров ветра, используемых в игре.</param>
        public GameSettingsData(double maxRoll, double minRoll, double maxPitch, double minPitch,
            double maxYaw, double minYaw, double maxHeave, double minHeave, double maxSway, double minSway,
            double maxSurge, double minSurge, double maxExtra1, double minExtra1, double maxExtra2, double minExtra2,
            double maxExtra3, double minExtra3, int windProc)
        {
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
            WindProc = windProc;
        }

        private GameSettingsData()
        {
        }

        /// <summary>
        ///     Максимальное значение угла крена (Roll) для объекта.
        /// </summary>
        public double MaxRoll { get; set; }

        /// <summary>
        ///     Минимальное значение угла крена (Roll) для объекта.
        /// </summary>
        public double MinRoll { get; set; }

        /// <summary>
        ///     Максимальное значение угла тангажа (Pitch) для объекта.
        /// </summary>
        public double MaxPitch { get; set; }

        /// <summary>
        ///     Минимальное значение угла тангажа (Pitch) для объекта.
        /// </summary>
        public double MinPitch { get; set; }

        /// <summary>
        ///     Максимальное значение угла рысканья (Yaw) для объекта.
        /// </summary>
        public double MaxYaw { get; set; }

        /// <summary>
        ///     Минимальное значение угла рысканья (Yaw) для объекта.
        /// </summary>
        public double MinYaw { get; set; }

        /// <summary>
        ///     Максимальное значение вертикального движения (Heave) для объекта.
        /// </summary>
        public double MaxHeave { get; set; }

        /// <summary>
        ///     Минимальное значение вертикального движения (Heave) для объекта.
        /// </summary>
        public double MinHeave { get; set; }

        /// <summary>
        ///     Максимальное значение бокового движения (Sway) для объекта.
        /// </summary>
        public double MaxSway { get; set; }

        /// <summary>
        ///     Минимальное значение бокового движения (Sway) для объекта.
        /// </summary>
        public double MinSway { get; set; }

        /// <summary>
        ///     Максимальное значение продольного движения (Surge) для объекта.
        /// </summary>
        public double MaxSurge { get; set; }

        /// <summary>
        ///     Минимальное значение продольного движения (Surge) для объекта.
        /// </summary>
        public double MinSurge { get; set; }

        /// <summary>
        ///     Максимальное значение дополнительного параметра 1 для объекта.
        /// </summary>
        public double MaxExtra1 { get; set; }

        /// <summary>
        ///     Минимальное значение дополнительного параметра 1 для объекта.
        /// </summary>
        public double MinExtra1 { get; set; }

        /// <summary>
        ///     Максимальное значение дополнительного параметра 2 для объекта.
        /// </summary>
        public double MaxExtra2 { get; set; }

        /// <summary>
        ///     Минимальное значение дополнительного параметра 2 для объекта.
        /// </summary>
        public double MinExtra2 { get; set; }

        /// <summary>
        ///     Максимальное значение дополнительного параметра 3 для объекта.
        /// </summary>
        public double MaxExtra3 { get; set; }

        /// <summary>
        ///     Минимальное значение дополнительного параметра 3 для объекта.
        /// </summary>
        public double MinExtra3 { get; set; }

        /// <summary>
        ///     Количество процессоров ветра, используемых в игре.
        /// </summary>
        public int WindProc { get; set; }

        public static GameSettingsData Default => new()
        {
            MaxRoll = 0,
            MinRoll = 3.390625,
            MaxPitch = 0,
            MinPitch = -3.390625,
            MaxYaw = 0,
            MinYaw = -3.390625,
            MaxHeave = 0,
            MinHeave = 3.390625,
            MaxSway = 0,
            MinSway = 3.390625,
            MaxSurge = 0,
            MinSurge = -3.390625,
            MaxExtra1 = 0,
            MinExtra1 = 3.390625,
            MaxExtra2 = 0,
            MinExtra2 = -3.390625,
            MaxExtra3 = 0,
            MinExtra3 = 3.390625,
            WindProc = 0
        };
    }
}