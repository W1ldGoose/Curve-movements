using System;
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
        int speed = 3;


        private static Point[] arPoints = new Point[0];
        private bool isCurve = false, isPoly = false, isFillCurve = false, isBezier = false, bMove = false;


        public Form1()
        {
            InitializeComponent();
            int var = ((int)('е') + (int)('з')) % 8;
            MessageBox.Show($"Вариант: {var}");
            pictureBox1.MouseDown += new MouseEventHandler(mouseDown);
            KeyPreview = true;
            KeyDown += new KeyEventHandler(keyDown);
        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            this.Capture = true;
            switch (e.KeyCode)
            {
                case (Keys.Add):
                    speed++;
                    break;
                case (Keys.Subtract):
                    if(speed>0)
                    speed--;
                    break;
                case (Keys.Space):
                    if (bMove)
                    {
                        bMove = false;
                        timer1.Tick -= TimerTickHandler;
                    }
                    else
                    {
                        bMove = true;
                        timer1.Tick += TimerTickHandler;
                    }
                    break;
                case (Keys.Escape):
                    if (bMove)
                    {
                        bMove = false;
                        timer1.Tick -= TimerTickHandler;
                    }
                    Array.Resize(ref arPoints, 0);
                    pictureBox1.Refresh();
                    break;
            }
            e.Handled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.MouseClick -= new MouseEventHandler(AddPoint);
            pictureBox1.MouseClick += new MouseEventHandler(AddPoint);
        }

        private void AddPoint(object sender, MouseEventArgs e)
        {
            Array.Resize(ref arPoints, arPoints.Length + 1);
            arPoints[arPoints.Length - 1] = e.Location;

            Graphics g = pictureBox1.CreateGraphics();
            Rectangle rect = new Rectangle(e.Location, pointSize);
            g.FillEllipse(new SolidBrush(pointColor), rect);
            g.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Parameters newForm = new Parameters(this);
            newForm.Show();
        }
        static Point[][] newArr = new Point[2][];

        private void button3_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            int tmpX = rand.Next(-1, 1)*speed;
            int tmpY = rand.Next(-1, 1)*speed;

            newArr[0] = (Point[])arPoints.Clone();
            newArr[1] = new Point[arPoints.Length];

            for (int i = 0; i < newArr[1].Length; i++)
            {
                newArr[1][ i] = new Point(tmpX, tmpY);
            }

            if (!bMove)
            {
                timer1.Tick += new EventHandler(TimerTickHandler);
                timer1.Interval = 30;
            }
            else
            {
                bMove = false;
                timer1.Tick -= TimerTickHandler;
            }
            pictureBox1.MouseClick -= AddPoint;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (bMove)
            {
                switch (keyData)
                {
                    case (Keys.Left):
                        for (int i = 0; i < newArr[1].Length; i++)
                            newArr[1][i].X = -speed;

                        break;
                    case (Keys.Right):
                        for (int i = 0; i < newArr[1].Length; i++)
                            newArr[1][i].X = speed;

                        break;
                    case (Keys.Down):
                        for (int i = 0; i < newArr[1].Length; i++)
                            newArr[1][i].Y = speed;
                        break;
                    case (Keys.Up):
                        for (int i = 0; i < newArr[1].Length; i++)
                            newArr[1][i].Y = -speed;

                        break;
                    default:
                        return base.ProcessCmdKey(ref msg, keyData);
                }

                return true;
            }
            return false;
        }
        private void MovePoint(int i)
        {
            if (arPoints[i].X < 0)
                newArr[1][i].X = speed;
            if (arPoints[i].X > pictureBox1.Width)
                newArr[1][i].X = -speed;
            if (arPoints[i].Y < 0)
                newArr[1][i].Y = speed;
            if (arPoints[i].Y > pictureBox1.Height)
                newArr[1][i].Y = -speed;
            arPoints[i].X += newArr[1][i].X; arPoints[i].Y += newArr[1][i].Y;

        }

        private void TimerTickHandler(object sender, EventArgs e)
        {

            bMove = true;
            for (int i = 0; i < arPoints.Length; i++)
            {
                MovePoint(i);
            }
            pictureBox1.Refresh();
        }

        private void mouseUpEvent(object sender, MouseEventArgs e)
        {
            pictureBox1.MouseMove -= mouseDrag;
            arPoints[iPointToDrag] = e.Location;
        }

        private void mouseDown(object sender, MouseEventArgs e)
        {
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
            arPoints[iPointToDrag] = e.Location;
            pictureBox1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CheckEvents();

            if (bMove)
            {
                bMove = false;
                timer1.Tick -= TimerTickHandler;
            }
            pictureBox1.MouseClick -= AddPoint;
            Array.Resize(ref arPoints, 0);
            pictureBox1.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            pictureBox1.MouseClick -= AddPoint;
            CheckEvents();
            pictureBox1.Paint += new PaintEventHandler(paint_Poly);
            pictureBox1.Refresh();
        }

        private void paint_Poly(object sender, PaintEventArgs e)
        {
            isPoly = true;
            Graphics g = e.Graphics;
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
            if (isBezier)
            {
                pictureBox1.Paint -= paint_Bezier;
                isBezier = false;
            }
            if (isFillCurve)
            {
                pictureBox1.Paint -= paint_Fill;
                isFillCurve = false;
            }
        }

        private void paint_Curve(object sender, PaintEventArgs e)
        {
            isCurve = true;
            Graphics g = e.Graphics;

            Pen pen = new Pen(lineColor, curveWidth);
            g.DrawClosedCurve(pen, arPoints);
        }

        private void button7_Click(object sender, EventArgs e)//точек должно быть 3*n+1 (4,7,10...)
        {
            pictureBox1.MouseClick -= AddPoint;
            CheckEvents();
            pictureBox1.Paint += new PaintEventHandler(paint_Bezier);
            pictureBox1.Refresh();
        }

        private void paint_Bezier(object sender, PaintEventArgs e)
        {
            isBezier = true;
            if (arPoints.Length % 3 != 1)
                return;
            //pictureBox1.Paint -= paint_Bezier;
            //isBezier = false;
            Graphics g = e.Graphics;
            Pen pen = new Pen(lineColor, curveWidth);
            g.DrawBeziers(pen, arPoints);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            pictureBox1.MouseClick -= AddPoint;
            CheckEvents();
            pictureBox1.Paint += new PaintEventHandler(paint_Fill);
            pictureBox1.Refresh();
        }

        private void paint_Fill(object sender, PaintEventArgs e)
        {
            isFillCurve = true;
            Graphics g = e.Graphics;
            Image image = Image.FromFile("чародейки.jpg");
            TextureBrush brush = new TextureBrush(image);
            g.FillClosedCurve(brush, arPoints);
        }

        private void picture_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Rectangle rect;
            foreach (var p in arPoints)
            {
                rect = new Rectangle(p.X, p.Y, pointSize.Height, pointSize.Width);
                g.FillEllipse(new SolidBrush(pointColor), rect);
            }
        }
    }
}
