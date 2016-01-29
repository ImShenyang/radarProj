using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace radarEchoSimulator.parameters
{
    class cComm
    {
        public static double C = 300000000;//light velocity

        public enum JamType
        {
            NULL, BlanketJamming, VelocityJamming, RangeJamming
        };
        public enum eprobdistr
        {
            NULL, Rayleigh, Weibull, lognormal, K, Sea
        };
        public enum Tgtfluct
        {
            Zero, I, II, III, IV, V
        }; //设置枚举类型：0-5，分为五种swerling 0，I，II，III，IV，V
        public enum eRepeatedFrequancyType
        {
            typeConstant, typeIrregular, typeTremble
        };
        public enum ePulseCmprsType
        {
            NULL = 0, BPSK = 3, LFM = 1, SFM = 2
        };//时宽带宽积>=1000
        public enum eOperateMode
        {
            pulsedDopplerRadar, pulseCompressionRadar,//pluseCompressRadar,
            frequencyAgileRadar, FMCW,
        };


    }
}
