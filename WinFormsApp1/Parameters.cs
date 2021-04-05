using System.Windows.Forms;
using System.Drawing;

namespace WinFormsApp1
{
    public partial class Parameters : Form
    {
        Form1 thisForm1;
        public Parameters()
        {
            InitializeComponent();
        }

        public Parameters(Form1 form)
        {
            InitializeComponent();
            thisForm1 = form;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {

            ColorDialog f = new ColorDialog();
            var res = f.ShowDialog();
            if (res == DialogResult.OK)
            {
                var color = f.Color;
                thisForm1.lineColor = color;
            }

        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            ColorDialog f = new ColorDialog();
            var res = f.ShowDialog();
            if (res == DialogResult.OK)
            {
                var color = f.Color;
                thisForm1.pointColor = color;
            }
        }


        private void trackBar1_Scroll(object sender, System.EventArgs e)
        {
            thisForm1.pointSize = new Size(this.trackBar1.Value, this.trackBar1.Value);
        }

        private void trackBar2_Scroll(object sender, System.EventArgs e)
        {
            thisForm1.curveWidth = this.trackBar2.Value;
        }
    }
}
