using System.Collections.Generic;
using Data;

public static class Settings
{
    public static List<AxisDofData> gameAxes = new();
    public static List<AxisDofData> gameAxes2 = new();
    public static GameSettingsData GameSettingsData;

    public static int shutdownValue = 50;
    public static int windCoefValue = 100;
    public static bool windConst;
    public static bool isRunning;
}