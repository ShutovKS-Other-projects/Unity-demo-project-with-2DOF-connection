using System.Net;
using System.Net.Sockets;
using System.Text;
using Test_connected_to_COM_Port;

var port = 12345;
var ipAddress = IPAddress.Parse("127.0.0.1");
var listener = new TcpListener(ipAddress, port);
listener.Start();

Console.WriteLine();

Console.WriteLine(ComPort_SerialPortStream.TryConnect());

var task = new Task(async () =>
{
    while (true)
    {
        var client = await listener.AcceptTcpClientAsync();
        
        await using (var stream = client.GetStream())
        {
            var receivedData = new byte[1024];
            var bytesRead = await stream.ReadAsync(receivedData);
            var actualData = new byte[bytesRead];
            Array.Copy(receivedData, actualData, bytesRead);
            ComPort_SerialPortStream.Write(actualData);
            var receivedMessage = Encoding.UTF8.GetString(actualData);
            Console.WriteLine("Полученные данные: " + receivedMessage);
        }

        client.Close();
        Thread.Sleep(20);
    }
});

task.Start();

Console.ReadLine();

task.Dispose();