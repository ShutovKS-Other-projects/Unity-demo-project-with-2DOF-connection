using System.Text;
using System.Threading;
using DOF.Data;
using DOF.Data.Dynamic;
using DOF.Data.Static;
using UnityEngine;

namespace DOF
{
    public class DataProcessingAndTransmission
    {
        public DataProcessingAndTransmission(
            ObjectTelemetryData objectTelemetryData,
            AxisAssignments axisAssignmentsA,
            AxisAssignments axisAssignmentsB)
        {
            _objectTelemetryData = objectTelemetryData;
            _axisAssignmentsA = axisAssignmentsA;
            _axisAssignmentsB = axisAssignmentsB;
        }

        private ObjectTelemetryData _objectTelemetryData;
        private AxisAssignments _axisAssignmentsA;
        private AxisAssignments _axisAssignmentsB;
        private double[] _lastAxisA = new double[9];
        private double[] _lastAxisB = new double[9];
        private string _sData;

        public void AxisAssignmentsSetUp(
            AxisDofData[] axisDofs1, AxisDofData[] axisDofs2, AxisDofData[] axisDofs3, AxisDofData[] axisDofs4,
            double minPitch, double maxPitch, double minRoll, double maxRoll, double minYaw, double maxYaw,
            double minSurge, double maxSurge, double minSway, double maxSway, double minHeave, double maxHeave,
            double minExtra1, double maxExtra1, double minExtra2, double maxExtra2, double minExtra3, double maxExtra3)
        {
            _axisAssignmentsA.SetAxisDofs(axisDofs1, axisDofs3,
                minPitch, maxPitch, minRoll, maxRoll, minYaw, maxYaw, minSurge, maxSurge, minSway, maxSway,
                minHeave, maxHeave, minExtra1, maxExtra1, minExtra2, maxExtra2, minExtra3, maxExtra3,
                0, 100, 0);

            _axisAssignmentsB.SetAxisDofs(axisDofs2, axisDofs4,
                minPitch, maxPitch, minRoll, maxRoll, minYaw, maxYaw, minSurge, maxSurge, minSway, maxSway,
                minHeave, maxHeave, minExtra1, maxExtra1, minExtra2, maxExtra2, minExtra3, maxExtra3,
                0, 100, 0);
        }

        public void Start() => new Thread(() =>
        {
            var indAxis = new int[18];
            var interfaceData = InterfaceData.interfaceData;
            GetInterfaceAxisIndex(indAxis, ref interfaceData);
            var bytes = Encoding.ASCII.GetBytes(interfaceData);
            var absValues = new bool[16];

            while (true)
            {
                var pitch = _objectTelemetryData.Pitch;
                var roll = _objectTelemetryData.Roll;
                var yaw = _objectTelemetryData.Yaw;
                var surge = _objectTelemetryData.Surge;
                var sway = _objectTelemetryData.Sway;
                var heave = _objectTelemetryData.Heave;
                var extra1 = _objectTelemetryData.Extra1;
                var extra2 = _objectTelemetryData.Extra2;
                var extra3 = _objectTelemetryData.Extra3;
                var wind = _objectTelemetryData.Wind;

                _axisAssignmentsA.ProcessingData(pitch, roll, yaw, surge, sway, heave, extra1, extra2, extra3, wind);
                _axisAssignmentsB.ProcessingData(pitch, roll, yaw, surge, sway, heave, extra1, extra2, extra3, wind);

                for (var i = 0; i < 8; i++)
                {
                    _lastAxisA[i] = _axisAssignmentsA.GetAxis(i, ref absValues[i]);
                    _lastAxisB[i] = _axisAssignmentsA.GetAxis(i, ref absValues[i + 8]);
                }

                _lastAxisA[8] = _axisAssignmentsA.GetAxis9();
                _lastAxisB[8] = _axisAssignmentsB.GetAxis9();

                var numbers = new byte[18];
                for (var index = 0; index < 8; ++index)
                {
                    numbers[index] = (byte)(sbyte.MaxValue * _lastAxisA[index] + sbyte.MaxValue);
                    numbers[index + 8] = (byte)(sbyte.MaxValue * _lastAxisB[index] + sbyte.MaxValue);
                }

                numbers[16] = (byte)_lastAxisA[8];
                numbers[17] = (byte)_lastAxisB[8];

                for (var index = 0; index < 16; ++index)
                {
                    if (indAxis[index] < 0)
                    {
                        continue;
                    }

                    var hexBytes = GetHexBytes(numbers[index]);
                    bytes[indAxis[index]] = hexBytes[0];
                    bytes[indAxis[index] + 1] = hexBytes[1];
                }

                if (indAxis[16] >= 0)
                {
                    var decimalBytes = GetDecimalBytes(numbers[16]);
                    bytes[indAxis[16]] = decimalBytes[0];
                    bytes[indAxis[16] + 1] = decimalBytes[1];
                    bytes[indAxis[16] + 2] = decimalBytes[2];
                }

                if (indAxis[17] >= 0)
                {
                    var decimalBytes = GetDecimalBytes(numbers[17]);
                    bytes[indAxis[17]] = decimalBytes[0];
                    bytes[indAxis[17] + 1] = decimalBytes[1];
                    bytes[indAxis[17] + 2] = decimalBytes[2];
                }

                if (SettingsData.isRunning)
                {
                    _sData = Encoding.Default.GetString(bytes);
                    // Debug.Log(_sData);
                }

                try
                {
                    ComPort.Write(bytes);

                    // var str = "";
                    //
                    // for (var index = 0; index < 18; ++index)
                    // {
                    //     str = str + nums[index] + " ";
                    // }
                    //
                    // Debug.Log(str);
                }
                catch
                {
                }

                if (SettingsData.isRunning == false)
                {
                    Thread.Sleep(100);
                    ComPort.Disconnect();
                }

                Thread.Sleep(InterfaceData.interfaceData_msec);
            }
        }).Start();


        private static void GetInterfaceAxisIndex(int[] indAxis, ref string interfaceData)
        {
            for (var index = 0; index < 18; ++index)
            {
                indAxis[index] = -1;
            }

            for (var index = 0; index < interfaceData.Length; ++index)
            {
                if (interfaceData[index] != '<')
                {
                    continue;
                }

                var flags = new bool[18];

                try
                {
                    for (var i = 1; i <= 9; i++)
                    {
                        flags[i - 1] = interfaceData.Substring(index, 9).Contains($"<Motor{i}a>");
                        flags[i + 8] = interfaceData.Substring(index, 9).Contains($"<Motor{i}b>");
                    }

                    for (var index1 = 0; index1 < 18; ++index1)
                    {
                        if (flags[index1])
                        {
                            indAxis[index1] = index;
                        }
                    }

                    var flag1To8And9To16 = false;

                    for (var index1 = 0; index1 < 9; ++index1)
                    {
                        flag1To8And9To16 = flags[index1] | flags[index1 + 9];
                        if (flag1To8And9To16)
                        {
                            break;
                        }
                    }

                    var flag9And16 = flags[8] | flags[16];

                    if (flag1To8And9To16)
                    {
                        interfaceData = interfaceData.Remove(index + 2, 7);
                    }
                    else if (flag9And16)
                    {
                        interfaceData = interfaceData.Remove(index + 3, 6);
                    }
                }
                catch
                {
                }
            }
        }

        private static byte[] GetHexBytes(byte value)
        {
            return Encoding.ASCII.GetBytes(value.ToString("X2"));
        }

        private static byte[] GetDecimalBytes(int value)
        {
            if (value > 999)
            {
                value = 999;
            }

            return Encoding.ASCII.GetBytes(value.ToString("000"));
        }
    }
}