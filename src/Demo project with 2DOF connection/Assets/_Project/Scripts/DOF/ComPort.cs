#region

using System.IO.Ports;

#endregion

namespace DOF
{
    /// <summary>
    ///     Утилитарный класс для управления COM-портом с использованием SerialPort.
    /// </summary>
    public static class ComPort
    {
        private static SerialPort serialPort;

        /// <summary>
        ///     Попытаться установить соединение с COM-портом.
        /// </summary>
        /// <param name="comPortNumber">Номер COM-порта (по умолчанию 3).</param>
        /// <param name="baudRate">Скорость передачи данных (по умолчанию 115200 бит/с).</param>
        /// <param name="dataBits">Количество бит данных (по умолчанию 8).</param>
        /// <param name="stopBits">Стоп-биты (по умолчанию StopBits.One).</param>
        /// <returns>True, если соединение успешно установлено, иначе - false.</returns>
        public static bool TryConnect(int comPortNumber = 3, int baudRate = 115200, int dataBits = 8,
            StopBits stopBits = StopBits.One)
        {
            serialPort = new SerialPort
            {
                BaudRate = baudRate,
                DataBits = dataBits,
                Parity = Parity.None,
                StopBits = stopBits,
                ReadBufferSize = 4096,
                WriteBufferSize = 4096,
                ReadTimeout = 200,
                PortName = "COM" + comPortNumber
            };

            try
            {
                serialPort.Open();
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Разорвать соединение с COM-портом.
        /// </summary>
        public static void Disconnect()
        {
            try
            {
                if (IsOpen()) serialPort.Close();
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     Отправить строку данных через COM-порт.
        /// </summary>
        /// <param name="s">Строка данных для отправки.</param>
        public static void Write(string s)
        {
            try
            {
                if (IsOpen()) serialPort.Write(s);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     Отправить байтовый массив данных через COM-порт.
        /// </summary>
        /// <param name="bytes">Байтовый массив данных для отправки.</param>
        public static void Write(byte[] bytes)
        {
            try
            {
                if (IsOpen()) serialPort.Write(bytes, 0, bytes.Length);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     Проверить, открыто ли соединение с COM-портом.
        /// </summary>
        /// <returns>True, если соединение открыто, иначе - false.</returns>
        public static bool IsOpen()
        {
            try
            {
                return serialPort.IsOpen;
            }
            catch
            {
                return false;
            }
        }
    }
}