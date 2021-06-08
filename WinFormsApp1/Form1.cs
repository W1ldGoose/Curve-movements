using System;
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
        int speed = 3;

        private static Point[] arPoints = new Point[0];
        private bool isCurve = false, isPoly = false, isFillCurve = false, isBezier = false, pointsMove = false;


        public Form1()
        {
            InitializeComponent();
            KeyPreview = true;
            KeyDown += new KeyEventHandler(keyDown);
        }

        private void AddPoint(object sender, MouseEventArgs e)
        {
            pictureBox1.MouseDown -= new MouseEventHandler(mouseDown);
            Array.Resize(ref arPoints, arPoints.Length + 1);
            arPoints[arPoints.Length - 1] = e.Location;

            Graphics g = pictureBox1.CreateGraphics();
            Rectangle rect = new Rectangle(e.Location, pointSize);
            g.FillEllipse(new SolidBrush(pointColor), rect);
            g.Dispose();
            pictureBox1.MouseDown += new MouseEventHandler(mouseDown);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.MouseClick -= new MouseEventHandler(AddPoint);
            pictureBox1.MouseClick += new MouseEventHandler(AddPoint);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Parameters newForm = new Parameters(this);
            newForm.Show();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (pointsMove)
            {
                pointsMove = false;
                timer1.Tick -= TimerTickHandler;
            }
            else
            {
                pointsMove = true;
                timer1.Interval = 30;
                timer1.Tick += new EventHandler(TimerTickHandler);
            }
            Random rand = new Random();
            int tmpX = rand.Next(-1, 1) * speed;
            int tmpY = rand.Next(-1, 1) * speed;
            Array.Resize(ref newArr, arPoints.Length);

            for (int i = 0; i < newArr.Length; i++)
                newArr[i] = new Point(tmpX, tmpY);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CheckEvents();
            pictureBox1.MouseMove -= mouseDrag;
            pictureBox1.MouseUp -= mouseUpEvent;
            pictureBox1.MouseDown -= mouseDown;
            if (pointsMove)
            {
                pointsMove = false;
                timer1.Tick -= TimerTickHandler;
            }
            Array.Resize(ref arPoints, 0);
            pictureBox1.Refresh();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            CheckEvents();
            pictureBox1.Paint += new PaintEventHandler(paint_Poly);
            pictureBox1.Refresh();
        }
 
        private void button6_Click(object sender, EventArgs e)
        {
            CheckEvents();
            pictureBox1.Paint += new PaintEventHandler(paint_Curve);
            pictureBox1.Refresh();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            CheckEvents();
            pictureBox1.Paint += new PaintEventHandler(paint_Bezier);
            pictureBox1.Refresh();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            CheckEvents();
            pictureBox1.Paint += new PaintEventHandler(paint_Fill);
            pictureBox1.Refresh();
        }

        static Point[] newArr = new Point[0];
       
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (pointsMove)
            {
                switch (keyData)
                {
                    case (Keys.Left):
                        for (int i = 0; i < newArr.Length; i++)
                            newArr[i].X = -speed;

                        break;
                    case (Keys.Right):
                        for (int i = 0; i < newArr.Length; i++)
                            newArr[i].X = speed;

                        break;
                    case (Keys.Down):
                        for (int i = 0; i < newArr.Length; i++)
                            newArr[i].Y = speed;
                        break;
                    case (Keys.Up):
                        for (int i = 0; i < newArr.Length; i++)
                            newArr[i].Y = -speed;

                        break;
                    default:
                        return base.ProcessCmdKey(ref msg, keyData);
                }

                return true;
            }
            return false;
        }
        private void keyDown(object sender, KeyEventArgs e)
        {
            this.Capture = true;
            switch (e.KeyCode)
            {
                case (Keys.Add):
                    speed++;
                    for (int i = 0; i < newArr.Length; i++)
                        newArr[i] = new Point(Math.Sign(newArr[i].X) * speed, Math.Sign(newArr[i].Y) * speed);
                    break;
                case (Keys.Subtract):
                    if (speed > 0)
                    {
                        speed--;
                        for (int i = 0; i < newArr.Length; i++)
                            newArr[i] = new Point(Math.Sign(newArr[i].X) * speed, Math.Sign(newArr[i].Y) * speed);
                    }
                    break;
                case (Keys.Space):
                    if (pointsMove)
                    {
                        pointsMove = false;
                        timer1.Tick -= TimerTickHandler;
                    }
                    else
                    {
                        pointsMove = true;
                        timer1.Interval = 30;
                        timer1.Tick += new EventHandler(TimerTickHandler);
                    }
                    Random rand = new Random();
                    int tmpX = rand.Next(-1, 1) * speed;
                    int tmpY = rand.Next(-1, 1) * speed;
                    Array.Resize(ref newArr, arPoints.Length);
                    for (int i = 0; i < newArr.Length; i++)
                        newArr[i] = new Point(tmpX, tmpY);

                    break;
                case (Keys.Escape):
                    if (pointsMove)
                    {
                        pointsMove = false;
                        timer1.Tick -= TimerTickHandler;
                    }
                    Array.Resize(ref arPoints, 0);
                    pictureBox1.Refresh();
                    break;

            }
            e.Handled = true;
        }
        private void MovePoint(int i)
        {
            if (arPoints[i].X < 0)
                newArr[i].X = speed;
            if (arPoints[i].X > pictureBox1.Width)
                newArr[i].X = -speed;
            if (arPoints[i].Y < 0)
                newArr[i].Y = speed;
            if (arPoints[i].Y > pictureBox1.Height)
                newArr[i].Y = -speed;
            arPoints[i].X += newArr[i].X; arPoints[i].Y += newArr[i].Y;

        }

        private void TimerTickHandler(object sender, EventArgs e)
        {

            pointsMove = true;
            for (int i = 0; i < arPoints.Length; i++)
            {
                MovePoint(i);
            }
            pictureBox1.Refresh();
        }

        private void mouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.MouseUp -= mouseUpEvent;
            if (arPoints.Length != 0)
            {
                for (var i = 0; i < arPoints.Length; i++)
                {
                    if (Math.Abs((e.Location - (Size)arPoints[i]).X) < 10 && Math.Abs((e.Location - (Size)arPoints[i]).Y) < 10)
                    {
                        pictureBox1.MouseClick -= AddPoint;
                        iPointToDrag = i;
                        pictureBox1.MouseMove += new MouseEventHandler(mouseDrag);
                        pictureBox1.MouseUp += new MouseEventHandler(mouseUpEvent);
                    }
                }
            }
        }

        private void mouseDrag(object sender, MouseEventArgs e)
        {
            arPoints[iPointToDrag] = e.Location;
            pictureBox1.Refresh();
        }
        private void mouseUpEvent(object sender, MouseEventArgs e)
        {
            pictureBox1.MouseMove -= mouseDrag;
            arPoints[iPointToDrag] = e.Location;
        }


        private void paint_Poly(object sender, PaintEventArgs e)
        {
            isPoly = true;
            Graphics g = e.Graphics;
            Pen pen = new Pen(lineColor, curveWidth);
            try
            {
                g.DrawPolygon(pen, arPoints);
            }
            catch (System.ArgumentException exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void paint_Curve(object sender, PaintEventArgs e)
        {
            isCurve = true;
            Graphics g = e.Graphics;

            Pen pen = new Pen(lineColor, curveWidth);
            try
            {
                g.DrawClosedCurve(pen, arPoints);
            }
            catch (System.ArgumentException exp)
            {
                MessageBox.Show(exp.Message);

            }
        }

        private void paint_Bezier(object sender, PaintEventArgs e)
        {
            isBezier = true;
            if (arPoints.Length % 3 != 1)
                MessageBox.Show("точек должно быть 3*n+1 (4,7,10...)");
            Graphics g = e.Graphics;
            Pen pen = new Pen(lineColor, curveWidth);
            try
            {
                g.DrawBeziers(pen, arPoints);
            }
            catch (System.ArgumentException exp)
            {
                MessageBox.Show(exp.Message);
            }

        }

        private void paint_Fill(object sender, PaintEventArgs e)
        {
            isFillCurve = true;
            Graphics g = e.Graphics;
            try
            {
                g.FillClosedCurve(Brushes.Aqua, arPoints);
            }
            catch (System.ArgumentException exp)
            {
                MessageBox.Show(exp.Message);
            }

        }

        private void picture_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Rectangle rect;
            foreach (var p in arPoints)
            {
                rect = new Rectangle(p.X - pointSize.Width / 2, p.Y - pointSize.Height / 2, pointSize.Height, pointSize.Width);
                g.FillEllipse(new SolidBrush(pointColor), rect);
            }

        }
        private void CheckEvents()
        {
            pictureBox1.MouseClick -= AddPoint;
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
    }
}
