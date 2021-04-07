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
            pictureBox1.MouseClick += new MouseEventHandler(AddPoint);
        }

        private void AddPoint(object sender, MouseEventArgs e)
        {
            
            listPoints.Add(e.Location);

            Graphics g = pictureBox1.CreateGraphics();
            Rectangle rect = new Rectangle(e.Location, pointSize);
            g.FillEllipse(new SolidBrush(pointColor), rect);

            //g.Dispose();
           
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

            
        }

        private void mouseUpEvent(object sender, MouseEventArgs e)
        {
            //this.MouseDown -= mouseDown;
            pictureBox1.MouseMove -= mouseDrag;
            listPoints[iPointToDrag] = e.Location;
            Refresh();
        }

        private void mouseDown(object sender, MouseEventArgs e)
        {
            //pictureBox1.MouseClick -= AddPoint;
            //this.MouseMove += new MouseEventHandler(mouseDrag);
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
            //Refresh();
            //RedrawPoints();
            pictureBox1.MouseUp += new MouseEventHandler(mouseUpEvent);
            listPoints[iPointToDrag] = e.Location;
            Refresh();
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
            isPoly = true;

            pictureBox1.Paint += new PaintEventHandler(paint_Poly);

            MouseClick -= AddPoint;

            Graphics g = this.CreateGraphics();
            Refresh();

            //RedrawPoints();
            Point[] arPoints = listPoints.ToArray();

            Pen pen = new Pen(lineColor, curveWidth);
            g.DrawClosedCurve(pen, arPoints);

        }

        private void paint_Poly(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
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
