using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        int iPointToDrag; // проверку не забыть
        public float curveWidth = 2;
        public Size pointSize = new Size(5, 5);
        public Color pointColor = Color.Black;
        public Color lineColor = Color.Red;
        int speed;

        private static List<Point> listPoints = new List<Point>();

        private Boolean isCurve = false, isPoly = false, isFillCurve = false, isBezier = false;
        // Обработчик события Paint


        public Form1()
        {
            InitializeComponent();
            //Paint += Form1_Paint;
            pictureBox1.MouseDown += new MouseEventHandler(mouseDown);

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
            pictureBox1.MouseClick -= new MouseEventHandler(AddPoint);
            pictureBox1.MouseClick += new MouseEventHandler(AddPoint);
        }

        private void AddPoint(object sender, MouseEventArgs e)
        {

            listPoints.Add(e.Location);

            Graphics g = pictureBox1.CreateGraphics();
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
            speed = 3;
            pictureBox1.MouseClick -= AddPoint;
            timer1.Interval = 30;
            timer1.Tick += new EventHandler(TimerTickHandler);
        }
        int speedX = 3, speedY = 3;
        private void MovePoint(int i)
        {
            //speedX = speed; speedY = speed;
            bool test = false;
            Point p = listPoints[i];
            if (p.X < 0)
            {
                speedX = speed;
                test = true;
            }
            if (p.Y < 0)
            {
                speedY = speed;
                test = true;
            }
            if (p.X > pictureBox1.Width)
            {
                speedX = -speed;
                test = true;
            }
            if (p.Y > pictureBox1.Height)
            {
                speedY = -speed;
                test = true;
            }
         
               // listPoints[i] = new Point(listPoints[i].X, listPoints[i].Y) + new Size(speed, speed);
        }

        private void TimerTickHandler(object sender, EventArgs e)
        {
            //Point arPoints listPoints.ToArray
            for (int i = 0; i < listPoints.Count; i++)
            {
               
                MovePoint(i);
                listPoints[i] = new Point(listPoints[i].X, listPoints[i].Y) + new Size(speedX, speedY);
            }
            pictureBox1.Refresh();
        }

        private void mouseUpEvent(object sender, MouseEventArgs e)
        {
            pictureBox1.MouseMove -= mouseDrag;
            listPoints[iPointToDrag] = e.Location;

        }

        private void mouseDown(object sender, MouseEventArgs e)
        {
            Point[] arPoints = listPoints.ToArray();

            for (var i = 0; i < arPoints.Length; i++)
            {
                if (Math.Abs((e.Location - (Size)arPoints[i]).X) < 10 && Math.Abs((e.Location - (Size)arPoints[i]).Y) < 10)
                {
                    pictureBox1.MouseClick -= AddPoint;
                    iPointToDrag = i;
                    pictureBox1.MouseMove += new MouseEventHandler(mouseDrag);
                }
            }

        }

        private void mouseDrag(object sender, MouseEventArgs e)
        {
            pictureBox1.MouseUp += new MouseEventHandler(mouseUpEvent);
            listPoints[iPointToDrag] = e.Location;
            pictureBox1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CheckEvents();
            pictureBox1.MouseClick -= AddPoint;
            listPoints.Clear();
            pictureBox1.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //isPoly = true;
            pictureBox1.MouseClick -= AddPoint;
            CheckEvents();
            pictureBox1.Paint += new PaintEventHandler(paint_Poly);
            pictureBox1.Refresh();
        }

        private void paint_Poly(object sender, PaintEventArgs e)
        {
            isPoly = true;
            Graphics g = e.Graphics;
            Point[] arPoints = listPoints.ToArray();

            Pen pen = new Pen(lineColor, curveWidth);
            g.DrawPolygon(pen, arPoints);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            pictureBox1.MouseClick -= AddPoint;
            CheckEvents();
            pictureBox1.Paint += new PaintEventHandler(paint_Curve);
            pictureBox1.Refresh();
        }

        private void CheckEvents()
        {

            if (isPoly)
            {
                pictureBox1.Paint -= paint_Poly;
                isPoly = false;
            }
            if (isCurve)
            {
                pictureBox1.Paint -= paint_Curve;
                isCurve = false;
            }
            //if(isBezier)

        }

        private void paint_Curve(object sender, PaintEventArgs e)
        {
            isCurve = true;
            Graphics g = e.Graphics;
            Point[] arPoints = listPoints.ToArray();

            Pen pen = new Pen(lineColor, curveWidth);
            g.DrawClosedCurve(pen, arPoints);
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

        private void picture_Paint(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;

            Point[] arPoints = listPoints.ToArray();

            Rectangle rect;
            foreach (var p in arPoints)
            {
                rect = new Rectangle(p.X, p.Y, pointSize.Height, pointSize.Width);
                g.FillEllipse(new SolidBrush(pointColor), rect);
            }


        }
    }
}
