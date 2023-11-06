using DOF.Data;
using DOF.Data.Dynamic;
using UnityEngine;

public class CarTelemetryHandler : MonoBehaviour
{
    [SerializeField] private Transform vehicleTransform;
    private ObjectTelemetryData _telemetryDataData;
    
    private void Update()
    {
        if (_telemetryDataData == null)
        {
            return;
        }
        
        var rotation = vehicleTransform.rotation;
        _telemetryDataData.Pitch = rotation.eulerAngles.x;
        _telemetryDataData.Roll = rotation.eulerAngles.z;
        _telemetryDataData.Yaw = rotation.eulerAngles.y;

        var position = vehicleTransform.position;
        _telemetryDataData.Surge = position.z;
        _telemetryDataData.Sway = position.x;
        _telemetryDataData.Heave = position.y;

        _telemetryDataData.Extra1 = 0.0;
        _telemetryDataData.Extra2 = 0.0;
        _telemetryDataData.Extra3 = 0.0;

        _telemetryDataData.Wind = 0.0;
    }
}