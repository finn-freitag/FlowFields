using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowFields
{
    public partial class PixelHolderForm : Form
    {
        PerlinNoiseFlowField flowField;

        public PixelHolderForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(ofd.FileName);
                flowField = new PerlinNoiseFlowField(pictureBox1.Image.Width, pictureBox1.Image.Height);
                flowField.RenderFlowVectors();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "PNG|*.png";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = PixelHolder.MovePixels(new Bitmap(pictureBox1.Image), flowField, 3);
        }
    }
}
