using System.IO.Ports;

namespace ArduinoSensorDashboard
{
    public class SerialHandler
    {
        private readonly SerialPort _serialPort;

        private SerialHandler()
        {
            _serialPort = new SerialPort();
            _serialPort.PortName = "/dev/cu.usbmodem142101"; //Set your board COM
            _serialPort.BaudRate = 9600;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Open();
        }

        public static SerialHandler Instance { get; } = new();
        
        public void Write(string command)
        {
            _serialPort.Write(command);
        }

        public string Read()
        {
            string result = _serialPort.ReadExisting();
            _serialPort.DiscardInBuffer();

            return result;
        }
    }
}