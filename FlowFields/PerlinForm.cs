using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace FlowFields
{
    public partial class PerlinForm : Form
    {
        PerlinNoiseFlowField flowField;
        ParticleHolder particleHolder;
        Bitmap baseBmp = null;

        System.Windows.Forms.Timer timer;
        Thread t;

        public PerlinForm()
        {
            InitializeComponent();
        }

        private void PerlinForm_Shown(object sender, EventArgs e)
        {
            flowField = new PerlinNoiseFlowField(pictureBox1.Width, pictureBox1.Height) { Animate = true };
            flowField.RenderFlowVectors();

            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.Flush();
            g.Dispose();
            //flowField.RenderFlowVectorsToBitmap(ref bmp, 10, Color.White);
            pictureBox1.Image = bmp;
            baseBmp = bmp;

            particleHolder = new DisappearingParticleHolder(300, pictureBox1.Width, pictureBox1.Height) { Brush = HSLBrush.CreateBrush(flowField) };

            timer = new System.Windows.Forms.Timer();
            timer.Enabled = false;
            timer.Interval = 50;
            timer.Tick += Timer_Tick;

            if (flowField.Animate)
            {
                t = new Thread(new ThreadStart(AnimationThread));
                t.Priority = ThreadPriority.Highest;
                t.Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++) makeStep();
        }

        private void AnimationThread()
        {
            while (Thread.CurrentThread.ThreadState == ThreadState.Running)
            {
                flowField.RenderFlowVectors(1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            makeStep();
        }

        private void makeStep()
        {
            particleHolder.MoveParticles(flowField, 1.5);
            Bitmap bmp;
            if (!checkBox2.Checked) bmp = new Bitmap(pictureBox1.Image);
            else bmp = (Bitmap)baseBmp.Clone();
            particleHolder.RenderParticles(ref bmp);
            try
            {
                pictureBox1.Image = bmp;
            }
            catch { }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer.Enabled = checkBox1.Checked;
        }

        private void PerlinForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            t.Abort();
        }
    }
}
