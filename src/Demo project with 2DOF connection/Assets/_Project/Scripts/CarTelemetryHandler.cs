using UnityEngine;

public class CarTelemetryHandler : MonoBehaviour
{
    [SerializeField] private Transform vehicleTransform;
    private GameTelemetry _telemetryData;
    
    private void Update()
    {
        if (_telemetryData == null)
        {
            return;
        }
        
        var rotation = vehicleTransform.rotation;
        _telemetryData.Pitch = rotation.eulerAngles.x;
        _telemetryData.Roll = rotation.eulerAngles.z;
        _telemetryData.Yaw = rotation.eulerAngles.y;

        var position = vehicleTransform.position;
        _telemetryData.Surge = position.z;
        _telemetryData.Sway = position.x;
        _telemetryData.Heave = position.y;

        _telemetryData.Extra1 = 0.0;
        _telemetryData.Extra2 = 0.0;
        _telemetryData.Extra3 = 0.0;

        _telemetryData.Wind = 0.0;
    }
}