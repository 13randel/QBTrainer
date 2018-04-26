using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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
//using ModbusTCP;
using EasyModbus;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace test
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //      private ModbusTCP.Master MBmaster;
        ModbusClient modbusClient = new ModbusClient();
        private int endDelay;
        private int startDelay;
        private SerialDevice Arduino;
        private DataWriter ArduinodataWriter;
        private bool load = false;
        public MainPage()
        {
            this.InitializeComponent();   
        }

        private async void awaitProxInput()
        {
            while (modbusClient.ReadCoils(4, 1)[0] != true)
            {
                // don't run again for at least 200 milliseconds
                await Task.Delay(100);
            }
            ReadToSerialPort("201|0|");

        }

        private async void ConnectToClickPLC2()
        {
            try
            {
                modbusClient = new ModbusClient("169.254.38.41", 502);
                //Then connect to the specified server
                modbusClient.Connect();

                // Create new modbus master and add event functions
                //          MBmaster = new Master("169.254.38.41", 502);
                //MBmaster.OnResponseData += new ModbusTCP.Master.ResponseData(MBmaster_OnResponseData);
                //MBmaster.OnException += new ModbusTCP.Master.ExceptionData(MBmaster_OnException);
                // Show additional fields, enable watchdog
            }
            catch (Exception error)
            {
                MessageDialog popup = new MessageDialog("PLC not Connected");
                await popup.ShowAsync();
            }
            
        }


        private async void ReadToSerialPort(string message)
        {

            if (load)
            {
                awaitProxInput();
                ArduinodataWriter.WriteString(message);
                await ArduinodataWriter.StoreAsync();
            }
            else
            {
                MessageDialog popup = new MessageDialog("Sorry, Devices need to be loaded");
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

                ReadToSerialPort(units.ToString() + "|" + LEDNUMBER.Text + "|");
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (load)
            {
                awaitProxInput();
                ReadToSerialPort("200|0|");
            }
            else
            {
                MessageDialog popup = new MessageDialog("Sorry, Devices need to be loaded");
                await popup.ShowAsync();
            }
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

        private async void ConnectToSerialPort(object sender, RoutedEventArgs e)
        {
            if (!load && Convert.ToInt32(LEDNUMBER.Text) > 0)
            {
                ushort vid = 0x2A03;
                ushort pid = 0x0042;

                string selector = SerialDevice.GetDeviceSelectorFromUsbVidPid(vid, pid);
                DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(selector);
                if (devices.Count > 0)
                {
                    DeviceInformation deviceInfo = devices[0];
                    Arduino = await SerialDevice.FromIdAsync(deviceInfo.Id);
                    Debug.WriteLine(Arduino);
                    Arduino.BaudRate = 57600;
                    Arduino.DataBits = 8;
                    Arduino.StopBits = SerialStopBitCount.Two;
                    Arduino.Parity = SerialParity.None;
                    ArduinodataWriter = new DataWriter(Arduino.OutputStream);
                    ConnectToClickPLC2();
                    ArduinodataWriter.WriteString("0|" + LEDNUMBER.Text + "| ");
                    await ArduinodataWriter.StoreAsync();
                }
                else
                {
                    MessageDialog popup = new MessageDialog("Sorry, no device found.");
                    await popup.ShowAsync();
                }
                load = true;
            }
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (load)
            {
                ReadToSerialPort("201|0|");
            }
            else
            {
                MessageDialog popup = new MessageDialog("Sorry, Devices need to be loaded");
                await popup.ShowAsync();
            }
        }
    }
}
