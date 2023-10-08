using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

        Timer timer;

        public PerlinForm()
        {
            InitializeComponent();
        }

        private void PerlinForm_Shown(object sender, EventArgs e)
        {
            flowField = new PerlinNoiseFlowField(pictureBox1.Width, pictureBox1.Height);
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

            timer = new Timer();
            timer.Enabled = false;
            timer.Interval = 50;
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 3; i++) makeStep();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            makeStep();
        }

        private void makeStep()
        {
            particleHolder.MoveParticles(flowField, 1.5);
            Bitmap bmp = (Bitmap)baseBmp.Clone();
            if (!checkBox2.Checked) bmp = new Bitmap(pictureBox1.Image);
            particleHolder.RenderParticles(ref bmp);
            pictureBox1.Image = bmp;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer.Enabled = checkBox1.Checked;
        }
    }
}
