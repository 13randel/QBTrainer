namespace RoboClawTest01
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonGoForward = new System.Windows.Forms.Button();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.labelModelTitle = new System.Windows.Forms.Label();
            this.labelRoboClawModel = new System.Windows.Forms.Label();
            this.labelTicks = new System.Windows.Forms.Label();
            this.labelTicksCount = new System.Windows.Forms.Label();
            this.buttonGoReverse = new System.Windows.Forms.Button();
            this.buttonGoToZero = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(13, 13);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(12, 149);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 1;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonGoForward
            // 
            this.buttonGoForward.Enabled = false;
            this.buttonGoForward.Location = new System.Drawing.Point(13, 43);
            this.buttonGoForward.Name = "buttonGoForward";
            this.buttonGoForward.Size = new System.Drawing.Size(75, 23);
            this.buttonGoForward.TabIndex = 2;
            this.buttonGoForward.Text = "Go Forward";
            this.buttonGoForward.UseVisualStyleBackColor = true;
            this.buttonGoForward.Click += new System.EventHandler(this.buttonGoForward_Click);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDisconnect.Enabled = false;
            this.buttonDisconnect.Location = new System.Drawing.Point(12, 178);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(75, 23);
            this.buttonDisconnect.TabIndex = 3;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // labelModelTitle
            // 
            this.labelModelTitle.AutoSize = true;
            this.labelModelTitle.Location = new System.Drawing.Point(96, 18);
            this.labelModelTitle.Name = "labelModelTitle";
            this.labelModelTitle.Size = new System.Drawing.Size(39, 13);
            this.labelModelTitle.TabIndex = 4;
            this.labelModelTitle.Text = "Model:";
            // 
            // labelRoboClawModel
            // 
            this.labelRoboClawModel.AutoSize = true;
            this.labelRoboClawModel.Location = new System.Drawing.Point(141, 18);
            this.labelRoboClawModel.Name = "labelRoboClawModel";
            this.labelRoboClawModel.Size = new System.Drawing.Size(10, 13);
            this.labelRoboClawModel.TabIndex = 5;
            this.labelRoboClawModel.Text = " ";
            // 
            // labelTicks
            // 
            this.labelTicks.AutoSize = true;
            this.labelTicks.Location = new System.Drawing.Point(96, 48);
            this.labelTicks.Name = "labelTicks";
            this.labelTicks.Size = new System.Drawing.Size(36, 13);
            this.labelTicks.TabIndex = 6;
            this.labelTicks.Text = "Ticks:";
            // 
            // labelTicksCount
            // 
            this.labelTicksCount.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTicksCount.Location = new System.Drawing.Point(138, 48);
            this.labelTicksCount.Name = "labelTicksCount";
            this.labelTicksCount.Size = new System.Drawing.Size(100, 14);
            this.labelTicksCount.TabIndex = 7;
            this.labelTicksCount.Text = "     0";
            this.labelTicksCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonGoReverse
            // 
            this.buttonGoReverse.Enabled = false;
            this.buttonGoReverse.Location = new System.Drawing.Point(13, 73);
            this.buttonGoReverse.Name = "buttonGoReverse";
            this.buttonGoReverse.Size = new System.Drawing.Size(75, 23);
            this.buttonGoReverse.TabIndex = 8;
            this.buttonGoReverse.Text = "Go Reverse";
            this.buttonGoReverse.UseVisualStyleBackColor = true;
            this.buttonGoReverse.Click += new System.EventHandler(this.buttonGoReverse_Click);
            // 
            // buttonGoToZero
            // 
            this.buttonGoToZero.Enabled = false;
            this.buttonGoToZero.Location = new System.Drawing.Point(13, 103);
            this.buttonGoToZero.Name = "buttonGoToZero";
            this.buttonGoToZero.Size = new System.Drawing.Size(75, 23);
            this.buttonGoToZero.TabIndex = 9;
            this.buttonGoToZero.Text = "Go To Zero";
            this.buttonGoToZero.UseVisualStyleBackColor = true;
            this.buttonGoToZero.Click += new System.EventHandler(this.buttonGoToZero_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(158, 76);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 10;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(99, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Speed";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 213);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonGoToZero);
            this.Controls.Add(this.buttonGoReverse);
            this.Controls.Add(this.labelTicksCount);
            this.Controls.Add(this.labelTicks);
            this.Controls.Add(this.labelRoboClawModel);
            this.Controls.Add(this.labelModelTitle);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.buttonGoForward);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonConnect);
            this.Name = "Form1";
            this.Text = "RoboClawTest01";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonGoForward;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.Label labelModelTitle;
        private System.Windows.Forms.Label labelRoboClawModel;
        private System.Windows.Forms.Label labelTicks;
        private System.Windows.Forms.Label labelTicksCount;
        private System.Windows.Forms.Button buttonGoReverse;
        private System.Windows.Forms.Button buttonGoToZero;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
    }
}

