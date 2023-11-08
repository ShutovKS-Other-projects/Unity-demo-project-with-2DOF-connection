using RJCP.IO.Ports;
using UnityEngine;

namespace DOF
{
    public class ComPort
    {
        private readonly SerialPortStream _serialPort;

        public ComPort(int comPortNumber = 3, int baudRate = 115200, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            _serialPort = new SerialPortStream
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
            
            Debug.Log("Connecting to COM port");
            Debug.Log("Is open: " + _serialPort.IsOpen);
        }

        public bool TryConnect()
        {
            try
            {
                _serialPort.Open();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void Disconnect()
        {
            Debug.Log("Disconnecting from COM port");
            try
            {
                if (IsOpen())
                {
                    _serialPort.Close();
                }
            }
            catch { }
        }

        public void Write(string s)
        {
            try
            {
                if (IsOpen())
                {
                    _serialPort.Write(s);
                }
            }
            catch { }
        }

        public void Write(byte[] bytes)
        {
            try
            {
                if (IsOpen())
                {
                    Debug.Log("Serial port is open");
                    _serialPort.Write(bytes, 0, bytes.Length);
                }
                else
                {
                    Debug.Log("Serial port is not open");
                }
            }
            catch { }
        }

        public bool IsOpen()
        {
            try
            {
                return _serialPort.IsOpen;
            }
            catch
            {
                return false;
            }
        }
    }
}