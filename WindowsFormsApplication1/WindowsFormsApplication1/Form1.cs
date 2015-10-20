using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        double zoom = 1;
        double moveX = 0, moveY = 0;
        int iterationCount = 1;

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            float length = (float)(pictureBox1.Width * 1.0 / 4 * zoom);
            float angle = 0;
            var command = this.GetLevySystem(iterationCount, ref length);
            PointF lineStart = new PointF((float)pictureBox1.Width / 2, (float)pictureBox1.Height / 2);
            command
                .ToCharArray()
                .ToList()
                .ForEach(cmd =>
                {
                    switch (cmd)
                    {
                        case 'L':
                            angle += (float)Math.PI / 4;
                            break;
                        case 'R':
                            angle -= (float)Math.PI / 4;
                            break;
                        case 'F':
                            PointF lineEnd = GetPointFromPolar(length, angle);
                            lineEnd = new PointF(lineStart.X + lineEnd.X, lineStart.Y + lineEnd.Y);

                            var start = lineStart;

                            e.Graphics.DrawLine(new Pen(Color.Black, 1), start, lineEnd);
                            lineStart = lineEnd;
                            break;
                    }
                });
        }

        private PointF GetPointFromPolar(float length, float angle)
        {
            return new PointF(length * (float)Math.Cos(angle), length * (float)Math.Sin(angle));
        }

        private string GetLevySystem(int p, ref float length)
        {
            string result = "F";
            while (p > 0)
            {
                length = (float)Math.Sqrt(length * length / 2);
                result = result.Replace("F", "LFRRFL");
                p--;
            }
            return result;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            zoom = (double)numericUpDown1.Value;
            pictureBox1.Invalidate();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            numericUpDown2.Value = iterationCount;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            iterationCount = (int)numericUpDown2.Value;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != '-' && e.KeyChar != '.' && (e.KeyChar < 48 || e.KeyChar > 57))
                e.KeyChar = (char)0;
            else
            {
                if ((String.IsNullOrEmpty((sender as TextBox).Text) && e.KeyChar == '.') || (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > 0))
                    e.KeyChar = (char)0;
            }
        }
    }
}
