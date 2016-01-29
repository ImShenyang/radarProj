using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace radarEchoSimulator.parameters
{
    class cTargetParam:cComm
    {
        ///场景参数 
        /// 
        /// </summary>
        private double rangeStartCltr;//仿真起始距离
        private double rangeEndCltr;//仿真结束距离
        private double rangeStartMov;//最近目标距离. 距离范围: 0.75-450km
        private double rangeEndMov;//最远目标距离
        private double numberMov;//运动目标个数/模拟目标回波数目不少于20个
        private double vrMin;//起始速度. 目标速度范围0-3000m/s
        private double vrMax;//终止速度
        private double[] vrSet;//目标速度集合

        public double RangeStartCltr
        {
            get
            {
                return rangeStartCltr;
            }

            set
            {
                rangeStartCltr = value;
            }
        }

        public double RangeEndCltr
        {
            get
            {
                return rangeEndCltr;
            }

            set
            {
                rangeEndCltr = value;
            }
        }

        public double RangeStartMov
        {
            get
            {
                return rangeStartMov;
            }

            set
            {
                rangeStartMov = value;
            }
        }

        public double RangeEndMov
        {
            get
            {
                return rangeEndMov;
            }

            set
            {
                rangeEndMov = value;
            }
        }

        public double NumberMov
        {
            get
            {
                return numberMov;
            }

            set
            {
                numberMov = value;
            }
        }

        public double VrMin
        {
            get
            {
                return vrMin;
            }

            set
            {
                vrMin = value;
            }
        }
                public double VrMax
        {
            get
            {
                return vrMax;
            }

            set
            {
                vrMax = value;
            }
        }

        public double[] VrSet
        {
            get
            {
                return vrSet;
            }

            set
            {
                vrSet = value;
            }
        }

        //public double rgMin()
        //{
        //    return Math.Min(RangeStartCltr, RangeStartMov);
        //}
        //public double rgMax()
        //{
        //    return Math.Max(RangeEndCltr, RangeEndMov);
        //}
        //public double[] rg_mov()
        //{
        //    //return linspace(RangeStartMov, RangeEndMov, NumberMov);
        //    return linspace(RangeStartMov + (RangeEndMov - RangeStartMov) * 1 / 4, RangeEndMov, NumberMov);

        //}//%[range_star+100*dis,range_star+200*dis,range_star+400*dis];%目标所在距离（三个）
        //public double[] vr()
        //{
        //    double[] tmp = randVr(VrMin, VrMax, NumberMov);
        //    double[] tmp2 = randGenerate(1, 3, NumberMov);
        //    for (int i = 0; i < NumberMov; i++)
        //    {
        //        tmp[i] = tmp[i] * tmp2[i];
        //    }
        //    return tmp;
        //}//目标径向速度  , 目标速度有正负
        //public double orgpnt()
        //{
        //    return Math.Max(rgMin() * 3 / 4, 0);
        //}

        protected double[] randGenerate(int rangeStart, int rangeEnd, int Num)
        {
            Random rand = new Random();
            double[] randv = new double[Num];
            for (int i = 0; i < Num; i++)
            {
                randv[i] = 2 * (rand.Next(1, 3) - 1) - 1;
            }
            return randv;
        }
        protected double[] randGenerate(double rangeStart, double rangeEnd, double Num)
        {
            Random rand = new Random();
            double[] randv = new double[(int)Num];
            for (int i = 0; i < Num; i++)
            {
                randv[i] = 2 * (rand.Next(1, 3) - 1) - 1;
            }
            return randv;
        }
        protected double[] randVr(double Vrmin, double Vrmax, double Num)
        {
            Random rand = new Random();
            double[] randv = new double[(int)Num];
            for (int i = 0; i < Num; i++)
            {
                randv[i] = rand.NextDouble() * (Vrmax - Vrmin) + Vrmin;
            }
            return randv;
        }//产生随机速度

    }
}
