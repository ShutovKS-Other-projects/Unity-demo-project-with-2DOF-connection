public class ObjectTelemetryData
{
    public double Pitch { get; set; }
    public double Roll { get; set; }
    public double Yaw { get; set; }
    public double Surge { get; set; }
    public double Sway { get; set; }
    public double Heave { get; set; }
    
    public double[] DataArray => new[] {Pitch, Roll, Yaw, Surge, Sway, Heave};

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