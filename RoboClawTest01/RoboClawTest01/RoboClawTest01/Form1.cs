using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RoboclawClassLib;

namespace RoboClawTest01
{
    public partial class Form1 : Form
    {
        private Byte speed = 0;
        private Roboclaw roboClaw;
        private string roboClawModel;
        int m1_count, m2_count;
        bool encoderWatch = false;

        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();

        public Form1()
        {
            InitializeComponent();
            roboClaw = new Roboclaw();

            myTimer.Tick += new EventHandler(TimerEventProcessor);  // Timer event and handler
            myTimer.Interval = 25; // Timer interval is 25 milliseconds
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (!roboClaw.IsOpen())
            {
                roboClaw.Open("AUTO", ref roboClawModel, 128, 38400); // Open the interface to the RoboClaw
                labelRoboClawModel.Text = roboClawModel; // Display the RoboClaw device model number
                roboClaw.ResetEncoders();
                buttonConnect.Enabled = false;
                buttonGoForward.Enabled = true;
                buttonGoReverse.Enabled = true;
                buttonDisconnect.Enabled = true;
            }
        }

        private void buttonGoForward_Click(object sender, EventArgs e)
        {
            if (roboClaw.IsOpen())
            {
                roboClaw.ST_M1Forward(speed); // Start the motor going forward at power 100
                myTimer.Start(); // Start timer to show encoder ticks
                buttonStop.Enabled = true;
                buttonGoForward.Enabled = false;
                buttonGoReverse.Enabled = false;
                buttonDisconnect.Enabled = false;
                buttonGoToZero.Enabled = false;
            }
        }

        private void buttonGoReverse_Click(object sender, EventArgs e)
        {
            if (roboClaw.IsOpen())
            {
                roboClaw.ST_M1Backward(speed); // Start the motor going forward at power 100
                myTimer.Start(); // Start timer to show encoder ticks
                buttonStop.Enabled = true;
                buttonGoForward.Enabled = false;
                buttonGoReverse.Enabled = false;
                buttonDisconnect.Enabled = false;
                buttonGoToZero.Enabled = false;
            }
        }

        private void buttonGoToZero_Click(object sender, EventArgs e)
        {
            if (roboClaw.IsOpen())
            {
                encoderWatch = true;
                myTimer.Start(); // Start timer to show encoder ticks
                if (m1_count < 0)
                {
                    roboClaw.ST_M1Backward(speed); // Start the motor going forward at power 100
                }
                else if (m1_count > 0)
                {
                    roboClaw.ST_M1Forward(speed); // Start the motor going forward at power 100
                }
                buttonStop.Enabled = true;
                buttonGoForward.Enabled = false;
                buttonGoReverse.Enabled = false;
                buttonDisconnect.Enabled = false;
                buttonGoToZero.Enabled = false;
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (roboClaw.IsOpen())
            {
                roboClaw.ST_M1Forward(0); // Stop the motor
                myTimer.Stop(); // Stop timer to stop encoder updates
                buttonStop.Enabled = false;
                buttonGoForward.Enabled = true;
                buttonGoReverse.Enabled = true;
                buttonDisconnect.Enabled = true;
                if(Math.Abs(m1_count) > 10)
                {
                    buttonGoToZero.Enabled = true;
                }
                encoderWatch = false;
            }
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            if (roboClaw.IsOpen())
            {
                myTimer.Stop(); // Stop the timer to stop the encoder display updates
                roboClaw.Close(); // Close the RoboClaw interface
                labelRoboClawModel.Text = " "; // Clear the RoboClaw device model number display
                buttonStop.Enabled = false;
                buttonGoForward.Enabled = false;
                buttonGoReverse.Enabled = false;
                buttonGoToZero.Enabled = false;
                buttonConnect.Enabled = true;
                buttonDisconnect.Enabled = false;
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (roboClaw.IsOpen())
            {
                roboClaw.ST_M1Forward(0); // Stop the motor
                roboClaw.Close(); // Close the interface
                myTimer.Stop(); // Stop the timer to stop the encoder display updates
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                speed = 0;
            }
            else
            {
                speed = Convert.ToByte(textBox1.Text);
            }
        }

        // This is the method to run when the timer is raised.
        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            roboClaw.GetEncoders(out m1_count, out m2_count);
            labelTicksCount.Text = m1_count.ToString();
            if (encoderWatch)
            {
                if (Math.Abs(m1_count) < 10)
                {
                    roboClaw.ST_M1Forward(0);
                    myTimer.Stop(); // Stop timer to stop encoder updates
                    buttonStop.Enabled = false;
                    buttonGoForward.Enabled = true;
                    buttonGoReverse.Enabled = true;
                    buttonGoToZero.Enabled = false;
                    buttonDisconnect.Enabled = true;
                    if (Math.Abs(m1_count) > 10)
                    {
                        buttonGoToZero.Enabled = true;
                    }
                    encoderWatch = false;
                }
                else if (Math.Abs(m1_count) < 10)
                {
                    if (m1_count > 0)
                    {
                        roboClaw.ST_M1Forward(0);
                    }
                    else
                    {
                        roboClaw.ST_M1Backward(0);
                    }
                }
                else if (Math.Abs(m1_count) < 100)
                {
                    if (m1_count > 0)
                    {
                        roboClaw.ST_M1Forward(0);
                    }
                    else
                    {
                        roboClaw.ST_M1Backward(0);
                    }
                }
                else if (Math.Abs(m1_count) < 250)
                {
                    if (m1_count > 0)
                    {
                        roboClaw.ST_M1Forward(0);
                    }
                    else
                    {
                        roboClaw.ST_M1Backward(0);
                    }
                }
                else if (Math.Abs(m1_count) < 1000)
                {
                    if (m1_count > 0)
                    {
                        roboClaw.ST_M1Forward(10);
                    }
                    else
                    {
                        roboClaw.ST_M1Backward(10);
                    }
                }
            }
        }
    }
}
