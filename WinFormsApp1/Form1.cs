using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        int iPointToDrag;
        public float curveWidth = 2;
        public Size pointSize = new Size(5, 5);
        public Color pointColor = Color.Black;
        public Color lineColor = Color.Red;

        private static List<Point> listPoints = new List<Point>();

        // Cache font instead of recreating font objects each time we paint.

        private void Form1_Load(object sender, System.EventArgs e)
        {

        }



        // Обработчик события Paint
        void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillClosedCurve(Brushes.Green, listPoints.ToArray());

        }

        public Form1()
        {
            InitializeComponent();
            //Paint += Form1_Paint;
            //this.KeyDown += new KeyEventHandler
            this.KeyDown += new KeyEventHandler(keyPress);
        }

        private void keyPress(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case (Keys.Space):
                    //KeyDown += new KeyEventHandler(keyPress);
                    break;
                default:
                    break;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MouseDown -= mouseEvent;
            MouseClick += AddPoint;
        }

        private void AddPoint(object sender, MouseEventArgs e)
        {
            listPoints.Add(e.Location);

            Graphics g = this.CreateGraphics();

            Rectangle rect = new Rectangle(e.Location, pointSize);
            g.FillEllipse(new SolidBrush(pointColor), rect);

        }

        private void RedrawPoints()
        {
            Graphics g = this.CreateGraphics();
            Point[] arPoints = listPoints.ToArray();
            Rectangle rect;
            foreach (var p in arPoints)
            {
                rect = new Rectangle(p.X, p.Y, pointSize.Height, pointSize.Width);
                g.FillEllipse(new SolidBrush(pointColor), rect);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Parameters newForm = new Parameters(this);
            newForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            MouseClick -= AddPoint;
            this.MouseDown += new MouseEventHandler(mouseEvent);
            this.MouseUp += mouseUpEvent;
        }

        private void mouseUpEvent(object sender, MouseEventArgs e)
        {
            //this.MouseDown -= mouseEvent;
            this.MouseMove -= mouseDrag;
        }

        private void mouseEvent(object sender, MouseEventArgs e)
        {
            //this.MouseMove += new MouseEventHandler(mouseDrag);
            Point[] arPoints = listPoints.ToArray();

            for (var i = 0; i < arPoints.Length; i++)
            {
                if (Math.Abs((e.Location - (Size)arPoints[i]).X) < 10 && Math.Abs((e.Location - (Size)arPoints[i]).Y) < 10)
                {
                    iPointToDrag = i;
                    this.MouseMove += new MouseEventHandler(mouseDrag);

                }
            }

        }

        private void mouseDrag(object sender, MouseEventArgs e)
        {
            Refresh();
            RedrawPoints();
            listPoints[iPointToDrag] = e.Location;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MouseClick -= AddPoint;
            listPoints.Clear();
            Graphics g = this.CreateGraphics();
            Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            MouseClick -= AddPoint;

            Graphics g = this.CreateGraphics();
            Refresh();

            RedrawPoints();
            Point[] arPoints = listPoints.ToArray();

            Pen pen = new Pen(lineColor, curveWidth);
            g.DrawClosedCurve(pen, arPoints);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            MouseClick -= AddPoint;
            Graphics g = this.CreateGraphics();
            Refresh();

            RedrawPoints();
            Point[] arPoints = listPoints.ToArray();

            Pen pen = new Pen(lineColor, curveWidth);
            g.DrawPolygon(pen, arPoints);
        }

        private void button7_Click(object sender, EventArgs e)//точек должно быть 3*n+1 (4,7,10...)
        {
            MouseClick -= AddPoint;
            Graphics g = this.CreateGraphics();
            Refresh();

            RedrawPoints();
            Point[] arPoints = listPoints.ToArray();

            Pen pen = new Pen(lineColor, curveWidth);
            g.DrawBeziers(pen, arPoints);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MouseClick -= AddPoint;
            Graphics g = this.CreateGraphics();
            Refresh();

            RedrawPoints();
            Point[] arPoints = listPoints.ToArray();
            Image image = Image.FromFile("чародейки.jpg");
            TextureBrush brush = new TextureBrush(image);
            g.FillClosedCurve(brush, arPoints);
        }
    }
}
