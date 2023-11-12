#region

using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

#endregion

public class GameController : MonoBehaviour
{
    private const string MAP_NAME = "2DOFMemoryDataGrabber";
    private ObjectTelemetryData _objectTelemetryData;
    private Task _handlerDataThread;

    [SerializeField] private CarTelemetryHandler _carTelemetryHandler;

    private void Awake()
    {
        InitializeParameters();
        Task.Run(HandlerData);
    }

    private void InitializeParameters()
    {
        _objectTelemetryData = new ObjectTelemetryData();
        _carTelemetryHandler.SetObjectTelemetryData(_objectTelemetryData);
    }

    private void HandlerData()
    {
        const int WAIT_TIME = 20;

        while (true)
        {
            using var memoryMappedFile = MemoryMappedFile.OpenExisting(MAP_NAME);
            using var accessor = memoryMappedFile.CreateViewAccessor();

            accessor.WriteArray(0, _objectTelemetryData.DataArray, 0, 6);

            Thread.Sleep(WAIT_TIME);
        }
    }
}