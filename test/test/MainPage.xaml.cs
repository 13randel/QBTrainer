using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace test
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }


        private async void ConnectToSerialPort(string message)
        {
            ushort vid = 0x2A03;
            ushort pid = 0x0042;
            string selector = SerialDevice.GetDeviceSelectorFromUsbVidPid(vid, pid);
            DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(selector);
            if (devices.Count > 0)
            {
                DeviceInformation deviceInfo = devices[0];
                SerialDevice serialDevice = await SerialDevice.FromIdAsync(deviceInfo.Id);
                Debug.WriteLine(serialDevice);
                serialDevice.BaudRate = 57600;
                serialDevice.DataBits = 8;
                serialDevice.StopBits = SerialStopBitCount.Two;
                serialDevice.Parity = SerialParity.None;

                DataWriter dataWriter = new DataWriter(serialDevice.OutputStream);
                dataWriter.WriteString(message);
                await dataWriter.StoreAsync();
                dataWriter.DetachStream();
                dataWriter = null;
                //await serialDevice.OutputStream.FlushAsync();
                serialDevice.OutputStream.Dispose();
            }
            else
            {
                MessageDialog popup = new MessageDialog("Sorry, no device found.");
                await popup.ShowAsync();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int speed = Convert.ToInt32(Speed.Text.ToString());
            if(speed > 127)
            {
                SpeedError.Text = "Speed cannot be greater than 127";
                Speed.Text = "127";
                speed = 127;
            }
            else if(speed < 0)
            {
                SpeedError.Text = "Speed cannot be less than 0";
                Speed.Text = "0";
                speed = 0;
            }
            else
            {
                SpeedError.Text = "";
            }

            if (Reverse.IsChecked == true)
            {
                ConnectToSerialPort("s" + (-1*speed).ToString() + "m" + MotorDelay.ToString() + "l" + LEDDelay.ToString() + "e");
            }
            else
            {
                ConnectToSerialPort("s" + speed.ToString() + "m" + MotorDelay.Text.ToString() + "l" + LEDDelay.Text.ToString() + "e");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ConnectToSerialPort("s0m0l0e");
        }
    }
}
