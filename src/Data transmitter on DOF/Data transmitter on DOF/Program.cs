#region

using System.IO.MemoryMappedFiles;
using DataTransmitterOnDOF.Data.Constant;
using DataTransmitterOnDOF.Data.Dynamic;
using DataTransmitterOnDOF.Dispatch;
using DataTransmitterOnDOF.Handler;
using Newtonsoft.Json;

#endregion

unsafe
{
    DataProcessingAndTransmission dataProcessingAndTransmission;
    ObjectTelemetryData* objectTelemetryDataLink;

    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    InitializeParameters();

    var memoryDataGrabTask = new Task(() => MemoryDataGrab(), TaskCreationOptions.LongRunning);
    var transmitTelemetryDataTask = new Task(() => dataProcessingAndTransmission.TransmitTelemetryData(), TaskCreationOptions.LongRunning);

    while (true)
    {
        Console.WriteLine("Введите номер COM порта (по умолчанию 3):");
        var comPortNumber = Console.ReadLine();

        if (comPortNumber == string.Empty)
        {
            comPortNumber = "3";
        }

        if (int.TryParse(comPortNumber, out var comPortNumberInt) == false)
        {
            Console.Clear();
            Console.WriteLine("Неверный ввод");
            continue;
        }

        if (ComPort.TryConnect(comPortNumberInt) == false)
        {
            Console.Clear();
            Console.WriteLine("Не удалось подключиться к COM порту");
            continue;
        }
        else
        {
            Console.WriteLine($"Подключение к COM порту {comPortNumberInt} установлено");
            break;
        }
    }
   
    memoryDataGrabTask.Start();
    transmitTelemetryDataTask.Start();
   
    while (true)
    {
        var key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.Escape)
        {
            cancellationTokenSource.Cancel();
            break;
        }
    }

    Task.WaitAll(memoryDataGrabTask, transmitTelemetryDataTask);

    dataProcessingAndTransmission.Dispose();

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
            return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DirectoriesPaths.JSON_DATA_PATH, fileName));
        }
    }

    void MemoryDataGrab()
    {
        Console.WriteLine($"Memory mapped file created");

        while (true)
        {
            if (ComPort.IsOpen() == false)
            {
                Thread.Sleep(1000);
                continue;
            }

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

static void ClearCurrentConsoleLine()
{
    int currentLineCursor = Console.CursorTop;
    Console.SetCursorPosition(0, Console.CursorTop);
    Console.Write(new string(' ', Console.WindowWidth));
    Console.SetCursorPosition(0, currentLineCursor);
}