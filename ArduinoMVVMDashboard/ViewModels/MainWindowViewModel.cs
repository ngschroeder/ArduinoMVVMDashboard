using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using ReactiveUI;

namespace ArduinoSensorDashboard.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private bool _counting;
        private int _number;
        private string[] _ports = SerialPort.GetPortNames();
        private string _selectedPort;
        private string _connectBtnColor = "LightGreen";
        private string _connectBtnContent = "Connect";
        private string _connectBtnEnabled = "True";
        private string _distBtnColor = "Gray";
        private string _distBtnContent = "Start";
        private string _distBtnEnabled = "False";
        private string _lcdRow1;
        private string _lcdRow2;
        private string _numberString = "N/A";
        private string _sendBtnColor = "Gray";
        private string _sendBtnEnabled = "False";
        private string _unit = "cm";

        public string[] Ports
        {
            get => _ports;
            set => this.RaiseAndSetIfChanged(ref _ports, value);
        }
        
        public string SelectedPort
        {
            get => _selectedPort;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedPort, value);
            } 
        }

        private bool Counting
        {
            get => _counting;
            set => this.RaiseAndSetIfChanged(ref _counting, value);
        }

        public string NumberString
        {
            get => _numberString + " " + _unit;
            set => this.RaiseAndSetIfChanged(ref _numberString, value);
        }

        public int Number
        {
            get => _number;
            set => this.RaiseAndSetIfChanged(ref _number, value);
        }

        [NotNull]
        public string DistBtnContent
        {
            get => _distBtnContent;
            set => this.RaiseAndSetIfChanged(ref _distBtnContent, value);
        }

        [NotNull]
        public string DistBtnColor
        {
            get => _distBtnColor;
            set => this.RaiseAndSetIfChanged(ref _distBtnColor, value);
        }

        [NotNull]
        public string DistBtnEnabled
        {
            get => _distBtnEnabled;
            set => this.RaiseAndSetIfChanged(ref _distBtnEnabled, value);
        }

        [NotNull]
        public string ConnectBtnContent
        {
            get => _connectBtnContent;
            set => this.RaiseAndSetIfChanged(ref _connectBtnContent, value);
        }

        [NotNull]
        public string ConnectBtnColor
        {
            get => _connectBtnColor;
            set => this.RaiseAndSetIfChanged(ref _connectBtnColor, value);
        }

        [NotNull]
        public string ConnectBtnEnabled
        {
            get => _connectBtnEnabled;
            set => this.RaiseAndSetIfChanged(ref _connectBtnEnabled, value);
        }

        [NotNull]
        public string SendBtnColor
        {
            get => _sendBtnColor;
            set => this.RaiseAndSetIfChanged(ref _sendBtnColor, value);
        }

        [NotNull]
        public string SendBtnEnabled
        {
            get => _sendBtnEnabled;
            set => this.RaiseAndSetIfChanged(ref _sendBtnEnabled, value);
        }
        
        public string LcdRow1
        {
            get => _lcdRow1;
            set => this.RaiseAndSetIfChanged(ref _lcdRow1, value);
        }

        public string LcdRow2
        {
            get => _lcdRow2;
            set => this.RaiseAndSetIfChanged(ref _lcdRow2, value);
        }

        public void ChangeUnit()
        {
            SerialHandler.Instance.Write("changeUnits");
            if (string.Equals(_unit, "cm"))
                _unit = "in";
            else
                _unit = "cm";
        }

        public void ConnectCommand(object sender)
        {
            if (Equals(sender, "Connect"))
            {
                try
                {
                    SerialHandler.Instance.SetPort(SelectedPort);
                    SerialHandler.Instance.Open();
                    SerialHandler.Instance.Write("start");
                    ConnectBtnContent = "Connected";
                    ConnectBtnColor = "Gray";
                    ConnectBtnEnabled = "False";
                    DistBtnEnabled = "True";
                    DistBtnColor = "LightGreen";
                    SendBtnEnabled = "True";
                    SendBtnColor = "LightGreen";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }
        }

        public void DistanceCommand(object sender)
        {
            CancellationTokenSource cancelTokenSource = new();
            var token = cancelTokenSource.Token;

            //Number = _number + 1;
            Console.WriteLine(_counting);
            if (Equals(sender, "Start"))
            {
                SerialHandler.Instance.Write("getDist");
                DistBtnContent = "Stop";
                DistBtnColor = "LightPink";
                Counting = true;
            }
            else
            {
                SerialHandler.Instance.Write("stopDist");
                DistBtnContent = "Start";
                DistBtnColor = "LightGreen";
                NumberString = "N/A";
                Counting = false;
            }

            if (_counting == false)
            {
                cancelTokenSource.Cancel();
            }
            else
            {
                Task task1 = new(async () => await Factorial(this, token));
                task1.Start();
            }
        }

        public void SendBtnCommand()
        {
            string pad1 = "";
            string pad2 = "";
            int remain1;
            int remain2;

            if (LcdRow1 != "")
                remain1 = 16;
            else
                remain1 = 16 - LcdRow1.Length;
            for (var i = 0; i <= remain1; i++) pad1 = pad1 + " ";
            string line1 = "lcd1:" + LcdRow1 + pad1 + "\n";
            SerialHandler.Instance.Write(line1);

            //SerialHandler.Instance.Flush();
            
            if (LcdRow2 != "")
                remain2 = 16;
            else
                remain2 = 16 - LcdRow2.Length;
            for (var i = 0; i <= remain2; i++) pad2 = pad2 + " ";
            string line2 = "lcd2:" + LcdRow2 + pad2 + "\n";
            SerialHandler.Instance.Write(line2);
        }

        private static async Task Factorial(MainWindowViewModel ctx, CancellationToken token)
        {
            while (ctx.Counting)
            {
                if (token.IsCancellationRequested) return;

                string newNum = SerialHandler.Instance.Read();
                if (newNum != "") ctx.NumberString = newNum;

                Thread.Sleep(200);
            }
        }
    }
}