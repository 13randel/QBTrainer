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
        private int endDelay;
        private int startDelay;
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

        private void CalculateLEDDelays(double seconds)
        {
            double distanceBetweenLEDSinches = 2.34;
            int yardsPerMile = 1760;
            int inchesPerYard = 36;
            double mph = ((40 / seconds) * 3600) / yardsPerMile;
            double yps = (40 / seconds);
            double ips = ((40 / seconds)) * 36;
            double ipms = (40 / (seconds*1000)) * 36;
            double tmp = distanceBetweenLEDSinches/ ipms;
            endDelay = Convert.ToInt32(Math.Round(distanceBetweenLEDSinches / ipms, 0, MidpointRounding.AwayFromZero));
            double accel = (yps - 0) / (seconds - 0);
            double accel2 = (ipms - 0) / ((seconds*1000) - 0);
            double tmp2 = distanceBetweenLEDSinches / accel2;
        }

        private double LengthToLEDCount(double length)
        {
            double distanceBetweenLEDSinches = 2.34;
            return (((length * 36) / 2.34) + 1);
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

                ConnectToSerialPort(units.ToString() + "|" + Length.Text + "|");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ConnectToSerialPort("0|0|");
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
                    CalculateLEDDelays(seconds);
                }
                else { MPH.Text = ""; }
            }
            catch (FormatException)
            {
                MPH.Text = "";
            }
        }

        private void Length_TextChanged(object sender, TextChangedEventArgs e)
        {
            double length = -1.0;
            try
            {
                length = Convert.ToDouble(Length.Text.ToString());
                if (length > 0.0)
                {
                    double number = LengthToLEDCount(length);
                    number = Math.Round(number, 0, MidpointRounding.AwayFromZero);
                    LEDNUMBER.Text = number.ToString();
                    if(number > 500)
                    {
                        LengthError.Text = "Max Length is 32.45yards or 500 LEDS";
                    }
                    else
                    {
                        LengthError.Text = "";
                    }
                }
                else { LEDNUMBER.Text = ""; }
            }
            catch (FormatException)
            {
                LEDNUMBER.Text = "";
            }
        }
    }
}
