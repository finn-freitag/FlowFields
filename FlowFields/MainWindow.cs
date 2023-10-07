﻿using System;
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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            flowField = new GravityPointFlowField(pictureBox1.Width, pictureBox1.Height);
            flowField.DefaultAngle = 0;
            FlowDirection fd = new FlowDirection();
            fd.Position = new Vector(200, 200);
            fd.Flow = new AngleVector(270, 100);
            flowField.Items.Add(fd);
            FlowDirection fd2 = new FlowDirection();
            fd2.Position = new Vector(400, 400);
            fd2.Flow = new AngleVector(-90, 100);
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
        }
    }
}
