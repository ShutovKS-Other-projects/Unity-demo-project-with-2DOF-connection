#region

using System;

#endregion

namespace DOF.Data
{
    /// <summary>
    ///     Класс, представляющий информацию о игре и настройках оси.
    /// </summary>
    [Serializable]
    public class AxisGameData
    {
        /// <summary>
        ///     Конструктор класса AxisGameData.
        /// </summary>
        /// <param name="axisIndex">Индекс оси.</param>
        /// <param name="gamePort">Порт игры.</param>
        /// <param name="windProc">Процент ветра.</param>
        /// <param name="axisMode">Режим оси.</param>
        public AxisGameData(byte axisIndex, int gamePort, int windProc, int axisMode)
        {
            AxisIndex = axisIndex;
            GamePort = gamePort;
            AxisMode = axisMode;
            WindProc = windProc;
        }

        /// <summary>
        ///     Индекс оси.
        /// </summary>
        public byte AxisIndex { get; set; }

        /// <summary>
        ///     Порт игры.
        /// </summary>
        public int GamePort { get; set; }

        /// <summary>
        ///     Процент ветра.
        /// </summary>
        public int WindProc { get; set; }

        /// <summary>
        ///     Режим оси.
        /// </summary>
        public int AxisMode { get; set; }
    }
}