using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace radarEchoSimulator.parameters
{
    class cRadarMode:cComm
    {
        //雷达工作模式：脉冲多普勒，脉冲压缩，频率捷变，连续波
        private eOperateMode operateMode;

        //脉冲压缩类型：pulseCompression:LFM SFM BPSK
        private ePulseCmprsType aPulseCmprsType;

        internal eOperateMode OperateMode
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

        internal ePulseCmprsType APulseCmprsType
        {
            get
            {
                return aPulseCmprsType;
            }

            set
            {
                aPulseCmprsType = value;
            }
        }
    }
}
