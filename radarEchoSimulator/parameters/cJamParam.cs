using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace radarEchoSimulator.parameters
{


    class cJamParam:cComm
    {
        /// 三种干扰类型参数设置: 
        ///    
        private double rangeStart;
        private double rangeEnd;
        private double num;

        private Array rangeSample;
        //public double[] rangeSample()
        //{
        //    return linspace(rangeStart, rangeEnd, num);
        //}
        private double sigma;
        private JamType myJamType;
        //public enum JamType { NULL, BlanketJamming, VelocityJamming, RangeJamming };
        private int onoff;
        //'BlanketJamming'     %压制干扰
        private double jsr;
        private Array jSR;
        private double b;
        //'VelocityJamming'    % 速度欺骗干扰
        private double vrStart;
        private double vrEnd;

        private Array vSample;

        public double RangeStart
        {
            get
            {
                return rangeStart;
            }

            set
            {
                rangeStart = value;
            }
        }

        public double RangeEnd
        {
            get
            {
                return rangeEnd;
            }

            set
            {
                rangeEnd = value;
            }
        }

        public double Num
        {
            get
            {
                return num;
            }

            set
            {
                num = value;
            }
        }

        public Array RangeSample
        {
            get
            {
                return rangeSample;
            }

            set
            {
                rangeSample = value;
            }
        }

        public double Sigma
        {
            get
            {
                return sigma;
            }

            set
            {
                sigma = value;
            }
        }

        public JamType MyJamType
        {
            get
            {
                return myJamType;
            }

            set
            {
                myJamType = value;
            }
        }

        public int Onoff
        {
            get
            {
                return onoff;
            }

            set
            {
                onoff = value;
            }
        }

        public double Jsr
        {
            get
            {
                return jsr;
            }

            set
            {
                jsr = value;
            }
        }

        public Array JSR
        {
            get
            {
                return jSR;
            }

            set
            {
                jSR = value;
            }
        }

        public double B
        {
            get
            {
                return b;
            }

            set
            {
                b = value;
            }
        }

        public double VrStart
        {
            get
            {
                return vrStart;
            }

            set
            {
                vrStart = value;
            }
        }

        public double VrEnd
        {
            get
            {
                return vrEnd;
            }

            set
            {
                vrEnd = value;
            }
        }

        public Array Vsample
        {
            get
            {
                return vSample;
            }

            set
            {
                vSample = value;
            }
        }

        //public double[] vSample()
        //{
        //    return linspace(vrStart, vrEnd, num);
        //}
        public cJamParam(double rs, double re, double n, double b, double sig, JamType myJT, int onoff, double K_fm, double vrStrt, double vrEd)
        {
            this.RangeStart = rs;
            this.RangeEnd = re;
            this.Num = n;
            //this.Rangesample = null;
            this.Vsample = null;
            this.JSR = null;
            this.B = b;
            this.Sigma = sig;
            this.Jsr = K_fm;
            this.MyJamType = myJT;
            this.Onoff = onoff;
            this.VrStart = vrStrt;
            this.VrEnd = vrEd;
        }

        public cJamParam()
        {

        }
    }
}
