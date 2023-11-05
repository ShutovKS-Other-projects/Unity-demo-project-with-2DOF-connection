#region

using System;

#endregion

namespace DOF.Data
{
    /// <summary>
    ///     Класс, представляющий данные для перенаправления на игровой сервер.
    /// </summary>
    [Serializable]
    public class RedirectData
    {
        /// <summary>
        ///     Инициализирует новый экземпляр класса RedirectData с указанными параметрами.
        /// </summary>
        /// <param name="ip">IP-адрес игрового сервера.</param>
        /// <param name="port">Порт игрового сервера.</param>
        /// <param name="gameName">Имя игры.</param>
        public RedirectData(string ip, int port, string gameName)
        {
            Ip = ip;
            Port = port;
            GameName = gameName;
        }

        /// <summary>
        ///     Получает или задает имя игры.
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        ///     Получает или задает IP-адрес игрового сервера.
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        ///     Получает или задает порт игрового сервера.
        /// </summary>
        public int Port { get; set; }
    }
}