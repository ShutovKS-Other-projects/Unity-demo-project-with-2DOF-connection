#region

using System.IO.Ports;
using System.Text;

#endregion

namespace Test_connected_to_COM_Port.Scripts.Dispatch;

public static class ComPort
{
    private static SerialPort serialPort;

    public static bool TryConnect(int comPortNumber = 3, int baudRate = 115200, int dataBits = 8,
        StopBits stopBits = StopBits.One)
    {
        serialPort = new SerialPort
        {
            BaudRate = baudRate,
            DataBits = dataBits,
            Parity = Parity.None,
            StopBits = stopBits,
            ReadBufferSize = 4096,
            WriteBufferSize = 4096,
            ReadTimeout = 200,
            PortName = "COM" + comPortNumber
        };

        Console.WriteLine("ComPort_SerialPort, Connecting to COM port");

        try
        {
            serialPort.Open();
            Console.WriteLine("true");
        }
        catch
        {
            Console.WriteLine("false");
            return false;
        }

        return true;
    }

    public static void Disconnect()
    {
        Console.WriteLine("Disconnecting from COM port");
        try
        {
            if (IsOpen())
            {
                serialPort.Close();
            }
        }
        catch
        {
        }
    }

    public static void Write(string s)
    {
        try
        {
            if (IsOpen())
            {
                serialPort.Write(s);
            }
        }
        catch
        {
        }
    }

    public static void Write(byte[] bytes)
    {
        // Console.Clear();
        Console.WriteLine(Encoding.Default.GetString(bytes));
        
        try
        {
            if (IsOpen())
            {
                serialPort.Write(bytes, 0, bytes.Length);
            }
            else
            {
                // Console.WriteLine("Serial port is not open");
            }
        }
        catch
        {
        }
    }

    public static bool IsOpen()
    {
        try
        {
            return serialPort.IsOpen;
        }
        catch
        {
            return false;
        }
    }
}