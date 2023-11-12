public class ObjectTelemetryData
{
    public double Pitch
    {
        get => DataArray[0];
        set => DataArray[0] = value;
    }

    public double Roll
    {
        get => DataArray[1];
        set => DataArray[1] = value;
    }

    public double Yaw
    {
        get => DataArray[2];
        set => DataArray[2] = value;
    }

    public double Surge
    {
        get => DataArray[3];
        set => DataArray[3] = value;
    }

    public double Sway
    {
        get => DataArray[4];
        set => DataArray[4] = value;
    }

    public double Heave
    {
        get => DataArray[5];
        set => DataArray[5] = value;
    }

    public double[] DataArray { get; private set; } = new double[6];

    public void Reset()
    {
        Pitch = 0.0;
        Roll = 0.0;
        Yaw = 0.0;
        Surge = 0.0;
        Sway = 0.0;
        Heave = 0.0;
    }
}