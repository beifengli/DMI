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
            ///A1测试
            bA1Mode = 0x11;  //FS  CSM
            dbVperm = 80;
            dbVint = 85;
            dbVrelease = 30;
            dbVtrain = 75;

            strTrainNum = "G1212";
            //byteDirection = 0X01;


            ///A2测试
            iEOF = 500;
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


        //A1
        //控制模式
        private byte _bA1Mode;
        public byte bA1Mode
        {
            get { return _bA1Mode; }
            set {
                _bA1Mode = value;
            }
        }

        //列车速度
        private double _dbVtrain;
        public double dbVtrain
        {
            get { return _dbVtrain; }
            set
            {
                _dbVtrain=value;
                if (null != this.A)
                {
                    this.A.Invalidate();
                }
                if (null != this.B)
                {
                    this.B.Invalidate();
                }
            }
        }

        //允许速度
        private double _dbVperm;
        public double dbVperm
        {
            get { return _dbVperm; }
            set
            {
                _dbVperm = value;
            }
        }


        //开口速度
        private double _dbVrelease;
        public double dbVrelease
        {
            get { return _dbVrelease; }
            set {
                _dbVrelease = value;
            }
        }

        //干预速度
        private double _dbVint;
        public double dbVint
        {
            get { return _dbVint; }
            set
            {
                _dbVint = value;
            }
        }


        private float dbIconWid=54;//色块宽度


        ///A2
        public int iEOF;
        public delegate void event_Handle(object sender, EventArgs e);  // 自定义事件的参数类型
        public event event_Handle TimerUpdate;


        private bool _serverConnect;
        private bool serverConnect
        {
            get { return _serverConnect; }
            set {
                _serverConnect = value;

            }
        }
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
            try
            {
                socketSend.Connect(point);

                serverConnect = true;

            }
            catch (SocketException e)
            {
                // 10035 == WSAEWOULDBLOCK
                if (e.NativeErrorCode.Equals(10035))
                {
                    Console.WriteLine("Still Connected, but the Send would block");
                    serverConnect = false;
                }
                else
                {
                    Console.WriteLine("Disconnected: error code {0}!", e.NativeErrorCode);
                }

            }
            finally
            { 
                if(true == serverConnect)
                    ShowMsg("连接成功");
                else
                    ShowMsg("连接失败");
            }
            

            Thread th = new Thread(Recive);
            th.IsBackground = true;
            th.Start();
        }

        string txtlog = "";
        void ShowMsg(string str)
        {
            DateTime time = DateTime.Now;
            strLog = time.ToString("d") +" "+time.ToString("t")+ "  " + str + "\r\n"+strLog;

        }

        message mes;
        /// <summary>
        /// 不停的接收服务器发来的消息
        /// </summary>
        void Recive()
        {
            if (false == socketSend.Connected)
                serverConnect = false;
            
            while (serverConnect)
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

                    string sTemp1 = s.Substring(50);
                    string sTemp2 = sTemp1.Substring(0, sTemp1.Length - 8);
                    //string[] strResule=sTemp.Split("11111111",StringSplitOptions.None);

                    dbVtrain = int.Parse(sTemp2);
                    ShowMsg(socketSend.RemoteEndPoint + ":" + sTemp2);
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
                    //ZD();
                }
            }
        }


        public static string uncode(string s)
        {
            System.Text.RegularExpressions.CaptureCollection cs =
                   System.Text.RegularExpressions.Regex.Match(s, @"([01]{8})+").Groups[1].Captures;
            byte[] data = new byte[cs.Count];
            for (int i = 0; i < cs.Count; i++)
            {
                data[i] = Convert.ToByte(cs[i].Value, 2);
            }
            return Encoding.Unicode.GetString(data, 0, data.Length);
        }
        /// <summary>
        /// A区域绘制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void A_Paint(object sender, PaintEventArgs e)
        {

            /*
             * 
             A1区：制动预警时间
             A2区：目标距离
             A3区：预留
             * 
             * */

            //双缓冲初始化
           
            
            Graphics g1 = e.Graphics;
            

            g1.Clear(BackColor);
            g1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Bitmap bit = new Bitmap(54, 300);
            Graphics g = Graphics.FromImage(bit);
            DrawImageA(g);

            ///将多个图形一次性绘制到窗体
            

            g1.DrawImage(bit, new Point(0, 0));
            //销毁
            
            g.Dispose();
            bit.Dispose();
            g1.Dispose();
            
        }


        private void DrawImageA(Graphics g)
        {
            //A1区绘制
            Brush a1Brush = a1GetBrush();
            g.FillRectangle(a1Brush, 0, 0, dbIconWid, dbIconWid);


            //A2绘制
            //如果FS 中的TSM RSM模式，显示目标距离，其余不显示
            Brush a2Brush = Brushes.White;
            double dbEOFlength = 0;
            if (0x11 == bA1Mode || 0x12 == bA1Mode)
            {
                if (1000 >= iEOF)
                {
                    dbEOFlength = (iEOF / 1000.0) * 172;
                }
                else
                {
                    dbEOFlength = 172;
                }

                
                ///光带
                int xyEOF = 104 + (172 - (int)dbEOFlength);

                Pen a2Pen = new Pen(Color.White);
                a2Pen.Width = 15;
                g.DrawLine(a2Pen, new PointF(41F, 276-(float)Math.Ceiling(dbEOFlength)), new PointF(41F,270));
                //g.FillRectangle(a2Brush,new RectangleF( 34F,(float)xyEOF,15F,(float)Math.Ceiling(dbEOFlength)));
               // 34, xyEOF, 15, Math.Round(dbEOFlength));
                
                // 数字
                String drawString = iEOF.ToString();
                g.DrawString(drawString, new Font("Arial", 13), Brushes.White, new PointF(0.0F, 104 - 30));

                //刻度
                Pen thickPen = new Pen(Brushes.White);
                thickPen.Width = 3;

                g.DrawLine(thickPen, new Point(5, 104 + 2), new Point(32, 104 + 2));
                for (int i = 1; i < 5; i++)
                {
                    int inter = 2 + i * 5;
                    g.DrawLine(Pens.White, new Point(15, 104 + inter), new Point(30, 104 + inter));
                }
                g.DrawLine(thickPen, new Point(5, 104 + 27), new Point(32, 104 + 27));
                g.DrawLine(thickPen, new Point(5, 270), new Point(32, 270));
            }
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
                //this.A.Invalidate();
                //DrawPin((float)dbVtrain);
                //this.B.Invalidate();
                
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
            dbVtrain+=1;
            if (250 == dbVtrain)
                dbVtrain = 75;

            iEOF+=10;
            if (1500 == iEOF)
                iEOF = 500;
        }













        /// <summary>
        /// 速度信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_Paint(object sender, PaintEventArgs e)
        {
            
            //双缓冲初始化
            Graphics g = e.Graphics;
            g.Clear(BackColor);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

           
            Bitmap bit = new Bitmap(280, 300);
            Graphics g1 = Graphics.FromImage(bit);

            //g1.Clear(Color.Transparent);
            g1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            DrawImageBit1(g1);

            /*
            Bitmap bit2 = new Bitmap(this.E.Width, this.E.Height);
            Graphics g2 = Graphics.FromImage(bit2);
            g2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //DrawPin(g2, (float)dbVtrain);
            DrawImageBit2(g2);
            */
            ///将多个图形一次性绘制到窗体
            g.DrawImage(bit, new Point(0, 0));
            //g.DrawImage(bit2, new Point(0, 0));
            //g.DrawImage(bit2,new Point(0,0));
           

            //销毁
            //bitmap2Graphics.Dispose();
            g1.Dispose();
            //g2.Dispose();
            bit.Dispose();
            g.Dispose();
        }

        private void DrawImageBit1(Graphics g)
        {
            //g.Clear(System.Drawing.Color.Transparent);

            drawFrame(g);
            DrawRuling1(g);
            drawPoint(g);
            DrawPin(g, (float)dbVtrain);
        }

        private void DrawImageBit2()
        {
            Graphics g = this.B.CreateGraphics();
            g.Clear(System.Drawing.Color.Transparent);

            Bitmap bit = new Bitmap(this.E.Width, this.E.Height);
            Graphics gbit = Graphics.FromImage(bit);
            gbit.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
            DrawPin(g, (float)dbVtrain);

            g.DrawImage(bit, new Point(0, 0));

            gbit.Dispose();
            g.Dispose();
            bit.Dispose();
        }

        /// <summary>
        /// 绘制环形速度表框线
        /// </summary>
        /// <param name="gp"></param>
        private void drawFrame(Graphics g)
        {
            // Create pen.
            Pen blackPen = new Pen(Color.White, 9);

            // Create coordinates of rectangle to bound ellipse.
            float x = 10.0F;
            float y = 10.0F;
            float width = 260.0F;
            float height = 260.0F;

            // Create start and sweep angles on ellipse.
            float startAngle = 135.0F;
            float sweepAngle = 270.0F;

            g.DrawArc(blackPen, x, y, width, height, startAngle, sweepAngle);
        }

        /// <summary>
        /// 绘制环形速度表刻度前部分
        /// </summary>
        /// <param name="gp"></param>
        private void DrawRuling1(Graphics gp)
        {

            int _diameter = 260;
            Color _frameColor = Color.White;
            float _maxValue = 450;
            //刻度
            int cerX = _diameter / 2+10;
            int cerY = _diameter / 2+10;

            //这里需要注意，因外在上面的图中标识了rad=0的位置，而我们的仪表时270度的，0点在135度处，

            //为了符合该效果所以起始位置设为135度。
            float start = 135;
            float sweepShot = 0;
            int dx = 0;
            int dy = 0;
            int soildLenght = 25;
            Pen linePen = new Pen(_frameColor, 1);
            float span = (float)(_maxValue / 30);
            float sp = 0;
            //用于右边数字右对齐
            StringFormat stf = new StringFormat();
            stf.Alignment = StringAlignment.Far;

            StringFormat stfMid = new StringFormat();
            stfMid.Alignment = StringAlignment.Center;
            stfMid.LineAlignment = StringAlignment.Center;
            for (int i = 0; i <= 30; i++)
            {
                //注意此处，C#提供的三角函数计算中使用的弧度值，而此处获取的是角度值，需要转化

                double rad = (sweepShot + start) * Math.PI / 180;
                float radius = _diameter / 2 - 5;
                int px = (int)(cerX + radius * Math.Cos(rad));
                int py = (int)(cerY + radius * Math.Sin(rad));
                if (sweepShot % 15 == 0)
                {
                    linePen.Width = 2;

                    //计算刻度中的粗线
                    dx = (int)(cerX + (radius - soildLenght) * Math.Cos(rad));
                    dy = (int)(cerY + (radius - soildLenght) * Math.Sin(rad));

                    //绘制刻度值，注意字串对其方式
                    string str = sp.ToString("f0");
                    if (sweepShot < 45)
                    {
                        gp.DrawString(str, new Font("Arial", 16), new SolidBrush(_frameColor), new PointF(dx-10, dy - 25));
                    }
                    else if (45 == sweepShot)
                    {
                        gp.DrawString(str, new Font("Arial", 16), new SolidBrush(_frameColor), new PointF(dx + 10, dy-10));
                    }
                    else if (sweepShot > 45 && sweepShot < 135)
                    {
                        gp.DrawString(str, new Font("Arial", 16), new SolidBrush(_frameColor), new PointF(dx, dy));
                    }
                    else if (sweepShot == 135)
                    {
                        gp.DrawString(str, new Font("Arial", 16), new SolidBrush(_frameColor), new PointF(dx, dy + 20), stfMid);
                    }
                    else if (sweepShot > 135 && sweepShot < 225)
                    {
                        gp.DrawString(str, new Font("Arial", 16), new SolidBrush(_frameColor), new PointF(dx, dy), stf);
                    }
                    else if (225 == sweepShot)
                    {
                        gp.DrawString(str, new Font("Arial", 16), new SolidBrush(_frameColor), new PointF(dx - 45, dy - 10));
                    }
                    else if (sweepShot >= 225)
                    {
                        gp.DrawString(str, new Font("Arial", 16), new SolidBrush(_frameColor), new PointF(dx + 10, dy - 25), stf);
                    }

                }
                else
                {

                    //计算刻度中细线

                    linePen.Width = 1;
                    dx = (int)(cerX + (radius - soildLenght + 10) * Math.Cos(rad));
                    dy = (int)(cerY + (radius - soildLenght + 10) * Math.Sin(rad));
                }

                //绘制刻度线
                gp.DrawLine(linePen, new Point(px, py), new Point(dx, dy));
                sp += span;
                sweepShot += 9;
            }
        }

        /// <summary>
        /// 绘制环形速度表刻度后部分
        /// </summary>
        /// <param name="gp"></param>
        private void DrawRuling2(Graphics gp)
        {

            int _diameter = 260;
            Color _frameColor = Color.White;
            float _maxValue = 450;
            //刻度
            int cerX = _diameter / 2 + 10;
            int cerY = _diameter / 2 + 10;

            //这里需要注意，因外在上面的图中标识了rad=0的位置，而我们的仪表时270度的，0点在135度处，

            //为了符合该效果所以起始位置设为135度。
            float start = 270;
            float sweepShot = 0;
            int dx = 0;
            int dy = 0;
            int soildLenght = 25;
            Pen linePen = new Pen(_frameColor, 1);
            float span = (float)(_maxValue / 30);
            float sp = 150;
            //用于右边数字右对齐
            StringFormat stf = new StringFormat();
            stf.Alignment = StringAlignment.Far;

            StringFormat stfMid = new StringFormat();
            stfMid.Alignment = StringAlignment.Center;
            stfMid.LineAlignment = StringAlignment.Center;
            for (int i = 0; i <= 30; i++)
            {
                //注意此处，C#提供的三角函数计算中使用的弧度值，而此处获取的是角度值，需要转化

                double rad = (sweepShot + start) * Math.PI / 180;
                float radius = _diameter / 2 - 5;
                int px = (int)(cerX + radius * Math.Cos(rad));
                int py = (int)(cerY + radius * Math.Sin(rad));
                if (sweepShot % 24.165 == 0)
                {
                    linePen.Width = 2;

                    //计算刻度中的粗线
                    dx = (int)(cerX + (radius - soildLenght) * Math.Cos(rad));
                    dy = (int)(cerY + (radius - soildLenght) * Math.Sin(rad));

                    //绘制刻度值，注意字串对其方式
                    string str = sp.ToString("f0");
                    if (sweepShot < 45)
                    {
                        gp.DrawString(str, new Font("Arial", 16), new SolidBrush(_frameColor), new PointF(dx - 10, dy - 25));
                    }
                    else if (45 == sweepShot)
                    {
                        gp.DrawString(str, new Font("Arial", 16), new SolidBrush(_frameColor), new PointF(dx + 10, dy - 10));
                    }
                    else if (sweepShot > 45 && sweepShot < 135)
                    {
                        gp.DrawString(str, new Font("Arial", 16), new SolidBrush(_frameColor), new PointF(dx, dy));
                    }
                    else if (sweepShot == 135)
                    {
                        gp.DrawString(str, new Font("Arial", 16), new SolidBrush(_frameColor), new PointF(dx, dy + 20), stfMid);
                    }
                    else if (sweepShot > 135 && sweepShot < 225)
                    {
                        gp.DrawString(str, new Font("Arial", 16), new SolidBrush(_frameColor), new PointF(dx, dy), stf);
                    }
                    else if (225 == sweepShot)
                    {
                        gp.DrawString(str, new Font("Arial", 16), new SolidBrush(_frameColor), new PointF(dx - 45, dy - 10));
                    }
                    else if (sweepShot >= 225)
                    {
                        gp.DrawString(str, new Font("Arial", 16), new SolidBrush(_frameColor), new PointF(dx + 10, dy - 25), stf);
                    }

                }
                else
                {

                    //计算刻度中细线

                    linePen.Width = 1;
                    dx = (int)(cerX + (radius - soildLenght + 10) * Math.Cos(rad));
                    dy = (int)(cerY + (radius - soildLenght + 10) * Math.Sin(rad));
                }

                //绘制刻度线
                gp.DrawLine(linePen, new Point(px, py), new Point(dx, dy));
                sp += span;
                sweepShot += 4.8F;
            }
        }

        /// <summary>
        /// 数字方式显示列车速度
        /// </summary>
        /// <param name="gp"></param>
        private void drawPoint(Graphics gp)
        {
            Color _frameColor = Color.White;
            int _diameter = 260;
            Color _pinColor = Color.White;

            Pen p = new Pen(_frameColor);
            int tmpWidth = 25;
            int px = _diameter / 2 - tmpWidth;
            //gp.DrawEllipse(p, new Rectangle(px+10, px+10, 2 * tmpWidth, 2 * tmpWidth));
            gp.FillEllipse(new SolidBrush(_pinColor), new Rectangle(px + 10, px + 10, 2 * tmpWidth, 2 * tmpWidth));

            
        }


        /// <summary>
        /// 指针
        /// </summary>
        /// <param name="g"></param>
        private void DrawPin(Graphics g, float _changeValue)
        {



            int _diameter = 260;
            float _maxValue=450;
            Color _pinColor = Color.White;
            double _PinLen = 100;
            double NxPinLen = 50;
 
            int cer = _diameter / 2+10;
            float start = 135;
            float sweepShot = (float)(_changeValue / _maxValue * 270);

            Pen linePen1 = new Pen(_pinColor, 3);
            Pen linePen2 = new Pen(_pinColor, 5);
            Pen linePen3 = new Pen(_pinColor, 7);
            Pen linePen4 = new Pen(_pinColor, 9);
            double rad = (sweepShot + start) * Math.PI / 180;
            float radius = _diameter / 2 - 5;

            int dx = (int)(cer + (_PinLen) * Math.Cos(rad));
            int dy = (int)(cer + (_PinLen) * Math.Sin(rad));

            /*
            int px = (int)(cer + (_PinLen * 0.4) * Math.Cos(rad));
            int py = (int)(cer + (_PinLen * 0.4) * Math.Sin(rad));
            */
            int dx2 = (int)(cer + (_PinLen-15) * Math.Cos(rad));
            int dy2 = (int)(cer + (_PinLen-15) * Math.Sin(rad));

            int dx3 = (int)(cer + (_PinLen - 20) * Math.Cos(rad));
            int dy3 = (int)(cer + (_PinLen - 20) * Math.Sin(rad));

            int dx4 = (int)(cer + (_PinLen - 25) * Math.Cos(rad));
            int dy4 = (int)(cer + (_PinLen - 25) * Math.Sin(rad));

            int nx = (int)(cer - (NxPinLen) * Math.Sin(rad));
            int ny = (int)(cer - (NxPinLen) * Math.Cos(rad));

            double r=25 * Math.Cos(rad);
            g.DrawLine(linePen1, new PointF(cer, cer), new PointF(dx, dy));
            g.DrawLine(linePen2, new PointF(cer, cer), new PointF(dx2, dy2));
            g.DrawLine(linePen3, new PointF(cer, cer), new PointF(dx3, dy3));
            g.DrawLine(linePen4, new PointF(cer, cer), new PointF(dx4, dy4));
            //g.DrawLine(NxPen, new Point(cer, cer), new Point(px, py));
            //g.DrawLine(xPen, new Point(cer, cer), new Point(ny, nx));


            int tmpWidth = 25;
            int pxStr = _diameter / 2 - tmpWidth;
            if (100 > dbVtrain)
            {
                g.DrawString(this.dbVtrain.ToString(), new Font("Arial", 16), Brushes.Black, new PointF(pxStr + 20F, pxStr + 24F));
            }
            else
            {
                g.DrawString(this.dbVtrain.ToString(), new Font("Arial", 16), Brushes.Black, new PointF(pxStr + 13F, pxStr + 24F));
            }
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


        private string _strLog = "";

        public string strLog
        {
            get { return _strLog; }
            set {
                _strLog = value;
                if (null != this.E)
                    this.E.Invalidate();
            }
        
        }
        /// <summary>
        /// 监控信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void E_Paint(object sender, PaintEventArgs e)
        {
           
            Graphics g = e.Graphics;
            g.Clear(BackColor);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Bitmap bit = new Bitmap(578, 180);
            Graphics gbit = Graphics.FromImage(bit);

            gbit.Clear(BackColor);
            gbit.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //strLog="测试开始";

            DrawImageE(gbit);
            //文本信息
            gbit.DrawString(getLog(strLog), new Font("Arial", 10), Brushes.White, new PointF(54F, 54F));



            g.DrawImage(bit, new Point(0, 0));

            gbit.Dispose();
            bit.Dispose();
            g.Dispose();
        }


        private string _strTrainNum;
        public string strTrainNum
        {
            get { return _strTrainNum; }
            set
            {
                _strTrainNum = value;
            }
        }
        private sbyte _byteDirection;
        public sbyte byteDirection
        {
            get { return _byteDirection; }
            set
            {
                _byteDirection = value;
            }
        }

        private sbyte _byteGSMState;
        public sbyte byteGSMState
        {
            get { return _byteGSMState; }
            set
            {
                _byteGSMState = value;
            }
        }
        /// <summary>
        /// E绘图函数
        /// </summary>
        /// <param name="g"></param>
        private void DrawImageE(Graphics g)
        {

            //E5 机控/人控
            if (0x11 == bA1Mode || 0x12 == bA1Mode || 0x13 == bA1Mode)
            {
                g.DrawString("机控", new Font("Arial", 10), Brushes.White, new PointF(0, 84F));
            }
            else
            {
                g.DrawString("人控", new Font("Arial", 10), Brushes.White, new PointF(0, 84F));
            }

            //E1 备用系统状态
            g.DrawString("C2正常", new Font("Arial", 10), Brushes.White, new PointF(0, 114F));

            //E24
            Point point1 = new Point(311, 57);
            Point point2 = new Point(301, 77);
            Point point3 = new Point(306, 77);
            Point point4 = new Point(306, 107);
            Point point5 = new Point(316, 107);
            Point point6 = new Point(316, 77);
            Point point7 = new Point(321, 77);

            Point[] pE24 = { point1, point2, point3, point4, point5, point6,point7};

            g.FillPolygon(Brushes.White, pE24);

            //E25
            Point point8 = new Point(311, 177);
            Point point9 = new Point(301, 157);
            Point point10 = new Point(306, 157);
            Point point11 = new Point(306, 127);
            Point point12 = new Point(316, 127);
            Point point13 = new Point(316, 157);
            Point point14 = new Point(321, 157);

            Point[] pE25 = { point8, point9, point10, point11, point12, point13, point14 };

            g.FillPolygon(Brushes.White, pE25);
            /*
            if (0x00 == byteDirection)
            {
                g.FillPolygon(Brushes.White, pE24);
            }
            else
            {
                g.FillPolygon(Brushes.White, pE25);
            }
             * */
            

            //E16a
            g.DrawString("车次号：", new Font("Arial", 10), Brushes.White, new PointF(336, 120F));
            g.DrawString(strTrainNum, new Font("Arial", 10), Brushes.White, new PointF(336, 150F));

            //E16b

            Pen penE16b = new Pen(Color.White);
            penE16b.Width = 3;
            g.DrawLine(penE16b, new PointF(383F, 117F),new PointF(388F,122F));
            switch (byteGSMState)
            {
                case 0x00:
                    break;
                case 0x01:
                    break;
                case 0x02:
                    break;
                default:
                    break;
            }

        }
         

        /// <summary>
        /// 获取日志
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string getLog(string str)
        {
            string strRe="";
            string[] split = str.Split('\n');
            if (5 <= split.Length)
            {
                for (uint i = 0; i < 4; i++)
                {
                    strRe += split[i] + "\n";
                }
            }
            else
            {
                strRe = str;
            }
            return strRe;
        }


        /// <summary>
        /// 功能键信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void F_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// 单元测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            //testVtrain();
        }


        /// <summary>
        /// 状态更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (TimerUpdate != null) TimerUpdate(this, new EventArgs());//调用自定义事件
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dbVtrain++;
            //this.A.Invalidate();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void B_MouseHover(object sender, EventArgs e)
        {
            
            
        }

        private void B_MouseMove(object sender, MouseEventArgs e)
        {
            //Control.MousePosition

            Point screenPoint = Control.MousePosition;//鼠标相对于屏幕左上角的坐标
            string str = screenPoint.X.ToString() + "," + screenPoint.Y.ToString();
            //this.toolStripStatusLabel1.Text = str;
             
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            strLog += "\t测试";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            socketStart();
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
