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

namespace FlowFields
{
    public partial class ChargedParticleForm : Form
    {
        ChargedParticleFlowField flowField;
        ParticleHolder particleHolder;
        Bitmap baseBmp = null;

        System.Windows.Forms.Timer timer;

        public ChargedParticleForm()
        {
            InitializeComponent();
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
        }

        private void ChargedParticleForm_Shown(object sender, EventArgs e)
        {
            flowField = new ChargedParticleFlowField(pictureBox1.Width, pictureBox1.Height);
            flowField.ChargedParticles = new List<ChargedParticle>()
            {
                new ChargedParticle(new Vector(200, 200), true),
                new ChargedParticle(new Vector(500, 200), true),
                //new ChargedParticle(new Vector(350, 350), true),
            };
            flowField.RenderFlowVectors();

            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            g.Flush();
            g.Dispose();
            flowField.RenderFlowVectorsToBitmap(ref bmp, 10, Color.White);
            pictureBox1.Image = bmp;
            baseBmp = bmp;

            particleHolder = new DisappearingParticleHolder(300, pictureBox1.Width, pictureBox1.Height) { Brush = HSLBrush.CreateBrush(flowField) };

            timer = new System.Windows.Forms.Timer();
            timer.Enabled = false;
            timer.Interval = 50;
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++) makeStep();
        }

        private void makeStep()
        {
            particleHolder.MoveParticles(flowField, 1.5);
            Bitmap bmp;
            if (!checkBox2.Checked) bmp = new Bitmap(pictureBox1.Image);
            else bmp = (Bitmap)baseBmp.Clone();
            particleHolder.RenderParticles(ref bmp);
            pictureBox1.Image = bmp;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer.Enabled = checkBox1.Checked;
        }
    }
}
