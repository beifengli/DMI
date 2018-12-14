using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighSpeedMaglevSYS
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
                this.packet() +
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

        public string uncode()
        {
            string s = USERDATA;
            System.Text.RegularExpressions.CaptureCollection cs =
                  System.Text.RegularExpressions.Regex.Match(s, @"([01]{8})+").Groups[1].Captures;
            byte[] data = new byte[cs.Count];
            for (int i = 0; i < cs.Count; i++)
            {
                data[i] = Convert.ToByte(cs[i].Value, 2);
            }
            return Encoding.Unicode.GetString(data, 0, data.Length);
        }
    }
}
