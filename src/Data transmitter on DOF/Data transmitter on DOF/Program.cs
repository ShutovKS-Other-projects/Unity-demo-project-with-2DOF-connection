#region

using System.IO.MemoryMappedFiles;
using Test_connected_to_COM_Port;

#endregion

const string MAP_NAME = "2DOF";
const int DATA_SIZE = 12 * sizeof(byte);

var task = new Task(() =>
{
    using var memoryMappedFile = MemoryMappedFile.CreateOrOpen(MAP_NAME, DATA_SIZE);

    while (true)
    {
        using var accessor = memoryMappedFile.CreateViewAccessor();

        var receivedData = new byte[12];

        accessor.ReadArray(0, receivedData, 0, 12);

        ComPort_SerialPortStream.Write(receivedData);

        Thread.Sleep(20);
    }
});

task.Start();

Console.ReadLine();

task.Dispose();