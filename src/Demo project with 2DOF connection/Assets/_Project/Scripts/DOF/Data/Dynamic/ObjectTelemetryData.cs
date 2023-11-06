namespace DOF.Data.Dynamic
{
    public class ObjectTelemetryData
    {
        public double Pitch { get; set; }
        public double Roll { get; set; }
        public double Yaw { get; set; }
        public double Surge { get; set; }
        public double Sway { get; set; }
        public double Heave { get; set; }
        public double Extra1 { get; set; }
        public double Extra2 { get; set; }
        public double Extra3 { get; set; }
        public double Wind { get; set; }
        
        public void Reset()
        {
            Pitch = 0.0;
            Roll = 0.0;
            Yaw = 0.0;
            Surge = 0.0;
            Sway = 0.0;
            Heave = 0.0;
            Wind = 0.0;
            Extra1 = 0.0;
            Extra2 = 0.0;
            Extra3 = 0.0;
        }
    }
}