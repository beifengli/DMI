using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Dashboard.A
{

    /// <summary>
    /// 制动预警时间
    /// </summary>
    public partial class A1 : UserControl
    {
        public uint uMode;//模式
        public double dbVtrain;//列车速度
        public double dbVperm;//允许速度
        public double dbVrelease;//开口速度

        public double dbVint;//干预速度
        private float dbIconWid;
        private float fPropor;
        

        
        public A1()
        {
            dbIconWid = 0;
            fPropor = 1;
            InitializeComponent();
        }

        public void SetA1(uint Mode, double vTrain, double vPerm, double vInt, double vRelease)
        {
            uMode = Mode;
            dbVtrain = vTrain;
            dbVperm = vPerm;
            dbVint = vInt;
            dbVrelease = vRelease;
        }

        private void A1_Paint(object sender, PaintEventArgs e)
        {
            
            e.Graphics.Clear(this.BackColor);
            Graphics g = e.Graphics;
            //设置绘图表面平滑模式
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Brush brushT = GetBrush();
            //绘制
            g.FillRectangle(brushT, Width / 2 - dbIconWid / 2, Height / 2 - dbIconWid / 2, dbIconWid, dbIconWid);
        }

        private uint GetState(double Vtrain,double Vperm)
        {
            if (Vtrain < Vperm || Vtrain == Vperm)
            {
                return 1;
            }
            else
            {
                if (Vperm < Vtrain &&
                    (Vtrain < dbVint || Vtrain == dbVint))
                {
                    return 2;
                }
                else
                {
                    if (Vtrain > dbVint)
                    {
                        return 3;
                    }
                    else
                    {
                        MessageBox.Show("DashBoard.A.A1.GetState()输入错误");
                        return 0;
                    }
                }

            }

        }

        private Brush GetBrush()
        {
            Brush A1Brush = Brushes.White;
            //CSM模式选择画笔
            uint bState = GetState(dbVtrain, dbVperm);

            switch (uMode)
            {
                case 0:
                    MessageBox.Show("输入模式错误");
                    break;
                case 1:
                    switch (bState)
                    {
                        case 0:
                            A1Brush = SigBrush.RedBrush;
                            break;
                        case 1:
                            A1Brush = SigBrush.GrayBrush;//灰色
                            break;
                        case 2:
                            A1Brush = SigBrush.OrangeBrush;
                            break;
                        case 3:
                            A1Brush = SigBrush.RedBrush;
                            break;
                        default:
                            break;
                    }
                    break;
                case 2:
                    switch (bState)
                    {
                        case 0:
                            A1Brush = SigBrush.RedBrush;
                            break;
                        case 1:
                            A1Brush = SigBrush.YellowBrush;//黄色
                            break;
                        case 2:
                            A1Brush = SigBrush.OrangeBrush;
                            break;
                        case 3:
                            A1Brush = SigBrush.RedBrush;
                            break;
                        default:
                            break;
                    }
                    break;
                case 3:
                    if (dbVrelease >= dbVtrain)
                    {
                        A1Brush = SigBrush.YellowBrush;
                    }
                    else
                    {
                        A1Brush = SigBrush.RedBrush;
                    }
                    break;
                default:
                    MessageBox.Show("DashBoard.A.A1.uMode输入错误");
                    break;
            }

            return A1Brush;
        }


    }


}
