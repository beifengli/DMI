using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dashboard
{
    public partial class Form1 : Form
    {
        private Dashboard.A.A1 m_A1 = new A.A1();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_A1.SetA1(1, 80, 100, 120, 130);
            m_A1.Location = new Point(0, 0);
            this.Controls.Add(m_A1);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
    //密封类，封装需要用的画笔
    public sealed class SigPen
    {
        public static Pen GrayPen;
        public static Pen RedPen;
        public static Pen SkyBluePen;
        public static Pen BluePen;
        public static Pen GreenPen;
        public static Pen YellowPen;
        public static Pen BlackPen;

        static SigPen()
        {
            RedPen = Pens.Red;
            GrayPen = Pens.Gray;
            RedPen = Pens.Red;
            SkyBluePen = Pens.SkyBlue;
            BluePen = Pens.Blue;
            GreenPen = Pens.Green;
            YellowPen = Pens.Yellow;
            BlackPen = Pens.Black;
        }
    }
    //密封类，封装需要用的画刷
    public sealed class SigBrush
    {
        public static SolidBrush GrayBrush;
        public static SolidBrush RedBrush;
        public static SolidBrush SkyBlueBrush;
        public static SolidBrush BlueBrush;
        public static SolidBrush GreenBrush;
        public static SolidBrush YellowBrush;
        public static SolidBrush BlackBrush;
        public static SolidBrush OrangeBrush;

        static SigBrush()
        {
            GrayBrush = new SolidBrush(Color.Gray);
            RedBrush = new SolidBrush(Color.Red);
            SkyBlueBrush = new SolidBrush(Color.SkyBlue);
            BlueBrush = new SolidBrush(Color.Blue);
            GreenBrush = new SolidBrush(Color.LimeGreen);
            YellowBrush = new SolidBrush(Color.Yellow);
            BlackBrush = new SolidBrush(Color.Black);
            OrangeBrush = new SolidBrush(Color.Orange);
        }
    }
}
