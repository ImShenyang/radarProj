using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace radarEchoSimulator.parameters
{
    class cRadarParam:cComm
    {
        //雷达工作模式：脉冲多普勒，脉冲压缩，频率捷变，连续波
        private eOperateMode operateMode;

        ///雷达参数：
        ///
        // 共有
        private double fc;//载频
        //public double PRF;//脉冲重复频率,范围：100Hz-350KHz
        private double prfUserSet;
        private double prfMin;
        private double prfNum;//prf个数
        private double[] prfSet;//prf数组
        private double cPINum;//CPI个数
        // 使距离不模糊的PRF选择        
        // 该状态用来判断PRF是否正确，若不正确，自动修改，修改后的值要进行显示
        private double bandWidth;//信号带宽：M
        private double sNR;//信噪比
        private double fs;//采样率
        //public double DeltaR()
        //{
        //    return C / Fs / 2;
        //}
        //public double JudgePRF(double prf)
        //{
        //    if (prf >= C / 2 / rgMax())
        //    {
        //        prf = C / 2 / rgMax();
        //    }
        //    return prf;
        //}
        //public double findPrfMin(double[] prfSet)
        //{
        //    double prfmin = prfSet[0];
        //    for (int i = 0; i < PrfNum; i++)
        //    {
        //        if (prfmin > prfSet[i])
        //            prfmin = prfSet[i];
        //    }
        //    return prfmin;
        //}

        private double dutyRatio; ///占空比///PD; pulsecompression-LFM,SFM; FrequencyAgility
        public double Tr(eOperateMode mode, double prf)
        {
            if (mode == eOperateMode.pulseCompressionRadar && aPulseCmprsType == ePulseCmprsType.BPSK)
                return codeT() * numByte;
            else if (eOperateMode.FMCW == mode)
                return 1 / prf;
            return DutyRatio / prf;
        }//脉冲宽度范围:0.05us-2ms
        public double K(eOperateMode mode, double prf)
        {
            if (eOperateMode.pulseCompressionRadar == mode
                && (ePulseCmprsType.SFM == aPulseCmprsType || ePulseCmprsType.BPSK == aPulseCmprsType))
                return -1; //SFM模式无此参数
            return BandWidth / Tr(mode, prf);
        }
        private double nan;

        public eOperateMode OperateMode
        {
            get
            {
                return operateMode;
            }

            set
            {
                operateMode = value;
            }
        }

        public double Fc
        {
            get
            {
                return fc;
            }

            set
            {
                fc = value;
            }
        }

        public double PrfUserSet
        {
            get
            {
                return prfUserSet;
            }

            set
            {
                prfUserSet = value;
            }
        }

        public double PrfMin
        {
            get
            {
                return prfMin;
            }

            set
            {
                prfMin = value;
            }
        }

        public double PrfNum
        {
            get
            {
                return prfNum;
            }

            set
            {
                prfNum = value;
            }
        }

        public double[] PrfSet
        {
            get
            {
                return prfSet;
            }

            set
            {
                prfSet = value;
            }
        }

        public double CPINum
        {
            get
            {
                return cPINum;
            }

            set
            {
                cPINum = value;
            }
        }

        public double BandWidth
        {
            get
            {
                return bandWidth;
            }

            set
            {
                bandWidth = value;
            }
        }

        public double SNR
        {
            get
            {
                return sNR;
            }

            set
            {
                sNR = value;
            }
        }

        public double Fs
        {
            get
            {
                return fs;
            }

            set
            {
                fs = value;
            }
        }

        public double DutyRatio
        {
            get
            {
                return dutyRatio;
            }

            set
            {
                dutyRatio = value;
            }
        }

        public double Nan
        {
            get
            {
                return nan;
            }

            set
            {
                nan = value;
            }
        }

        //public void setNan(double nan, eOperateMode mode)
        //{
        //    if (mode == eOperateMode.frequencyAgileRadar)
        //        this.Nan = NumPlsGrounp * NumFreq;
        //    else
        //        this.Nan = nan;
        //}
        //public double nrn(eOperateMode mode, double prf)
        //{
        //    double tmp;
        //    if (eOperateMode.FMCW == mode)
        //        tmp = Tr(mode, prf) * Fs;
        //    else
        //        tmp = (rgMax() - rgMin()) / DeltaR() + Tr(mode, prf) * Fs;

        //    return Math.Pow(2, Math.Ceiling(Math.Log(tmp, 2)));
        //}
        //private double[] sphereEffect_factor(int sphereEffect, double numMov, double[] reMov)
        //{
        //    double[] factor = new double[(int)numMov];
        //    switch (sphereEffect)
        //    {
        //        case 0:
        //            for (int i = 0; i < numMov; i++)
        //                factor[i] = 1;
        //            break;
        //        case 1:
        //        case 2:
        //            for (int i = 0; i < numMov; i++)
        //                factor[i] = Math.Pow(10, (-2 * reMov[i] / 1e3 * 0.01 / 20));  //0.01db/km
        //            break;
        //        case 3:
        //            for (int i = 0; i < numMov; i++)
        //                factor[i] = Math.Pow(10, (-2 * reMov[i] / 1e3 * 0.032 / 20));  //0.032db/km
        //            break;
        //        case 4:
        //            for (int i = 0; i < numMov; i++)
        //                factor[i] = Math.Pow(10, (-2 * reMov[i] / 1e3 * 0.04 / 20));  // 0.04db/km
        //            break;
        //    }
        //    return factor;
        //}


        //特有
        
            
            
        ///PD
        //脉冲多普勒雷达重频类型（脉组）
        public eRepeatedFrequancyType aRFType;
        //pulseCompression:LFM SFM BPSK
        public ePulseCmprsType aPulseCmprsType;
        //SFM
        public double m_f;//        param_sfm.m_f = 1;   % 正弦调制指数 SFM
        public double f_m;//      param_sfm.f_m = 10;  % 正弦调制频率 SFM
        //BPSK
        public double numByte;//BPSK

        public double codeT()
        {
            return 1 / BandWidth;
        }
        public double[] setwvfm(double prf)
        {
            int tmp = (int)Math.Ceiling(Fs * Tr(eOperateMode.pulseCompressionRadar, prf) / numByte);
            double[] dataTrans = { -1, -1, -1, -1, -1, 1, 1, -1, -1, 1, -1, 1, -1 };
            int num = 13 * tmp;
            double[] wvfm = new double[num];
            int i = 0;
            foreach (double var in dataTrans)
            {
                if (var == 1)
                {
                    for (int j = i; j < i + tmp; j++)
                        wvfm[j] = 1;
                }
                else
                {
                    for (int j = i; j < i + tmp; j++)
                        wvfm[j] = 0;
                }
                i += tmp;
            }
            return dataTrans;
        }
        //FrequencyAgility
        public double NumFreq;         //param_freqag.N_freq = 4;%捷变频个数
        public double scanOffset;         //param_freqag.scanOffset = 10e6;  % 跳变间隔
        public double NumPlsGrounp;         //param_freqag.plsgroupNum = 32;%脉冲数

        //性能分析参数：
        public double exp_num;
        public bool[] Case;


        /// <summary>
        /// 初始化参数
        /// </summary>
        public cRadarParam()
        {
            aRFType = eRepeatedFrequancyType.typeConstant;
            aPulseCmprsType = ePulseCmprsType.NULL;
            m_f = 50000;
            f_m = 10000;
            numByte = 13;
            NumFreq = 4;
            scanOffset = 10000000;
            NumPlsGrounp = 32;

            exp_num = 10;
            Case = new bool[3];
            Case[0] = false;
            Case[1] = false;
            Case[2] = false;
        }

    }
}
