#region

using System;
using System.Linq;
using System.Text;
using System.Threading;
using DOF.Data;

#endregion

namespace DOF
{
    /// <summary>
    ///     Класс, представляющий интерфейс для взаимодействия с устройством через последовательный порт.
    /// </summary>
    public class SerialInterface
    {
        /// <summary>
        ///     Конструктор класса SerialInterface, инициализирующий объект для взаимодействия с устройством.
        /// </summary>
        /// <param name="interfaceSerialData">Данные о последовательном порту.</param>
        /// <param name="gameTelemetry">Данные о телеметрии игры.</param>
        public SerialInterface(InterfaceSerialData interfaceSerialData, GameTelemetry gameTelemetry)
        {
            _interfaceSerialData = interfaceSerialData;
            _gameTelemetry = gameTelemetry;

            StartComPort();
            Start();
        }

        /// <summary>
        ///     Приватное поле для хранения объекта AxisAssignments, представляющего назначения осей движения для группы A моторов.
        /// </summary>
        private readonly AxisAssignments _axisAssignmentsA = new();

        /// <summary>
        ///     Приватное поле для хранения объекта AxisAssignments, представляющего назначения осей движения для группы B моторов.
        /// </summary>
        private readonly AxisAssignments _axisAssignmentsB = new();

        /// <summary>
        ///     Данные о телеметрии игры для взаимодействия с устройством.
        /// </summary>
        private readonly GameTelemetry _gameTelemetry;

        /// <summary>
        ///     Данные о последовательном порту для взаимодействия с устройством.
        /// </summary>
        private readonly InterfaceSerialData _interfaceSerialData;

        /// <summary>
        ///     Последние измеренные значения оси A (для каждого мотора).
        /// </summary>
        private readonly double[] _lastAxisA = new double[9];

        /// <summary>
        ///     Последние измеренные значения оси B (для каждого мотора).
        /// </summary>
        private readonly double[] _lastAxisB = new double[9];

        /// <summary>
        ///     Приватное поле для указания завершения работы класса.
        /// </summary>
        private readonly bool _terminated = false;

        /// <summary>
        ///     Приватное поле для указания на приостановку игры.
        /// </summary>
        private bool _gamePaused;

        /// <summary>
        ///     Приватное поле для хранения последнего времени изменения данных.
        /// </summary>
        private DateTime _lastChangeTime;

        /// <summary>
        ///     Последние измеренные значения оси Pitch.
        /// </summary>
        private double _lastPitch;

        /// <summary>
        ///     Последние измеренные значения оси Roll.
        /// </summary>
        private double _lastRoll;

        /// <summary>
        ///     Последние измеренные значения оси Surge.
        /// </summary>
        private double _lastSurge;

        /// <summary>
        ///     Последние измеренные значения оси Sway.
        /// </summary>
        private double _lastSway;

        /// <summary>
        ///     Последние измеренные значения оси Yaw.
        /// </summary>
        private double _lastYaw;

        /// <summary>
        ///     Приватное поле для указания на остановку работы с устройством.
        /// </summary>
        private bool _stop;

        /// <summary>
        ///     Приватное поле для указания на остановку работы с устройством в режиме платформы.
        /// </summary>
        private bool _stopPlatformMode;

        /// <summary>
        ///     Приватное поле для хранения строки данных SData.
        /// </summary>
        private string SData { get; set; }


        /// <summary>
        ///     Приватный метод для инициализации соединения с последовательным портом.
        /// </summary>
        private void StartComPort()
        {
            ComPort.TryConnect();
        }

        /// <summary>
        ///     Приватный метод для завершения соединения с последовательным портом.
        /// </summary>
        private void StopComPort()
        {
            ComPort.Disconnect();
        }

        /// <summary>
        ///     Метод для установки параметров взаимодействия с устройством.
        /// </summary>
        /// <param name="started">Флаг, указывающий, начата ли работа с устройством.</param>
        public void SetUp(bool started)
        {
            if (started)
            {
                _stop = false;

                if (!_stopPlatformMode)
                {
                    StartComPort();
                }

                _stopPlatformMode = false;
                _gamePaused = false;

                var axisDofDataArray = new[]
                {
                    Settings.gameAxes.Where(x => x.AxisIndex == 0).ToArray(),
                    Settings.gameAxes.Where(x => x.AxisIndex == 1).ToArray(),
                    Settings.gameAxes2.Where(x => x.AxisIndex == 0).ToArray(),
                    Settings.gameAxes2.Where(x => x.AxisIndex == 1).ToArray()
                };

                var gameSetting = Settings.GameSettingsData;

                _axisAssignmentsA.SetAxisDofs(axisDofDataArray[0], axisDofDataArray[2], gameSetting.MinPitch,
                    gameSetting.MaxPitch, gameSetting.MinRoll, gameSetting.MaxRoll, gameSetting.MinYaw,
                    gameSetting.MaxYaw,
                    gameSetting.MinSurge, gameSetting.MaxSurge, gameSetting.MinSway, gameSetting.MaxSway,
                    gameSetting.MinHeave, gameSetting.MaxHeave, gameSetting.MinExtra1, gameSetting.MaxExtra1,
                    gameSetting.MinExtra2, gameSetting.MaxExtra2, gameSetting.MinExtra3, gameSetting.MaxExtra3,
                    gameSetting.WindProc, Settings.windCoefValue, Settings.windConst ? 1 : 0);

                _axisAssignmentsB.SetAxisDofs(axisDofDataArray[1], axisDofDataArray[3], gameSetting.MinPitch,
                    gameSetting.MaxPitch, gameSetting.MinRoll, gameSetting.MaxRoll, gameSetting.MinYaw,
                    gameSetting.MaxYaw,
                    gameSetting.MinSurge, gameSetting.MaxSurge, gameSetting.MinSway, gameSetting.MaxSway,
                    gameSetting.MinHeave, gameSetting.MaxHeave, gameSetting.MinExtra1, gameSetting.MaxExtra1,
                    gameSetting.MinExtra2, gameSetting.MaxExtra2, gameSetting.MinExtra3, gameSetting.MaxExtra3,
                    gameSetting.WindProc, Settings.windCoefValue, Settings.windConst ? 1 : 0);
            }
            else
            {
                _stopPlatformMode = true;
            }
        }

        /// <summary>
        ///     Метод для получения индексов осей движения из данных интерфейса.
        /// </summary>
        /// <param name="indAxis">Массив для хранения индексов осей движения.</param>
        /// <param name="interfaceData">Данные о последовательном порту.</param>
        private void GetInterfaceAxisIndex(int[] indAxis, ref string interfaceData)
        {
            var motorSubstrings = new[]
            {
                "<Motor1a>", "<Motor2a>", "<Motor3a>", "<Motor4a>", "<Motor5a>",
                "<Motor6a>", "<Motor7a>", "<Motor8a>", "<Motor9a>",
                "<Motor1b>", "<Motor2b>", "<Motor3b>", "<Motor4b>", "<Motor5b>",
                "<Motor6b>", "<Motor7b>", "<Motor8b>", "<Motor9b>"
            };

            for (var index = 0; index < 18; ++index)
            {
                indAxis[index] = -1;
            }

            foreach (var motorSubstring in motorSubstrings)
            {
                var index = interfaceData.IndexOf(motorSubstring);
                if (index != -1)
                {
                    indAxis[Array.IndexOf(motorSubstrings, motorSubstring)] = index;
                    interfaceData = interfaceData.Remove(index, motorSubstring is "<Motor9a>" or "<Motor9b>" ? 9 : 8);
                }
            }
        }

        /// <summary>
        ///     Преобразует значение типа byte в массив байт в формате ASCII.
        /// </summary>
        /// <param name="value">Значение типа byte для преобразования.</param>
        /// <returns>Массив байт в формате ASCII.</returns>
        private byte[] GetHexBytes(byte value)
        {
            return Encoding.ASCII.GetBytes(value.ToString("X2"));
        }

        /// <summary>
        ///     Преобразует целочисленное значение в массив байт в формате ASCII с фиксированной шириной.
        /// </summary>
        /// <param name="value">Целочисленное значение для преобразования.</param>
        /// <returns>Массив байт в формате ASCII.</returns>
        private byte[] GetDecimalBytes(int value)
        {
            if (value > 999)
            {
                value = 999;
            }

            return Encoding.ASCII.GetBytes(value.ToString("000"));
        }

        /// <summary>
        ///     Определяет приостановку игры на основе изменений в значениях осей движения.
        /// </summary>
        /// <param name="pitch">Значение оси Pitch.</param>
        /// <param name="roll">Значение оси Roll.</param>
        /// <param name="yaw">Значение оси Yaw.</param>
        /// <param name="sway">Значение оси Sway.</param>
        /// <param name="surge">Значение оси Surge.</param>
        private void DetectPauseGame(double pitch, double roll, double yaw, double sway, double surge)
        {
            if (pitch != _lastPitch || roll != _lastRoll || yaw != _lastYaw || surge != _lastSurge || sway != _lastSway)
            {
                _lastChangeTime = DateTime.Now;
            }

            if ((pitch != 0.0 || roll != 0.0 || yaw != 0.0 || sway != 0.0 || surge != 0.0) &&
                (DateTime.Now - _lastChangeTime).TotalSeconds > 3.0)
            {
                if (!_gamePaused)
                {
                    _gamePaused = true;
                }
            }
            else if (_gamePaused)
            {
                _gamePaused = false;
            }

            _lastPitch = pitch;
            _lastRoll = roll;
            _lastYaw = yaw;
            _lastSurge = surge;
            _lastSway = sway;
        }

        /// <summary>
        ///     Метод для запуска основной логики работы с устройством.
        /// </summary>
        private void Start()
        {
            new Thread(() =>
            {
                var indAxis = new int[18];
                var interfaceData = _interfaceSerialData.InterfaceData;
                GetInterfaceAxisIndex(indAxis, ref interfaceData);
                var bytes = Encoding.ASCII.GetBytes(interfaceData);
                var absValues = new bool[16];

                while (!_terminated)
                {
                    var pitch = _gameTelemetry.Pitch;
                    var roll = _gameTelemetry.Roll;
                    var yaw = _gameTelemetry.Yaw;
                    var surge = _gameTelemetry.Surge;
                    var sway = _gameTelemetry.Sway;
                    var heave = _gameTelemetry.Heave;
                    var extra1 = _gameTelemetry.Extra1;
                    var extra2 = _gameTelemetry.Extra2;
                    var extra3 = _gameTelemetry.Extra3;
                    var wind = _gameTelemetry.Wind;

                    _axisAssignmentsA.ProcessingData(pitch, roll, yaw, surge, sway, heave, extra1, extra2, extra3,
                        wind);
                    _axisAssignmentsB.ProcessingData(pitch, roll, yaw, surge, sway, heave, extra1, extra2, extra3,
                        wind);

                    if (!_stopPlatformMode && !_gamePaused)
                    {
                        for (var i = 0; i < 8; i++)
                        {
                            _lastAxisA[i] = _axisAssignmentsA.GetAxis(i + 1, ref absValues[i]);
                            _lastAxisB[i] = _axisAssignmentsB.GetAxis(i + 1, ref absValues[i + 8]);
                        }

                        _lastAxisA[8] = _axisAssignmentsA.GetAxis9();
                        _lastAxisB[8] = _axisAssignmentsB.GetAxis9();
                    }
                    else if (Settings.shutdownValue == 0)
                    {
                        for (var i = 0; i < 9; i++)
                        {
                            _lastAxisA[i] = 0.0;
                            _lastAxisB[i] = 0.0;
                        }
                    }
                    else
                    {
                        for (var i = 0; i < 8; i++)
                        {
                            if (absValues[i])
                            {
                                _lastAxisA[i] = 0.0;
                            }

                            if (absValues[i + 8])
                            {
                                _lastAxisB[i] = 0.0;
                            }
                        }

                        _lastAxisA[8] = 0.0;
                        _lastAxisB[8] = 0.0;

                        for (var i = 0; i < 6; i++)
                        {
                            if (_lastAxisA[i] != 0.0)
                            {
                                if (_lastAxisA[i] > 0.0)
                                {
                                    _lastAxisA[i] -= (101 - Settings.shutdownValue) / 10000.0;
                                    if (_lastAxisA[i] < 0.0)
                                    {
                                        _lastAxisA[i] = 0.0;
                                    }
                                }
                                else
                                {
                                    _lastAxisA[i] += (101 - Settings.shutdownValue) / 10000.0;
                                    if (_lastAxisA[i] > 0.0)
                                    {
                                        _lastAxisA[i] = 0.0;
                                    }
                                }
                            }

                            if (_lastAxisB[i] != 0.0)
                            {
                                if (_lastAxisB[i] > 0.0)
                                {
                                    _lastAxisB[i] -= (101 - Settings.shutdownValue) / 10000.0;
                                    if (_lastAxisB[i] < 0.0)
                                    {
                                        _lastAxisB[i] = 0.0;
                                    }
                                }
                                else
                                {
                                    _lastAxisB[i] += (101 - Settings.shutdownValue) / 10000.0;
                                    if (_lastAxisB[i] > 0.0)
                                    {
                                        _lastAxisB[i] = 0.0;
                                    }
                                }
                            }
                        }
                    }

                    for (var i = 0; i < 18; i++)
                    {
                        if (indAxis[i] < 0)
                        {
                            continue;
                        }

                        byte num;

                        switch (i)
                        {
                            case < 8:
                            {
                                num = !absValues[i]
                                    ? (byte)(sbyte.MaxValue * _lastAxisA[i] + sbyte.MaxValue)
                                    : (byte)_lastAxisA[i];
                                break;
                            }
                            case < 16:
                            {
                                num = !absValues[i]
                                    ? (byte)(sbyte.MaxValue * _lastAxisB[i - 8] + sbyte.MaxValue)
                                    : (byte)_lastAxisB[i - 8];
                                break;
                            }
                            default:
                            {
                                var value = i == 16 ? (int)_lastAxisA[8] : (int)_lastAxisB[8];
                                var decimalBytes = GetDecimalBytes(value);
                                bytes[indAxis[i]] = decimalBytes[0];
                                bytes[indAxis[i] + 1] = decimalBytes[1];
                                bytes[indAxis[i] + 2] = decimalBytes[2];
                                continue;
                            }
                        }

                        var hexBytes = GetHexBytes(num);
                        bytes[indAxis[i]] = hexBytes[0];
                        bytes[indAxis[i] + 1] = hexBytes[1];
                    }

                    if (!_stop)
                    {
                        SData = Encoding.Default.GetString(bytes);
                    }

                    try
                    {
                        ComPort.Write(bytes);
                    }
                    catch
                    {
                    }

                    if (_stopPlatformMode &&
                        _lastAxisA[0] == 0.0 && _lastAxisA[1] == 0.0 && _lastAxisA[2] == 0.0 && _lastAxisA[3] == 0.0 &&
                        _lastAxisA[4] == 0.0 && _lastAxisA[5] == 0.0 &&
                        _lastAxisB[0] == 0.0 && _lastAxisB[1] == 0.0 && _lastAxisB[2] == 0.0 && _lastAxisB[3] == 0.0 &&
                        _lastAxisB[4] == 0.0 && _lastAxisB[5] == 0.0)
                    {
                        _stopPlatformMode = false;
                        _stop = true;
                        Thread.Sleep(100);
                        StopComPort();
                    }

                    Thread.Sleep(_interfaceSerialData.InterfaceDataMsec);
                }
            }).Start();
        }
    }
}