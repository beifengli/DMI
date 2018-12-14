using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Socket通信_Server;


namespace simulation
{
    public partial class FormTrack : Form
    {
        public const int originX = 0;
        public const int originY = 0;
        //public Point[] trackSingle;
        public TRACK G1, G2, DG1, DG2, DG3, DG4, 
            G3, G4, G5, G6, G7, G8, G9, G10;
        public PLATFORM P1, P2, P3, P4;

        public SWITCH SW1, SW2, SW3, SW4;
        public SIGNAL X1, X2, X3, S1, S2, S4;
        
        public FormTrack()  //窗体构造函数
        {
            InitializeComponent();
            Graphics g = this.CreateGraphics();

            Point[] trackSingle = new Point[4];
            trackSingle[0].X = 0; trackSingle[0].Y = 0;
            trackSingle[1].X = 0; trackSingle[1].Y = 9;
            trackSingle[2].X = 240; trackSingle[2].Y = 9;
            trackSingle[3].X = 240; trackSingle[3].Y = 0;

            Point[] trackUpLeft = new Point[8];
            trackUpLeft[0].X =   0; trackUpLeft[0].Y = 0;
            trackUpLeft[1].X =   0; trackUpLeft[1].Y = 9;
            trackUpLeft[2].X =  58; trackUpLeft[2].Y = 9;
            trackUpLeft[3].X = 120; trackUpLeft[3].Y = 70;
            trackUpLeft[4].X = 120; trackUpLeft[4].Y = 56;
            trackUpLeft[5].X =  72; trackUpLeft[5].Y = 9;
            trackUpLeft[6].X = 240; trackUpLeft[6].Y = 9;
            trackUpLeft[7].X = 240; trackUpLeft[7].Y = 0;

            Point[] trackUpRight = new Point[8];
            trackUpRight[0].X = 240 -   0; trackUpRight[0].Y = 0;
            trackUpRight[1].X = 240 -   0; trackUpRight[1].Y = 9;
            trackUpRight[2].X = 240 -  58; trackUpRight[2].Y = 9;
            trackUpRight[3].X = 240 - 120; trackUpRight[3].Y = 70;
            trackUpRight[4].X = 240 - 120; trackUpRight[4].Y = 56;
            trackUpRight[5].X = 240 -  72; trackUpRight[5].Y = 9;
            trackUpRight[6].X = 240 - 240; trackUpRight[6].Y = 9;
            trackUpRight[7].X = 240 - 240; trackUpRight[7].Y = 0;

            Point[] trackDownLeft = new Point[8];
            trackDownLeft[0].X =   0; trackDownLeft[0].Y = 9 - 0;
            trackDownLeft[1].X =   0; trackDownLeft[1].Y = 9 - 9;
            trackDownLeft[2].X =  58; trackDownLeft[2].Y = 9 - 9;
            trackDownLeft[3].X = 119; trackDownLeft[3].Y = 8 - 70;
            trackDownLeft[4].X = 119; trackDownLeft[4].Y = 8 - 56;
            trackDownLeft[5].X =  71; trackDownLeft[5].Y = 9 - 9;
            trackDownLeft[6].X = 240; trackDownLeft[6].Y = 9 - 9;
            trackDownLeft[7].X = 240; trackDownLeft[7].Y = 9 - 0;

            Point[] trackDownRight = new Point[8];
            trackDownRight[0].X = 240 -   0; trackDownRight[0].Y = 9 - 0;
            trackDownRight[1].X = 240 -   0; trackDownRight[1].Y = 9 - 9;
            trackDownRight[2].X = 240 -  58; trackDownRight[2].Y = 9 - 9;
            trackDownRight[3].X = 241 - 120; trackDownRight[3].Y = 8 - 70;
            trackDownRight[4].X = 241 - 120; trackDownRight[4].Y = 8 - 56;
            trackDownRight[5].X = 241 -  72; trackDownRight[5].Y = 9 - 9;
            trackDownRight[6].X = 240 - 240; trackDownRight[6].Y = 9 - 9;
            trackDownRight[7].X = 240 - 240; trackDownRight[7].Y = 9 - 0;

            Point[] platformIslandUp = new Point[4];
            platformIslandUp[0].X =  75; platformIslandUp[0].Y = 23;
            platformIslandUp[1].X = 165; platformIslandUp[1].Y = 23;
            platformIslandUp[2].X = 165; platformIslandUp[2].Y = 58;
            platformIslandUp[3].X =  75; platformIslandUp[3].Y = 58;

            Point[] platformIslandDown = new Point[4];
            platformIslandDown[0].X =  75; platformIslandDown[0].Y = -13;
            platformIslandDown[1].X = 165; platformIslandDown[1].Y = -13;
            platformIslandDown[2].X = 165; platformIslandDown[2].Y = -48;
            platformIslandDown[3].X =  75; platformIslandDown[3].Y = -48;

            Point[] switchRightDownNormal = new Point[4];
            Point[] switchRightDownInverse = new Point[4];
            switchRightDownNormal [0].X =  -9; switchRightDownNormal [0].Y =  -9;
            switchRightDownNormal [1].X =   0; switchRightDownNormal [1].Y =   0;
            switchRightDownNormal [2].X = -37; switchRightDownNormal [2].Y =   0;
            switchRightDownNormal [3].X = -37; switchRightDownNormal [3].Y =  -9;
            switchRightDownInverse[0].X = -15; switchRightDownInverse[0].Y =   0;
            switchRightDownInverse[1].X =   0; switchRightDownInverse[1].Y =   0;
            switchRightDownInverse[2].X = -24; switchRightDownInverse[2].Y = -24;
            switchRightDownInverse[3].X = -39; switchRightDownInverse[3].Y = -24;

            Point[] switchLeftDownNormal = new Point[4];
            Point[] switchLeftDownInverse = new Point[4];
            switchLeftDownNormal[0].X = 9; switchLeftDownNormal[0].Y = -9;
            switchLeftDownNormal[1].X = 0; switchLeftDownNormal[1].Y = 0;
            switchLeftDownNormal[2].X = 37; switchLeftDownNormal[2].Y = 0;
            switchLeftDownNormal[3].X = 37; switchLeftDownNormal[3].Y = -9;
            switchLeftDownInverse[0].X = 16; switchLeftDownInverse[0].Y = 0;
            switchLeftDownInverse[1].X = 0; switchLeftDownInverse[1].Y = 0;
            switchLeftDownInverse[2].X = 24; switchLeftDownInverse[2].Y = -24;
            switchLeftDownInverse[3].X = 40; switchLeftDownInverse[3].Y = -24;

            Point[] switchRightUpNormal = new Point[4];
            Point[] switchRightUpInverse = new Point[4];
            switchRightUpNormal[0].X = -9; switchRightUpNormal[0].Y = 9;
            switchRightUpNormal[1].X = 0; switchRightUpNormal[1].Y = 0;
            switchRightUpNormal[2].X = -37; switchRightUpNormal[2].Y = 0;
            switchRightUpNormal[3].X = -37; switchRightUpNormal[3].Y = 9;
            switchRightUpInverse[0].X = -15; switchRightUpInverse[0].Y = 0;
            switchRightUpInverse[1].X = 0; switchRightUpInverse[1].Y = 0;
            switchRightUpInverse[2].X = -24; switchRightUpInverse[2].Y = 24;
            switchRightUpInverse[3].X = -39; switchRightUpInverse[3].Y = 24;

            Point[] switchLeftUpNormal = new Point[4];
            Point[] switchLeftUpInverse = new Point[4];
            switchLeftUpNormal[0].X = 9; switchLeftUpNormal[0].Y = 9;
            switchLeftUpNormal[1].X = 0; switchLeftUpNormal[1].Y = 0;
            switchLeftUpNormal[2].X = 37; switchLeftUpNormal[2].Y = 0;
            switchLeftUpNormal[3].X = 37; switchLeftUpNormal[3].Y = 9;
            switchLeftUpInverse[0].X = 15; switchLeftUpInverse[0].Y = 0;
            switchLeftUpInverse[1].X = 0; switchLeftUpInverse[1].Y = 0;
            switchLeftUpInverse[2].X = 24; switchLeftUpInverse[2].Y = 24;
            switchLeftUpInverse[3].X = 39; switchLeftUpInverse[3].Y = 24;



            G1 = new TRACK(trackSingle,         
                originX + 120, originY + 480,   
                label1G, null);
            G2 = new TRACK(trackSingle,
                originX + 120, originY + 360,
                label2G, null);
            DG1 = new TRACK(trackDownRight, 
                originX + 360, originY + 480, 
                label1DG, null);
            DG2 = new TRACK(trackUpLeft,
                originX + 360, originY + 360, 
                label2DG, null);
            DG3 = new TRACK(trackDownLeft,
                originX + 600, originY + 480,
                label1DG, null);
            DG4 = new TRACK(trackUpRight,
                originX + 600, originY + 360,
                label2DG, null);
            G3 = new TRACK(trackSingle,
                originX + 840, originY + 480,
                label3G, null);
            G4 = new TRACK(trackSingle,
                originX + 840, originY + 360,
                label4G, null);
            G5 = new TRACK(trackSingle,
                originX + 1080, originY + 480,
                label5G, null);
            G6 = new TRACK(trackSingle,
                originX + 1080, originY + 360,
                label6G, null);
            G7 = new TRACK(trackSingle,
                originX + 1320, originY + 480,
                label7G, null);
            G8 = new TRACK(trackSingle,
                originX + 1320, originY + 360,
                label8G, null);
            G9 = new TRACK(trackSingle,
                originX + 1560, originY + 480,
                label9G, null);
            G10 = new TRACK(trackSingle,
                originX + 1560, originY + 360,
                label10G, null);

            P1 = new PLATFORM(1, platformIslandDown, 
                originX + 120, originY + 480);
            P2 = new PLATFORM(2, platformIslandUp, 
                originX + 120, originY + 360);
            P3 = new PLATFORM(3, platformIslandDown, 
                originX + 1320, originY + 480);
            P4 = new PLATFORM(4, platformIslandUp, 
                originX + 1320, originY + 360);

            SW3 = new SWITCH(originX + 646, originY + 489,
                switchLeftDownNormal, switchLeftDownInverse, labelSW1);
            SW2 = new SWITCH(originX + 409, originY + 360,
                switchLeftUpNormal, switchLeftUpInverse, labelSW2);
            SW1 = new SWITCH(originX + 553, originY + 489,
                switchRightDownNormal, switchRightDownInverse, labelSW3);
            SW4 = new SWITCH(originX + 792, originY + 360,
                switchRightUpNormal, switchRightUpInverse, labelSW4);

            X1 = new SIGNAL(360, 480, labelX1, true);
            X2 = new SIGNAL(360, 360, labelX2, true);
            X3 = new SIGNAL(1560, 480, labelX3, true);
            S1 = new SIGNAL(840, 480, labelS1, false);
            S2 = new SIGNAL(840, 360, labelS2, false);
            S4 = new SIGNAL(1320, 360, labelS4, false);
        }

        private void FormTrack_Load(object sender, EventArgs e)     //窗体加载
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        private bool Init() //初始化
        {
            return true;
        }

        private void FormTrack_Paint(object sender, PaintEventArgs e)   //重画函数
        {
            Graphics g = this.CreateGraphics();
            G1.drawTrack(g);
            G2.drawTrack(g);
            DG1.drawTrack(g);
            DG2.drawTrack(g);
            DG3.drawTrack(g);
            DG4.drawTrack(g);
            G3.drawTrack(g);
            G4.drawTrack(g);
            G5.drawTrack(g);
            G6.drawTrack(g);
            G7.drawTrack(g);
            G8.drawTrack(g);
            G9.drawTrack(g);
            G10.drawTrack(g);
            P1.drawPlatform(g);
            P2.drawPlatform(g);
            P3.drawPlatform(g);
            P4.drawPlatform(g);
            SW1.drawSwitch(g);
            SW2.drawSwitch(g);
            SW3.drawSwitch(g);
            SW4.drawSwitch(g);
            X1.drawSignal(g);
            X2.drawSignal(g);
            X3.drawSignal(g);
            S1.drawSignal(g);
            S2.drawSignal(g);
            S4.drawSignal(g);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                //点击开始监听的时候 在服务器端创建
                //一个负责监听IP地址跟端口号的Socket
                Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Any;
                //创建端口号对象
                IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
                //监听
                socketWatch.Bind(point);
                ShowMsg("监听成功");
                socketWatch.Listen(10);

                Thread th = new Thread(Listen);
                th.IsBackground = true;
                th.Start(socketWatch);
            }
            catch
            {

            }
        }
        Socket socketSend;

        //将远程连接的客户端的IP地址和Socket存入集合中
        Dictionary<string, Socket> dicSocket = new Dictionary<string, Socket>();
        /// <summary>
        /// 等待客户端的连接 并且创建与之通信用的Socket
        /// </summary>
        void Listen(Object o)
        {
            Socket socketWatch = o as Socket;
            while (true)
            {
                try
                {
                    //等待客户端连接，并创建一个负责通信的Socket
                    socketSend = socketWatch.Accept();

                    /////////////////////
                    dicSocket.Add(socketSend.RemoteEndPoint.ToString(), socketSend);
                    comboBox1.Items.Add(socketSend.RemoteEndPoint.ToString());
                    //192.168...连接成功
                    ShowMsg(socketSend.RemoteEndPoint.ToString() + ":" + "连接成功");
                    //开启一个新线程，不停的接收客户端发送过来的消息
                    Thread th = new Thread(Recive);
                    th.IsBackground = true;
                    th.Start(socketSend);
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// 服务端不停的接收客户端发送过来的消息
        /// </summary>
        /// <param name="o"></param>
        void Recive(object o)
        {
            Socket socketSend = o as Socket;
            while (true)
            {
                try
                {
                    //客户端连接成功后，服务器应该接受客户端发来的消息
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    //实际接收到的有效字节数
                    int r = socketSend.Receive(buffer);
                    if (r == 0)
                        break;
                    string str = Encoding.UTF8.GetString(buffer, 0, r);
                    ShowMsg(socketSend.RemoteEndPoint + ":" + str);
                }
                catch
                {

                }
            }
        }
        void ShowMsg(string str)
        {
            txtLog.AppendText(str + "\r\n");
        }

        /// <summary>
        /// 服务器给客户端发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSend_Click(object sender, EventArgs e)
        {
            try
            {
                message mes = new message();
                //mes.USERDATA = System.Text.Encoding.UTF8.GetBytes(txtMsg.Text);
                /*
                string outstr = string.Empty;
                foreach (char iten in txtMsg.Text)
                {
                    int i = iten;
                    outstr += Convert.ToString(i, 2);
                }
                mes.USERDATA = outstr;
                */

                for (uint i = 0; i < 100; i++)
                {

                    //mes.USERDATA = txtMsg.Text;
                    mes.USERDATA = i.ToString();

                    string str = mes.packet();
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
                    List<byte> list = new List<byte>();
                    list.Add(0);
                    list.AddRange(buffer);
                    byte[] newBuffer = list.ToArray();
                    //获得用户在下拉框中选中的IP地址
                    string ip = comboBox1.SelectedItem.ToString();
                    dicSocket[ip].Send(newBuffer);
                    Thread.Sleep(500);
                    
                }
                //socketSend.Send(buffer);
            }
            catch
            {

            }
        }

    }
}

    public class TRACK              //轨道区段类
    {
        public bool isOccupied,     //占用
            isLocked;               //锁闭
        public Point[] outline,     //轮廓
            outlineNormal,          //定位轮廓
            outlineInverse,         //反位轮廓
            outlineInverse2;        //反位轮廓2
        private Label labelTrack,   //关联标签
            labelWindow;            //关联车次窗

        public TRACK(Point[] shape, int x, int y,
            Label labelTrack1, Label labelWindow1)
        {            //构造函数（无岔区段）
            outline = outlineInit(shape, x, y);
            labelTrack = labelTrack1;
            labelWindow = labelWindow1;
            isOccupied = false;
            isLocked = false;
        }

        public Point[] outlineInit(Point[] shape, int x, int y)     //初始化轮廓
        {
            Point[] shape1 = new Point[shape.Length];
            for (int i = 0; i < shape.Length; i++)
            {
                shape1[i].X = shape[i].X + x;
                shape1[i].Y = shape[i].Y + y;
            }
            return shape1;
        }

        public void drawTrack(Graphics g)                       //画轨道区段
        {
            g.FillPolygon(Brushes.LightBlue, outline);
            drawIsolation(g);
        }

        private void drawIsolation(Graphics g)
        {
            try
            {
                Point temp;
                for (int i = 0; i < outline.Length; i++)  //遍历所有点
                {
                    if (i < outline.Length - 1)  //不是最后一个点
                    {
                        //相邻点的横坐标相同，画绝缘节
                        if ((outline[i].X == outline[i + 1].X)
                            //&& (System.Math.Abs(outline[i].Y - outline[i + 1].Y) == 6)
                            )
                        {
                            if (outline[i].Y < outline[i + 1].Y)
                            {
                                temp = outline[i];
                            }
                            else
                            {
                                temp = outline[i + 1];
                            }
                            Rectangle r = new Rectangle(temp.X - 2, temp.Y - 2, 2, 15);
                            g.FillRectangle(Brushes.SteelBlue, r);
                        }
                    }
                    else if (i == outline.Length - 1)    //是最后一个点
                    {
                        //相邻点的横坐标相同，画绝缘节
                        if (outline[i].X == outline[0].X)
                        {
                            if (outline[i].Y < outline[0].Y)
                            {
                                temp = outline[i];
                            }
                            else
                            {
                                temp = outline[0];
                            }
                            Rectangle r = new Rectangle(temp.X - 2, temp.Y - 2, 2, 15);
                            g.FillRectangle(Brushes.SteelBlue, r);
                        }
                    }
                }
            }
            catch { }
        }

    }

    public class PLATFORM
    {
        public int ID;
        public bool isDoorOpen;
        public Point[] outline;

        public PLATFORM(int ID1, Point[] shape, int x, int y)
        {
            ID = ID1;
            outline = outlineInit(shape, x, y);
            isDoorOpen = false;
        }

        public Point[] outlineInit(Point[] shape, int x, int y)     //初始化轮廓
        {
            Point[] shape1 = new Point[shape.Length];
            for (int i = 0; i < shape.Length; i++)
            {
                shape1[i].X = shape[i].X + x;
                shape1[i].Y = shape[i].Y + y;
            }
            return shape1;
        }

        public void drawPlatform(Graphics g)
        {
            g.FillPolygon(Brushes.LightBlue, outline);
        }
    }

    public class SWITCH
    {
        public bool isNormal, isInverse;
        private Point[] outlineNormal, outlineInverse;
        private Label labelSwitch;

        public SWITCH(int x, int y, Point[] shapeNormal, Point[] shapeInverse, Label label1)
        {
            outlineNormal = outlineInit(shapeNormal, x, y);
            outlineInverse = outlineInit(shapeInverse, x, y);
            isNormal = true;
            isInverse = false;
            labelSwitch = label1;
        }

        public Point[] outlineInit(Point[] shape, int x, int y)     //初始化轮廓
        {
            Point[] shape1 = new Point[shape.Length];
            for (int i = 0; i < shape.Length; i++)
            {
                shape1[i].X = shape[i].X + x;
                shape1[i].Y = shape[i].Y + y;
            }
            return shape1;
        }

        public void drawSwitch(Graphics g)
        {
            if (isNormal && !isInverse)
            {
                g.FillPolygon(Brushes.Black, outlineInverse);
                g.FillPolygon(Brushes.LightGreen, outlineNormal);
                labelSwitch.ForeColor = Color.LightGreen;
            }
            else if (!isNormal && isInverse)
            {
                g.FillPolygon(Brushes.Black, outlineNormal);
                g.FillPolygon(Brushes.Yellow, outlineInverse);
                labelSwitch.ForeColor = Color.Yellow;
            }
            else
            {
                g.FillPolygon(Brushes.Blue, outlineNormal);
                g.FillPolygon(Brushes.Blue, outlineInverse);
                labelSwitch.ForeColor = Color.HotPink;
            }
        }
    }

    public class SIGNAL
    {
        public int status;              //0红，1黄，2绿
        private Rectangle circle;
        private Rectangle line;
        private Label labelSignal;

        public SIGNAL(int x, int y, Label label1, bool isHeadToRight)
        {
            status = 0;
            labelSignal = label1;
            if (isHeadToRight)
            {
                line = new Rectangle(x - 2, y + 15, 2, 15);
                circle = new Rectangle(x, y + 14, 14, 14);
            }
            else
            {
                line = new Rectangle(x - 2, y - 19, 2, 15);
                circle = new Rectangle(x - 16, y - 18, 14, 14);
            }
        }

        public void drawSignal(Graphics g)
        {
            Pen pen = new Pen(Brushes.SteelBlue);
            g.DrawEllipse(pen, circle);
            g.FillRectangle(Brushes.SteelBlue, line);
            if (status == 1)
            {
                g.FillEllipse(Brushes.Yellow, circle);
            }
            else if (status == 2)
            {
                g.FillEllipse(Brushes.LightGreen, circle);
            }
            else
            {
                g.FillEllipse(Brushes.Tomato, circle);
            }
        }
    }

namespace Socket通信_Server
{
    class message
    {
        private string _Q_UPDOWN;
        private string _M_VERSION;
        private string _Q_MEDIA;
        private string _N_PIG;
        private string _N_TOTAL;
        private string _M_DUP;
        private string _M_MCOUNT;
        private string _NID_C;
        private string _NID_BG;
        private string _Q_LINK;

        private string _USERDATA;
        private string _END;

        public string Q_UPDOWN
        {
            get { return _Q_UPDOWN; }
            set
            {
                _Q_UPDOWN = value;
            }
        }

        public string M_VERSION
        {
            get { return _M_VERSION; }
            set
            {
                _M_VERSION = value;
            }
        }

        public string Q_MEDIA
        {
            get { return _Q_MEDIA; }
            set
            {
                _Q_MEDIA = value;
            }
        }

        public string N_PIG
        {
            get { return _N_PIG; }
            set
            {
                _N_PIG = value;
            }
        }

        public string N_TOTAL
        {
            get { return _N_TOTAL; }
            set
            {
                _N_TOTAL = value;
            }
        }
        public string M_DUP
        {
            get { return _M_DUP; }
            set
            {
                _M_DUP = value;
            }
        }

        public string M_MCOUNT
        {
            get { return _M_MCOUNT; }
            set
            {
                _M_MCOUNT = value;
            }
        }

        public string NID_C
        {
            get { return _NID_C; }
            set
            {
                _NID_C = value;
            }
        }

        public string NID_BG
        {
            get { return _NID_BG; }
            set
            {
                _NID_BG = value;
            }
        }

        public string Q_LINK
        {
            get { return _Q_LINK; }
            set
            {
                _Q_LINK = value;
            }
        }


        public string USERDATA
        {
            get { return _USERDATA; }
            set
            {
                _USERDATA = value;
            }
        }

        public string END
        {
            get { return _END; }
            set
            {
                _END = value;
            }
        }


        public message()
        {
            //信息传送的方向 (0=车对地，1=地对车)
            Q_UPDOWN = "1";
            //语言/代码版本编号 (0010000=V1.0)
            M_VERSION = "0010000";
            //信息传输媒介 (0=应答器，1=环线)
            Q_MEDIA = "0";
            //本应答器在应答器组中的位置 (000=1,111=8)
            N_PIG = "000";
            //应答器组中所包含的应答器数量 (000=1,111=8)
            N_TOTAL = "001";
            //本应答器信息与前/后应答器信息的关系 (00=不同，01=与后一个相同，10=与前一个相同)
            M_DUP = "00";
            //报文计数器(0～255)
            M_MCOUNT = "00000001";
            //地区编号(高7位=大区编号，低3位=分区编号)
            NID_C = "0000000000";
            //应答器标识号 (高6位=车站编号，低8位=应答器编号)
            NID_BG = "00000000000000";
            //应答器组的链接关系 (0=不被链接，1=被链接)
            Q_LINK = "1";

            //用户信息包区
            USERDATA = "";
            //=11111111，表示信息帧结束
            END = "11111111";
        }

        public string packet()
        {
            string str = "";
            str = this.Q_UPDOWN +
                this.M_VERSION +
                this.Q_MEDIA +
                this.N_PIG +
                this.N_TOTAL +
                this.M_DUP +
                this.M_MCOUNT +
                this.NID_C +
                this.NID_BG +
                this.Q_LINK +
                //this.packet() +
                this.USERDATA +
                this.END;

            return str;
        }

        public string encode()
        {
            string s = USERDATA;
            byte[] data = Encoding.Unicode.GetBytes(s);
            StringBuilder result = new StringBuilder(data.Length * 8);


            foreach (byte b in data)
            {
                result.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            return result.ToString();
        }

    }
}

