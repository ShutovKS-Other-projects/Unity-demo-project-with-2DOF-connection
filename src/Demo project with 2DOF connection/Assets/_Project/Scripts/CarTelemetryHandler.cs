using DOF.Data;
using UnityEngine;

public class CarTelemetryHandler : MonoBehaviour
{
    public GameTelemetry telemetryData;
    public Transform vehicleTransform;

    public float updateInterval = 0.1f;

    private void Start()
    {
        telemetryData = new GameTelemetry();
    }

    private void Update()
    {
        var rotation = vehicleTransform.rotation;
        telemetryData.Pitch = rotation.eulerAngles.x;
        telemetryData.Roll = rotation.eulerAngles.z;
        telemetryData.Yaw = rotation.eulerAngles.y;

        var position = vehicleTransform.position;
        telemetryData.Surge = position.z;
        telemetryData.Sway = position.x;
        telemetryData.Heave = position.y;

        telemetryData.Extra1 = 0.0;
        telemetryData.Extra2 = 0.0;
        telemetryData.Extra3 = 0.0;

        telemetryData.Wind = 0.0;
    }
}