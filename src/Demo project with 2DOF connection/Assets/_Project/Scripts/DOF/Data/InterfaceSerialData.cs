#region

using System;

#endregion

namespace DOF.Data
{
    /// <summary>
    ///     Класс `InterfaceSerialData` представляет собой структуру данных, используемую для хранения параметров интерфейса
    ///     серийной связи, таких как COM-порт, скорость передачи данных и другие параметры.
    /// </summary>
    [Serializable]
    public class InterfaceSerialData
    {
        /// <summary>
        ///     Инициализирует новый экземпляр класса `InterfaceSerialData` с заданными параметрами.
        /// </summary>
        /// <param name="comPort">COM-порт.</param>
        /// <param name="bitsPerSec">Скорость передачи данных.</param>
        /// <param name="stopBits">Количество стоп-битов.</param>
        /// <param name="dataBits">Количество бит данных.</param>
        /// <param name="startUp">Настройки запуска интерфейса.</param>
        /// <param name="interfaceData">Данные интерфейса.</param>
        /// <param name="shutDown">Настройки завершения работы интерфейса.</param>
        /// <param name="startUpMsec">Задержка перед запуском интерфейса в миллисекундах.</param>
        /// <param name="interfaceDataMsec">Задержка для передачи данных интерфейса в миллисекундах.</param>
        /// <param name="shutDownMsec">Задержка перед завершением работы интерфейса в миллисекундах.</param>
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

        /// <summary>
        ///     Инициализирует новый экземпляр класса `InterfaceSerialData` с параметрами по умолчанию.
        /// </summary>
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

        public static InterfaceSerialData Default => new()
        {
            ComPort = "COM3",
            BitsPerSec = "115200",
            StopBits = "1",
            DataBits = "8",
            StartUp = null,
            InterfaceData = "L<Motor1a>R<Motor2a>Z<Motor4a>",
            ShutDown = null,
            StartUpMsec = 0,
            InterfaceDataMsec = 0,
            ShutDownMsec = 0,
            Vid = "0403",
            Pid = "6001"
        };

        /// <summary>
        ///     Получает или задает COM-порт, используемый для связи.
        /// </summary>
        public string ComPort { get; set; }

        /// <summary>
        ///     Получает или задает скорость передачи данных в битах в секунду (бит/сек).
        /// </summary>
        public string BitsPerSec { get; set; }

        /// <summary>
        ///     Получает или задает количество стоп-битов, используемых в коммуникации.
        /// </summary>
        public string StopBits { get; set; }

        /// <summary>
        ///     Получает или задает количество бит данных в каждом пакете данных.
        /// </summary>
        public string DataBits { get; set; }

        /// <summary>
        ///     Получает или задает строку, представляющую настройки запуска интерфейса.
        /// </summary>
        public string StartUp { get; set; }

        /// <summary>
        ///     Получает или задает строку, представляющую данные интерфейса.
        /// </summary>
        public string InterfaceData { get; set; }

        /// <summary>
        ///     Получает или задает строку, представляющую настройки завершения работы интерфейса.
        /// </summary>
        public string ShutDown { get; set; }

        /// <summary>
        ///     Получает или задает время задержки в миллисекундах перед запуском интерфейса.
        /// </summary>
        public int StartUpMsec { get; set; }

        /// <summary>
        ///     Получает или задает время задержки в миллисекундах для передачи данных интерфейса.
        /// </summary>
        public int InterfaceDataMsec { get; set; }

        /// <summary>
        ///     Получает или задает время задержки в миллисекундах перед завершением работы интерфейса.
        /// </summary>
        public int ShutDownMsec { get; set; }

        /// <summary>
        ///     Получает или задает идентификатор устройства (Vendor ID) для интерфейса.
        /// </summary>
        public string Vid { get; set; }

        /// <summary>
        ///     Получает или задает идентификатор продукта (Product ID) для интерфейса.
        /// </summary>
        public string Pid { get; set; }
    }
}