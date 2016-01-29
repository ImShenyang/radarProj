using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using radarEchoSimulator.parameters;

namespace radarEchoSimulator
{
    class cRadar:cComm
    {
        private cRadarMode myRadarMode;
        private cRadarParam myRadarParam;
        private cTargetParam myTargetParam;
        private cJamParam myJamParam;
        private cOtherParam myOtherParam;
        private cMainRadar myMainRadar;
        
        public void catchRadarMode(eOperateMode aOperateMode,ePulseCmprsType aPulseCmpsType)
        {
            myRadarMode.OperateMode = aOperateMode;
            myRadarMode.APulseCmprsType = aPulseCmpsType;
        }
        public void catchRadarParam(cRadarMode aRM, double BW)
        {
            myRadarParam.BandWidth = BW;
        }
        public void catchJamParam(double b, Array jsr, JamType JT, int Num, int onoff,
            Array rangesample, double sigma, Array vsample)
        {
            myJamParam.B = b;
            myJamParam.JSR = jsr;
            myJamParam.MyJamType = JT;
            myJamParam.Num = (double)Num;
            myJamParam.Onoff = onoff;
            myJamParam.RangeSample = rangesample;
            myJamParam.Sigma = sigma;
            myJamParam.Vsample = vsample;
        }

    }
}
