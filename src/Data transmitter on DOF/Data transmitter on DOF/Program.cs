#region

using System.IO.MemoryMappedFiles;
using Newtonsoft.Json;
using Test_connected_to_COM_Port.Scripts.Data.Constant;
using Test_connected_to_COM_Port.Scripts.Data.Dynamic;
using Test_connected_to_COM_Port.Scripts.Dispatch;
using Test_connected_to_COM_Port.Scripts.Handler;

#endregion

unsafe
{
    DataProcessingAndTransmission dataProcessingAndTransmission;
    ObjectTelemetryData* objectTelemetryDataLink;
    Thread memoryDataGrabTask;

    ComPort.TryConnect();
    InitializeParameters();
    InitializeMemoryDataGrabTask();

    memoryDataGrabTask.Start();
    dataProcessingAndTransmission.Start();

    Console.ReadLine();

    dataProcessingAndTransmission.Dispose();
    memoryDataGrabTask.Abort();

    return;

    void InitializeParameters()
    {
        var objectTelemetryData = new ObjectTelemetryData();
        
        objectTelemetryDataLink = &objectTelemetryData;
        dataProcessingAndTransmission = new DataProcessingAndTransmission(
            ref objectTelemetryDataLink,
            new AxisAssignments(true),
            new AxisAssignments(false));


        var gameSettingsData = GetData<GameSettingsData>("gameSetting.json");

        var axisDofData1 = GetData<AxisDofData[]>("axisDofDataArray1.json");
        var axisDofData2 = GetData<AxisDofData[]>("axisDofDataArray2.json");
        var axisDofData3 = GetData<AxisDofData[]>("axisDofDataArray3.json");
        var axisDofData4 = GetData<AxisDofData[]>("axisDofDataArray4.json");

        dataProcessingAndTransmission.AxisAssignmentsSetUp(
            axisDofData1, axisDofData2, axisDofData3, axisDofData4,
            gameSettingsData.MinPitch, gameSettingsData.MaxPitch,
            gameSettingsData.MinRoll, gameSettingsData.MaxRoll,
            gameSettingsData.MinYaw, gameSettingsData.MaxYaw,
            gameSettingsData.MinSurge, gameSettingsData.MaxSurge,
            gameSettingsData.MinSway, gameSettingsData.MaxSway,
            gameSettingsData.MinHeave, gameSettingsData.MaxHeave,
            gameSettingsData.MinExtra1, gameSettingsData.MaxExtra1,
            gameSettingsData.MinExtra2, gameSettingsData.MaxExtra2,
            gameSettingsData.MinExtra3, gameSettingsData.MaxExtra3);

        return;

        T? GetData<T>(string fileName)
        {
            return JsonConvert.DeserializeObject<T>(GetPath(fileName));
        }

        string GetPath(string fileName)
        {
            return File.ReadAllText(Path.Combine(DirectoriesPaths.JSON_DATA_PATH, fileName));
        }
    }

    void InitializeMemoryDataGrabTask()
    {
        memoryDataGrabTask = new Thread(MemoryDataGrab);
    }

    void MemoryDataGrab()
    {
        Console.WriteLine($"Memory mapped file created");

        while (true)
        {
            using var memoryMappedFile = MemoryMappedFile.CreateOrOpen(MemoryDataGrabber.MAP_NAME, MemoryDataGrabber.DATA_SIZE);
            using var accessor = memoryMappedFile.CreateViewAccessor();

            var receivedData = new double[MemoryDataGrabber.DATA_COUNT];

            accessor.ReadArray(0, receivedData, 0, MemoryDataGrabber.DATA_COUNT);

            objectTelemetryDataLink->Pitch = receivedData[0];
            objectTelemetryDataLink->Roll = receivedData[1];
            objectTelemetryDataLink->Yaw = receivedData[2];
            objectTelemetryDataLink->Surge = receivedData[3];
            objectTelemetryDataLink->Sway = receivedData[4];
            objectTelemetryDataLink->Heave = receivedData[5];
            
            Thread.Sleep(20);
        }
    }
}