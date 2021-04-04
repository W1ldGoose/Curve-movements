using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        
        private static List<Point> listPoints = new List<Point>();

        private PictureBox pictureBox1 = new PictureBox();
        // Cache font instead of recreating font objects each time we paint.
        private Font fnt = new Font("Arial", 10);
        private void Form1_Load(object sender, System.EventArgs e)
        {
            
            //pictureBox1.Paint += new PaintEventHandler()

            //this.Controls.Add(pictureBox1);
        }



        // Обработчик события Paint
        void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

        }

        public Form1()
        {
            InitializeComponent();
            Paint += Form1_Paint;
        }

        private void button1_Click(object sender, EventArgs e) => MouseClick += AddPoint;

        private void AddPoint(object sender, MouseEventArgs e)
        {
            listPoints.Add(e.Location);

            Graphics g = Graphics.FromHwnd(this.Handle);
            Pen blackPen = new Pen(new SolidBrush(Color.Black), 2f);
            g.DrawRectangle(blackPen, e.X, e.Y, 2, 2);
        }


        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Paint += Paint_Curve;
            /*MouseClick -= AddPoint;

            Point[] arPoints = listPoints.ToArray();
            Pen pen = new Pen(Color.Red);
            Graphics g = Graphics.FromHwnd(this.Handle);
            g.DrawClosedCurve(pen, arPoints);*/
        }

        private void Paint_Curve(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Red);
            g.DrawCurve(pen, listPoints.ToArray());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MouseClick -= AddPoint;
            Point[] arPoints = listPoints.ToArray();
            Pen pen = new Pen(Color.Red);
            Graphics g = Graphics.FromHwnd(this.Handle);
            g.DrawPolygon(pen, arPoints);
        }

        private void button7_Click(object sender, EventArgs e)//точек должно быть 3*n+1 (4,7,10...)
        {
            MouseClick -= AddPoint;
            Point[] arPoints = listPoints.ToArray();
            Pen pen = new Pen(Color.Red);
            Graphics g = Graphics.FromHwnd(this.Handle);
            g.DrawBeziers(pen, arPoints);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MouseClick -= AddPoint;
            Point[] arPoints = listPoints.ToArray();
            Pen pen = new Pen(Color.Red);
            Graphics g = Graphics.FromHwnd(this.Handle);
        }

    }
}
