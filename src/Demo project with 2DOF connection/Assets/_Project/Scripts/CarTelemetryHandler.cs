#region

using DOF;
using DOF.Data;
using UnityEngine;

#endregion

public class CarTelemetryHandler : MonoBehaviour
{
    [SerializeField] private Transform vehicleTransform;
    private readonly GameTelemetry _telemetryData = new();
    private SerialInterface _serialInterface;

    private void Start()
    {
        _serialInterface = new SerialInterface(InterfaceSerialData.Default, _telemetryData);
    }

    private void Update()
    {
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