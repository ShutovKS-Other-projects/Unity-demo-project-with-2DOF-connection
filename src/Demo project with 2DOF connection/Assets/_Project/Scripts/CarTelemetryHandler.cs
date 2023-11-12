#region

using UnityEngine;

#endregion

public class CarTelemetryHandler : MonoBehaviour
{
    [SerializeField] private Transform vehicleTransform;
    private ObjectTelemetryData _telemetryDataData;

    private void FixedUpdate()
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
    }

    public void SetObjectTelemetryData(ObjectTelemetryData objectTelemetryData)
    {
        _telemetryDataData = objectTelemetryData;
    }
}