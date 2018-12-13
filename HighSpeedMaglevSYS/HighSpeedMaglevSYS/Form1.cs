using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HighSpeedMaglevSYS
{ 
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            bA1Mode = 0x10;  //FS  CSM
            dbVperm = 80;
            dbVint = 85;
            dbVrelease = 30;
            dbVtrain = 75;


            //注册自定义事件
            TimerUpdate += new event_Handle(changedEvent1);
            //A1EventHandler A1Hand = new A1EventHandler(this.changedEvent1);
            //changedEvent1 += A1Hand;

            
            InitializeComponent();
        }



        /// <summary>
        /// 距离监控信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public byte bA1Mode;//控制模式
        public double dbVtrain;//列车速度
        public double dbVperm;//允许速度
        public double dbVrelease;//开口速度
        public double dbVint;//干预速度

        private float dbIconWid=54;//色块宽度

        //A1Event A1EventTemp = new A1Event();
        public delegate void event_Handle(object sender, EventArgs e);  // 自定义事件的参数类型
        public event event_Handle TimerUpdate;  

        /// <summary>
        /// TCP/IP通信
        /// DMI为客户端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        Socket socketSend;
        private void socketStart()
        {
            //创建负责通信的Socket
            socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32("50000"));
            //获得要连接的远程服务器应用程序的IP地址和端口号
            socketSend.Connect(point);
            ShowMsg("连接成功");

            Thread th = new Thread(Recive);
            th.IsBackground = true;
            th.Start();
        }

        string txtlog ="";
        void ShowMsg(string str)
        {
            txtlog += str + "\r\n";
            
        }

        /// <summary>
        /// 不停的接收服务器发来的消息
        /// </summary>
        void Recive()
        {
            while (true)
            {
                byte[] buffer = new byte[1024 * 1024 * 2];
                int r = socketSend.Receive(buffer);
                //实际接收到的有效字节数
                if (r == 0)
                {
                    break;
                }

                if (buffer[0] == 0)
                {

                    string s = Encoding.UTF8.GetString(buffer, 1, r - 1);
                    ShowMsg(socketSend.RemoteEndPoint + ":" + s);
                }
                else if (buffer[0] == 1)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.InitialDirectory = @"C:\Users\Xuheyao\Desktop";
                    sfd.Title = "请选择要保存的文件";
                    sfd.Filter = "所有文件|*.*";
                    sfd.ShowDialog(this);

                    string path = sfd.FileName;
                    using (FileStream fsWrite = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        fsWrite.Write(buffer, 1, r - 1);
                    }
                    MessageBox.Show("保存成功");
                }
                else if (buffer[0] == 2)
                {
                    ZD();
                }
            }
        }

        private void A_Paint(object sender, PaintEventArgs e)
        {

            /*
             * 
             A1区：制动预警时间
             A2区：目标距离
             A3区：预留
             * 
             * */

            //初始化
            e.Graphics.Clear(this.BackColor);
            Graphics g = e.Graphics;
            //设置绘图表面平滑模式
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            this.DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //A1区绘制
            Brush a1Brush = a1GetBrush();
            g.FillRectangle(a1Brush, 0, 0, dbIconWid, dbIconWid);

           


        }
        
        //public delegate void A1EventHandler(object sender, EventArgs e);
        public double dbVtrainOld;//列车速度

        //列车速度改变事件
        public void changedEvent1(object sender, EventArgs e)
        {
            //...
            if (dbVtrain != dbVtrainOld)
            {
                //MessageBox.Show("vTrain changed");
                this.A.Invalidate();
                dbVtrainOld = dbVtrain;
            }
        }

       

        /// <summary>
        /// 获取A1的画刷颜色
        /// </summary>
        /// <returns></returns>
        private Brush a1GetBrush()
        {
            Brush A1Brush = Brushes.White;
            switch (bA1Mode)
            {
                case 0x10://FS CSM
                    if (dbVtrain <= dbVperm)
                        A1Brush = SigBrush.GrayBrush;
                    else
                    {
                        if (dbVperm < dbVtrain && dbVtrain <= dbVint)
                            A1Brush = SigBrush.OrangeBrush;
                        else
                        {
                            if (dbVtrain > dbVint)
                                A1Brush = SigBrush.RedBrush;
                            else
                                MessageBox.Show("Vtrain 无效");
                        }
                    }
                    break;
                case 0x11://FS TSM
                    if (dbVtrain <= dbVperm)
                        A1Brush = SigBrush.YellowBrush;
                    else
                    {
                        if (dbVperm < dbVtrain && dbVtrain <= dbVint)
                            A1Brush = SigBrush.OrangeBrush;
                        else
                        {
                            if (dbVtrain > dbVint)
                                A1Brush = SigBrush.RedBrush;
                            else
                                MessageBox.Show("Vtrain 无效");
                        }
                    }
                    break;
                case 0x13://FS RSM
                    if (dbVtrain <= dbVrelease)
                        A1Brush = SigBrush.YellowBrush;
                    else
                    {
                        if (dbVtrain > dbVrelease)
                            A1Brush = SigBrush.RedBrush;
                        else
                            MessageBox.Show("Vtrain 无效");
                    }
                    break;
                default:
                    break;
            }
            return A1Brush;

        }


        /// <summary>
        /// 速度增加测试
        /// </summary>
        private void testVtrain()
        {
            dbVtrain+=0.5;
            if (90 == dbVtrain)
                dbVtrain = 75;
        }













        /// <summary>
        /// 速度信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_Paint(object sender, PaintEventArgs e)
        {

        }


        /// <summary>
        /// 补充驾驶信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void C_Paint(object sender, PaintEventArgs e)
        {

        }


        /// <summary>
        /// 运行计划信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void D_Paint(object sender, PaintEventArgs e)
        {

        }


        /// <summary>
        /// 监控信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void E_Paint(object sender, PaintEventArgs e)
        {

        }



        /// <summary>
        /// 功能键信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void F_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            testVtrain();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (TimerUpdate != null) TimerUpdate(this, new EventArgs());//调用自定义事件
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dbVtrain++;
            //this.A.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
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
