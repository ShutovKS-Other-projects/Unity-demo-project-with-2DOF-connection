using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DOF.Data;
using UnityEngine;

namespace DOF
{
    public static class Settings
    {
        static Settings()
        {
            Debug.Log("Construct Settings");
            LoadDefaultData();
        }
        
        public static bool IsRunning = true;
        public static GameSettingsData GameSettings;
        public static AxisGameData AxisGameData1;
        public static AxisGameData AxisGameData2;
        public static List<AxisDofData> AxisDofData1;
        public static List<AxisDofData> AxisDofData2;
        public static List<InterfaceSerialData> InterfaceSerialDatas;
        public static int ShutdownValue = 50;
        public static int WindCoefValue = 100;
        public static bool PointSeparator = false;
        public static bool WindConst;

        public static List<RedirectData> RedirectDatas = new();

        public static void LoadDefaultData()
        {
            InterfaceSerialDatas = new List<InterfaceSerialData>();
            for (var index = 0; index < 8; ++index)
            {
                InterfaceSerialDatas.Add(new InterfaceSerialData());
            }

            AxisDofData1 = new List<AxisDofData>();
            for (var index = 0; index < 48; ++index)
            {
                AxisDofData1.Add(new AxisDofData(0));
                AxisDofData1.Add(new AxisDofData(1));
            }

            AxisDofData2 = new List<AxisDofData>();
            for (var index = 0; index < 48; ++index)
            {
                AxisDofData2.Add(new AxisDofData(0));
                AxisDofData2.Add(new AxisDofData(1));
            }

            AxisGameData1 = new AxisGameData(0, 4123, 0, 1);
            AxisGameData2 = new AxisGameData(1, 4123, 0, 1);
        }

        public static void LoadGameSetting(string fileSettings)
        {
            if (!File.Exists(fileSettings))
            {
                return;
            }

            using Stream serializationStream = File.Open(fileSettings, FileMode.Open);
            try
            {
                var binaryFormatter = new BinaryFormatter();
                var interfaces = (List<InterfaceSerialData>)binaryFormatter.Deserialize(serializationStream);
                var gameAxes = (List<AxisDofData>)binaryFormatter.Deserialize(serializationStream);
                var gameDatas = (List<AxisGameData>)binaryFormatter.Deserialize(serializationStream);
                var shutdownValue = (int)binaryFormatter.Deserialize(serializationStream);
                var modelPlatform = (byte)binaryFormatter.Deserialize(serializationStream);
                var redirectPort = (int)binaryFormatter.Deserialize(serializationStream);
                var windCoefValue = (int)binaryFormatter.Deserialize(serializationStream);
                var windFromGame = (bool)binaryFormatter.Deserialize(serializationStream);
                var windConst = (bool)binaryFormatter.Deserialize(serializationStream);
                var gameAxes2 = (List<AxisDofData>)binaryFormatter.Deserialize(serializationStream);
                var listenLocalPackets = (bool)binaryFormatter.Deserialize(serializationStream);
                var redirectDatas = (List<RedirectData>)binaryFormatter.Deserialize(serializationStream);
            }
            catch
            {
            }
        }

        public static GameSettingsData LoadGameSettingsMinMax(string pathMaxMin)
        {
            var maxRoll = 0.0;
            var minRoll = 0.0;
            var maxPitch = 0.0;
            var minPitch = 0.0;
            var maxYaw = 0.0;
            var minYaw = 0.0;
            var maxHeave = 0.0;
            var minHeave = 0.0;
            var maxSway = 0.0;
            var minSway = 0.0;
            var maxSurge = 0.0;
            var minSurge = 0.0;
            var maxExtra1 = 0.0;
            var minExtra1 = 0.0;
            var maxExtra2 = 0.0;
            var minExtra2 = 0.0;
            var maxExtra3 = 0.0;
            var minExtra3 = 0.0;

            if (File.ReadAllText(pathMaxMin).Contains("<_RollMax>"))
            {
                foreach (var readAllLine in File.ReadAllLines(pathMaxMin))
                {
                    var num1 = readAllLine.IndexOf("<_RollMax>", StringComparison.Ordinal);
                    if (num1 != -1)
                    {
                        var num2 = readAllLine.IndexOf("</_RollMax>", StringComparison.Ordinal);
                        maxRoll = Convert.ToDouble(!PointSeparator
                            ? readAllLine.Substring(num1 + 10, num2 - num1 - 10).Trim().Replace('.', ',')
                            : readAllLine.Substring(num1 + 10, num2 - num1 - 10).Trim().Replace(',', '.'));
                        continue;
                    }

                    var num3 = readAllLine.IndexOf("<_RollMin>", StringComparison.Ordinal);
                    if (num3 != -1)
                    {
                        var num4 = readAllLine.IndexOf("</_RollMin>", StringComparison.Ordinal);
                        minRoll = Convert.ToDouble(!PointSeparator
                            ? readAllLine.Substring(num3 + 10, num4 - num3 - 10).Trim().Replace('.', ',')
                            : readAllLine.Substring(num3 + 10, num4 - num3 - 10).Trim().Replace(',', '.'));
                        continue;
                    }

                    var num5 = readAllLine.IndexOf("<_PitchMax>", StringComparison.Ordinal);
                    if (num5 != -1)
                    {
                        var num6 = readAllLine.IndexOf("</_PitchMax>", StringComparison.Ordinal);
                        maxPitch = Convert.ToDouble(!PointSeparator
                            ? readAllLine.Substring(num5 + 11, num6 - num5 - 11).Trim().Replace('.', ',')
                            : readAllLine.Substring(num5 + 11, num6 - num5 - 11).Trim().Replace(',', '.'));
                        continue;
                    }

                    var num7 = readAllLine.IndexOf("<_PitchMin>", StringComparison.Ordinal);
                    if (num7 != -1)
                    {
                        var num8 = readAllLine.IndexOf("</_PitchMin>", StringComparison.Ordinal);
                        minPitch = Convert.ToDouble(!PointSeparator
                            ? readAllLine.Substring(num7 + 11, num8 - num7 - 11).Trim().Replace('.', ',')
                            : readAllLine.Substring(num7 + 11, num8 - num7 - 11).Trim().Replace(',', '.'));
                        continue;
                    }

                    var num9 = readAllLine.IndexOf("<_HeaveMax>", StringComparison.Ordinal);
                    if (num9 != -1)
                    {
                        var num10 = readAllLine.IndexOf("</_HeaveMax>", StringComparison.Ordinal);
                        maxHeave = Convert.ToDouble(!PointSeparator
                            ? readAllLine.Substring(num9 + 11, num10 - num9 - 11).Trim().Replace('.', ',')
                            : readAllLine.Substring(num9 + 11, num10 - num9 - 11).Trim().Replace(',', '.'));
                        continue;
                    }

                    var num11 = readAllLine.IndexOf("<_HeaveMin>", StringComparison.Ordinal);
                    if (num11 != -1)
                    {
                        var num12 = readAllLine.IndexOf("</_HeaveMin>", StringComparison.Ordinal);
                        minHeave = Convert.ToDouble(!PointSeparator
                            ? readAllLine.Substring(num11 + 11, num12 - num11 - 11).Trim().Replace('.', ',')
                            : readAllLine.Substring(num11 + 11, num12 - num11 - 11).Trim().Replace(',', '.'));
                        continue;
                    }

                    var num13 = readAllLine.IndexOf("<_YawMax>", StringComparison.Ordinal);
                    if (num13 != -1)
                    {
                        var num14 = readAllLine.IndexOf("</_YawMax>", StringComparison.Ordinal);
                        maxYaw = Convert.ToDouble(!PointSeparator
                            ? readAllLine.Substring(num13 + 9, num14 - num13 - 9).Trim().Replace('.', ',')
                            : readAllLine.Substring(num13 + 9, num14 - num13 - 9).Trim().Replace(',', '.'));
                        continue;
                    }

                    var num15 = readAllLine.IndexOf("<_YawMin>", StringComparison.Ordinal);
                    if (num15 != -1)
                    {
                        var num16 = readAllLine.IndexOf("</_YawMin>", StringComparison.Ordinal);
                        minYaw = Convert.ToDouble(!PointSeparator
                            ? readAllLine.Substring(num15 + 9, num16 - num15 - 9).Trim().Replace('.', ',')
                            : readAllLine.Substring(num15 + 9, num16 - num15 - 9).Trim().Replace(',', '.'));
                        continue;
                    }

                    var num17 = readAllLine.IndexOf("<_SwayMax>", StringComparison.Ordinal);
                    if (num17 != -1)
                    {
                        var num18 = readAllLine.IndexOf("</_SwayMax>", StringComparison.Ordinal);
                        maxSway = Convert.ToDouble(!PointSeparator
                            ? readAllLine.Substring(num17 + 10, num18 - num17 - 10).Trim().Replace('.', ',')
                            : readAllLine.Substring(num17 + 10, num18 - num17 - 10).Trim().Replace(',', '.'));
                        continue;
                    }

                    var num19 = readAllLine.IndexOf("<_SwayMin>", StringComparison.Ordinal);
                    if (num19 != -1)
                    {
                        var num20 = readAllLine.IndexOf("</_SwayMin>", StringComparison.Ordinal);
                        minSway = Convert.ToDouble(!PointSeparator
                            ? readAllLine.Substring(num19 + 10, num20 - num19 - 10).Trim().Replace('.', ',')
                            : readAllLine.Substring(num19 + 10, num20 - num19 - 10).Trim().Replace(',', '.'));
                        continue;
                    }

                    var num21 = readAllLine.IndexOf("<_SurgeMax>", StringComparison.Ordinal);
                    if (num21 != -1)
                    {
                        var num22 = readAllLine.IndexOf("</_SurgeMax>", StringComparison.Ordinal);
                        maxSurge = Convert.ToDouble(!PointSeparator
                            ? readAllLine.Substring(num21 + 11, num22 - num21 - 11).Trim().Replace('.', ',')
                            : readAllLine.Substring(num21 + 11, num22 - num21 - 11).Trim().Replace(',', '.'));
                        continue;
                    }

                    var num23 = readAllLine.IndexOf("<_SurgeMin>", StringComparison.Ordinal);
                    if (num23 != -1)
                    {
                        var num24 = readAllLine.IndexOf("</_SurgeMin>", StringComparison.Ordinal);
                        minSurge = Convert.ToDouble(!PointSeparator
                            ? readAllLine.Substring(num23 + 11, num24 - num23 - 11).Trim().Replace('.', ',')
                            : readAllLine.Substring(num23 + 11, num24 - num23 - 11).Trim().Replace(',', '.'));
                        continue;
                    }

                    var num25 = readAllLine.IndexOf("<_Extra1Max>", StringComparison.Ordinal);
                    if (num25 != -1)
                    {
                        var num26 = readAllLine.IndexOf("</_Extra1Max>", StringComparison.Ordinal);
                        maxExtra1 = Convert.ToDouble(!PointSeparator
                            ? readAllLine.Substring(num25 + 12, num26 - num25 - 12).Trim().Replace('.', ',')
                            : readAllLine.Substring(num25 + 12, num26 - num25 - 12).Trim().Replace(',', '.'));
                        continue;
                    }

                    var num27 = readAllLine.IndexOf("<_Extra1Min>", StringComparison.Ordinal);
                    if (num27 != -1)
                    {
                        var num28 = readAllLine.IndexOf("</_Extra1Min>", StringComparison.Ordinal);
                        minExtra1 = Convert.ToDouble(!PointSeparator
                            ? readAllLine.Substring(num27 + 12, num28 - num27 - 12).Trim().Replace('.', ',')
                            : readAllLine.Substring(num27 + 12, num28 - num27 - 12).Trim().Replace(',', '.'));
                        continue;
                    }

                    var num29 = readAllLine.IndexOf("<_Extra2Max>", StringComparison.Ordinal);
                    if (num29 != -1)
                    {
                        var num30 = readAllLine.IndexOf("</_Extra2Max>", StringComparison.Ordinal);
                        maxExtra2 = Convert.ToDouble(!PointSeparator
                            ? readAllLine.Substring(num29 + 12, num30 - num29 - 12).Trim().Replace('.', ',')
                            : readAllLine.Substring(num29 + 12, num30 - num29 - 12).Trim().Replace(',', '.'));
                        continue;
                    }

                    var num31 = readAllLine.IndexOf("<_Extra2Min>", StringComparison.Ordinal);
                    if (num31 != -1)
                    {
                        var num32 = readAllLine.IndexOf("</_Extra2Min>", StringComparison.Ordinal);
                        minExtra2 = Convert.ToDouble(!PointSeparator
                            ? readAllLine.Substring(num31 + 12, num32 - num31 - 12).Trim().Replace('.', ',')
                            : readAllLine.Substring(num31 + 12, num32 - num31 - 12).Trim().Replace(',', '.'));
                        continue;
                    }

                    var num33 = readAllLine.IndexOf("<_Extra3Max>", StringComparison.Ordinal);
                    if (num33 != -1)
                    {
                        var num34 = readAllLine.IndexOf("</_Extra3Max>", StringComparison.Ordinal);
                        maxExtra3 = Convert.ToDouble(!PointSeparator
                            ? readAllLine.Substring(num33 + 12, num34 - num33 - 12).Trim().Replace('.', ',')
                            : readAllLine.Substring(num33 + 12, num34 - num33 - 12).Trim().Replace(',', '.'));
                        continue;
                    }

                    var num35 = readAllLine.IndexOf("<_Extra3Min>", StringComparison.Ordinal);
                    if (num35 != -1)
                    {
                        var num36 = readAllLine.IndexOf("</_Extra3Min>", StringComparison.Ordinal);
                        minExtra3 = Convert.ToDouble(!PointSeparator
                            ? readAllLine.Substring(num35 + 12, num36 - num35 - 12).Trim().Replace('.', ',')
                            : readAllLine.Substring(num35 + 12, num36 - num35 - 12).Trim().Replace(',', '.'));
                        continue;
                    }
                }
            }
            else
            {
                var binaryReader = new BinaryReader(new FileStream(pathMaxMin, FileMode.Open));
                maxRoll = binaryReader.ReadDouble();
                minRoll = binaryReader.ReadDouble();
                maxPitch = binaryReader.ReadDouble();
                minPitch = binaryReader.ReadDouble();
                maxHeave = binaryReader.ReadDouble();
                minHeave = binaryReader.ReadDouble();
                maxYaw = binaryReader.ReadDouble();
                minYaw = binaryReader.ReadDouble();
                maxSway = binaryReader.ReadDouble();
                minSway = binaryReader.ReadDouble();
                maxSurge = binaryReader.ReadDouble();
                minSurge = binaryReader.ReadDouble();
                maxExtra1 = binaryReader.ReadDouble();
                minExtra1 = binaryReader.ReadDouble();
                maxExtra2 = binaryReader.ReadDouble();
                minExtra2 = binaryReader.ReadDouble();
                maxExtra3 = binaryReader.ReadDouble();
                minExtra3 = binaryReader.ReadDouble();
                binaryReader.Close();
            }

            return new GameSettingsData(maxRoll, minRoll, maxPitch, minPitch, maxYaw,
                minYaw, maxHeave, minHeave, maxSway, minSway, maxSurge, minSurge, maxExtra1, minExtra1, maxExtra2,
                minExtra2, maxExtra3, minExtra3);
        }
    }
}