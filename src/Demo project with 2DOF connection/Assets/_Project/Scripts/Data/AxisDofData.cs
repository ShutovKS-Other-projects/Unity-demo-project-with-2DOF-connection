using System;

namespace Data
{
    /// <summary>
    /// Класс `AxisDofData` представляет собой структуру данных, предназначенную для хранения параметров управления осью (DOF - Degree of Freedom).
    /// </summary>
    [Serializable]
    public class AxisDofData
    {
        /// <summary>
        /// Получает или задает индекс оси, к которой применяются параметры управления.
        /// </summary>
        public byte AxisIndex { get; set; }

        /// <summary>
        /// Получает или задает направление движения оси (true - положительное, false - отрицательное).
        /// </summary>
        public bool Dir { get; set; }

        /// <summary>
        /// Получает или задает параметр силы, связанный с управлением осью.
        /// </summary>
        public string Force { get; set; }

        /// <summary>
        /// Получает или задает значение процесса (Proc) для управления осью.
        /// </summary>
        public int Proc { get; set; }

        /// <summary>
        /// Получает или задает значение сглаживания (Smoothing) для управления осью.
        /// </summary>
        public int Smoothing { get; set; }

        /// <summary>
        /// Получает или задает значение параметра Nonlinear для управления осью.
        /// </summary>
        public int Nonlinear { get; set; }

        /// <summary>
        /// Получает или задает значение параметра AntiRoll для управления осью.
        /// </summary>
        public int AntiRoll { get; set; }

        /// <summary>
        /// Получает или задает значение параметра DeathZone для управления осью.
        /// </summary>
        public int DeathZone { get; set; }

        /// <summary>
        /// Получает или задает значение параметра DeathToZero для управления осью.
        /// </summary>
        public int DeathToZero { get; set; }

        /// <summary>
        /// Получает или задает значение параметра SmoothingSim для управления осью.
        /// </summary>
        public int SmoothingSim { get; set; }

        /// <summary>
        /// Получает или задает значение времени DeathToZeroTime для управления осью.
        /// </summary>
        public int DeathToZeroTime { get; set; }

        /// <summary>
        /// Получает или задает интервал DeathToZeroInterval для управления осью.
        /// </summary>
        public int DeathToZeroInterval { get; set; }

        /// <summary>
        /// Конструктор класса `AxisDofData`, инициализирующий объект с указанием индекса оси.
        /// </summary>
        /// <param name="axisIndex">Индекс оси, к которой применяются параметры управления.</param>
        public AxisDofData(byte axisIndex)
        {
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