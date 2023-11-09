using System.IO;
using DOF;
using DOF.Data.Dynamic;
using Newtonsoft.Json;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private CarTelemetryHandler _carTelemetryHandler;
    private ObjectTelemetryData _objectTelemetryData;
    private DataProcessingAndTransmission _dataProcessingAndTransmission;

    private void Start()
    {
        InitializeParameters();
    }

    private void InitializeParameters()
    {
        _objectTelemetryData = new ObjectTelemetryData();

        _carTelemetryHandler.SetObjectTelemetryData(_objectTelemetryData);

        _dataProcessingAndTransmission = new DataProcessingAndTransmission(
            _objectTelemetryData,
            new AxisAssignments(true),
            new AxisAssignments(false));
        
        var inputDirectory = @"C:\Users\ShutovKS\Documents\projects\Unity-demo-project-with-2DOF-connection\src\Demo project with 2DOF connection\Assets\_Project\Data\Json";

        var gameSettingsData = JsonConvert.DeserializeObject<GameSettingsData>(File.ReadAllText(Path.Combine(inputDirectory, "gameSetting.json")));

        var axisDofData1 = JsonConvert.DeserializeObject<AxisDofData[]>(File.ReadAllText(Path.Combine(inputDirectory, "axisDofDataArray1.json")));
        var axisDofData2 = JsonConvert.DeserializeObject<AxisDofData[]>(File.ReadAllText(Path.Combine(inputDirectory, "axisDofDataArray2.json")));
        var axisDofData3 = JsonConvert.DeserializeObject<AxisDofData[]>(File.ReadAllText(Path.Combine(inputDirectory, "axisDofDataArray3.json")));
        var axisDofData4 = JsonConvert.DeserializeObject<AxisDofData[]>(File.ReadAllText(Path.Combine(inputDirectory, "axisDofDataArray4.json")));

        _dataProcessingAndTransmission.AxisAssignmentsSetUp(
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
        
        _dataProcessingAndTransmission.Start();
    }
}