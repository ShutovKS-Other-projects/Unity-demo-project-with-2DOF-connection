#region

using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

#endregion

public class CarTelemetryHandler : MonoBehaviour
{
    [SerializeField] private Transform vehicleTransform;
    [SerializeField] private Rigidbody _rigidbody;
    private ObjectTelemetryData _telemetryDataData;

    private void Start()
    {
        StartCoroutine(TelemetryHandler());
    }

    private IEnumerator TelemetryHandler()
    {
        while (true)
        {
            if (_telemetryDataData == null)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            var rotation = vehicleTransform.rotation;
            _telemetryDataData.Pitch = rotation.eulerAngles.x > 180
                ? rotation.eulerAngles.x - 360
                : rotation.eulerAngles.x;
            _telemetryDataData.Roll = rotation.eulerAngles.z > 180
                ? rotation.eulerAngles.z - 360
                : rotation.eulerAngles.z;
            _telemetryDataData.Yaw = rotation.eulerAngles.y > 180
                ? rotation.eulerAngles.y - 360
                : rotation.eulerAngles.y;
            
            var velocity = _rigidbody.velocity;
            _telemetryDataData.Surge = velocity.magnitude;
            _telemetryDataData.Sway = velocity.x;
            _telemetryDataData.Heave = velocity.y;
            
            yield return null;
        }
    }

    public void SetObjectTelemetryData(ObjectTelemetryData objectTelemetryData)
    {
        _telemetryDataData = objectTelemetryData;
    }
}