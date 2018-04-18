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
        public MainPage() => this.InitializeComponent();


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

        private double FortyYdToMPH(double seconds)
        {
            int yardsPerMile = 1760;
            return ((40 / seconds) * 3600) / yardsPerMile;
        }

        private double MPHToUnits(double mph)
        {
            double topSpeed = 127;
            return (mph / 32.1) * topSpeed;
        }

        private async void Err(string message)
        {
            MessageDialog popup = new MessageDialog(message);
            await popup.ShowAsync();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double seconds = -1.0;
            try
            {
                seconds = Convert.ToDouble(Speed.Text.ToString());
            } catch (FormatException)
            {
                Err("40 yard dash time must be a number.");
                Speed.Text = "";
                MPH.Text = "";
            }

            if (seconds <= 0)
            {
                Err("40 yard dash time must be greater than zero.");
                Speed.Text = "";
                MPH.Text = "";
            } else
            {
                double mph = FortyYdToMPH(seconds);

                int units = Convert.ToInt32(MPHToUnits(mph));

                QPPS.Text = units.ToString();

                ConnectToSerialPort(units.ToString() + "|");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ConnectToSerialPort("0");
        }

        private void Speed_TextChanged(object sender, TextChangedEventArgs e)
        {
            double seconds = -1.0;
            try
            {
                seconds = Convert.ToDouble(Speed.Text.ToString());
                if (seconds > 0.0)
                {
                    double mph = FortyYdToMPH(seconds);
                    mph = Math.Round(mph, 3);
                    MPH.Text = mph.ToString() + " mph";
                }
                else { MPH.Text = ""; }
            }
            catch (FormatException)
            {
                MPH.Text = "";
            }
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
