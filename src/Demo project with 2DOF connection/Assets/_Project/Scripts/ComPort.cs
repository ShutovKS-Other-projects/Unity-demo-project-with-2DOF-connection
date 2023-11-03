using System.IO.Ports;

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

        try
        {
            serialPort.Open();
        }
        catch
        {
            return false;
        }

        return true;
    }

    public static void Disconnect()
    {
        try
        {
            if (IsOpen())
            {
                serialPort.Close();
            }
        }
        catch
        {
            // ignored
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
            // ignored
        }
    }

    public static void Write(byte[] bytes)
    {
        try
        {
            if (IsOpen())
            {
                serialPort.Write(bytes, 0, bytes.Length);
            }
        }
        catch
        {
            // ignored
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