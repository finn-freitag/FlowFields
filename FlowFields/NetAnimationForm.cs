using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace FlowFields
{
    public partial class NetAnimationForm : Form
    {
        List<PerlinNoiseFlowField> flowFields = new List<PerlinNoiseFlowField>();
        List<ParticleHolder> particleHolders = new List<ParticleHolder>();
        Bitmap baseBmp = null;
        int fieldCount = 5;
        double pointsPerPixel = 0.00005;

        System.Windows.Forms.Timer timer;

        public NetAnimationForm()
        {
            InitializeComponent();
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
        }

        private void NetAnimationForm_Shown(object sender, EventArgs e)
        {
            int pointsPerHolder = (int)(pointsPerPixel * pictureBox1.Width * pictureBox1.Height / fieldCount);
            for (int i = 0; i < fieldCount; i++)
            {
                flowFields.Add(new PerlinNoiseFlowField(pictureBox1.Width, pictureBox1.Height));
                flowFields[i].RenderFlowVectors();
                particleHolders.Add(new DisappearingParticleHolder(pointsPerHolder, pictureBox1.Width, pictureBox1.Height) { Brush = new SolidBrush(Color.White) });
            }

            ((DisappearingParticleHolder)particleHolders[0]).Background = new SolidBrush(Color.FromArgb(12, 77, 21));

            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.Flush();
            g.Dispose();
            //flowField.RenderFlowVectorsToBitmap(ref bmp, 10, Color.White);
            pictureBox1.Image = bmp;
            baseBmp = bmp;

            timer = new System.Windows.Forms.Timer();
            timer.Enabled = false;
            timer.Interval = 75;
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++) makeStep();
        }

        private void makeStep()
        {
            for (int i = 0; i < flowFields.Count; i++)
            {
                particleHolders[i].MoveParticles(flowFields[i], 1.5);
            }
            Bitmap bmp;
            if (!checkBox2.Checked) bmp = new Bitmap(pictureBox1.Image);
            else bmp = (Bitmap)baseBmp.Clone();
            for (int i = 0; i < flowFields.Count; i++)
            {
                particleHolders[i].RenderParticles(ref bmp);
            }
            Graphics g = Graphics.FromImage(bmp);
            double maxLineLength = Math.Max(pictureBox1.Width, pictureBox1.Height) / 3;
            double maxDist = Math.Sqrt(Math.Pow(pictureBox1.Width, 2) + Math.Pow(pictureBox1.Height, 2));
            for (int i = 0; i < particleHolders.Count; i++)
            {
                for (int j = 0; j < particleHolders[i].particles.Count; j++)
                {
                    for (int k = 0; k < particleHolders.Count; k++)
                    {
                        for (int l = 0; l < particleHolders[k].particles.Count; l++)
                        {
                            double dist = Maths.GetDistance(particleHolders[i].particles[j], particleHolders[k].particles[l]);
                            if (dist > maxLineLength) continue;
                            int opacity = (int)Math.Pow(Math.Max(Math.Min(255 - (dist / maxDist * 255), 255), 0) / 25, 2);
                            g.DrawLine(new Pen(new SolidBrush(Color.FromArgb(opacity, 255, 255, 255))), particleHolders[i].particles[j], particleHolders[k].particles[l]);
                        }
                    }
                }
            }
            g.Flush();
            g.Dispose();
            try
            {
                pictureBox1.Image = bmp;
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            makeStep();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer.Enabled = checkBox1.Checked;
        }
    }
}
