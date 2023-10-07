using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowFields
{
    public partial class MainWindow : Form
    {
        GravityPointFlowField flowField;
        ParticleHolder particleHolder;
        Bitmap baseBmp = null;

        Timer timer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            flowField = new GravityPointFlowField(pictureBox1.Width, pictureBox1.Height);
            flowField.DefaultAngle = 180;
            FlowDirection fd = new FlowDirection();
            fd.Position = new Vector(200, 200);
            fd.Flow = new AngleVector(90, 100);
            flowField.Items.Add(fd);
            FlowDirection fd2 = new FlowDirection();
            fd2.Position = new Vector(400, 400);
            fd2.Flow = new AngleVector(90, 100);
            flowField.Items.Add(fd2);
            GravityPoint gp = new GravityPoint();
            gp.Position = new Vector(200, 400);
            gp.Radius = 100;
            flowField.Items.Add(gp);
            TangentGravityPoint tgp = new TangentGravityPoint();
            tgp.Position = new Vector(400, 200);
            tgp.Radius = 100;
            flowField.Items.Add(tgp);
            flowField.RenderFlowVectors();

            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            g.Flush();
            g.Dispose();
            flowField.RenderFlowVectorsToBitmap(ref bmp, 10, Color.White);
            pictureBox1.Image = bmp;
            baseBmp = bmp;

            particleHolder = new ParticleHolder(300, pictureBox1.Width, pictureBox1.Height);

            timer = new Timer();
            timer.Enabled = false;
            timer.Interval = 100;
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            makeStep();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            makeStep();
        }

        private void makeStep()
        {
            particleHolder.MoveParticles(flowField, 1);
            Bitmap bmp = (Bitmap)baseBmp.Clone();
            if (!checkBox2.Checked) bmp = new Bitmap(pictureBox1.Image);
            particleHolder.RenderParticles(ref bmp, 3, Color.Red);
            pictureBox1.Image = bmp;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer.Enabled = checkBox1.Checked;
        }
    }
}
