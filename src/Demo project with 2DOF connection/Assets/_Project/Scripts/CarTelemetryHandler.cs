#region

using DOF;
using DOF.Data;
using UnityEngine;

#endregion

public class CarTelemetryHandler : MonoBehaviour
{
    [SerializeField] private Transform vehicleTransform;
    private readonly GameTelemetry _gameTelemetry = new();
    private SerialInterface _serialInterface;

    private void Start()
    {
        _serialInterface = new SerialInterface(Settings.InterfaceSerialDatas[0], _gameTelemetry);
        _serialInterface.SetUp(true);
    }

    private void Update()
    {
        var rotation = vehicleTransform.rotation;
        _gameTelemetry.Pitch = rotation.eulerAngles.x;
        _gameTelemetry.Roll = rotation.eulerAngles.z;
        _gameTelemetry.Yaw = rotation.eulerAngles.y;

        var position = vehicleTransform.position;
        _gameTelemetry.Surge = position.z;
        _gameTelemetry.Sway = position.x;
        _gameTelemetry.Heave = position.y;

        _gameTelemetry.Extra1 = 0.0;
        _gameTelemetry.Extra2 = 0.0;
        _gameTelemetry.Extra3 = 0.0;

        _gameTelemetry.Wind = 0.0;
    }
}