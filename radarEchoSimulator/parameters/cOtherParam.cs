using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace radarEchoSimulator.parameters
{

    class cOtherParam:cComm
    {
        /// 杂波类型
        ///         
        ///杂波干扰类型：回波中叠加地杂波，支持瑞利分布，
        ///对数正态分布，韦伯尔分布和k分布杂波统计模型，
        /// 可以模拟海杂波；
        /// 
        //public enum eprobdistr { NULL, Rayleigh, Weibull, lognormal, K, Sea };
        private eprobdistr aProbDistr;
        private double probparam1;
        private double probparam2;
        private double probparam3;
        private double probparam4;

        ///目标起伏类型
        ///
        //public enum Tgtfluct { Zero, I, II, III, IV, V }; //设置枚举类型：0-5，分为五种swerling 0，I，II，III，IV，V
        private Tgtfluct myTrgtfluct;

        ///云雨雾雪传输损耗：
        ///目标回波强度应体现云、雨、雾、雪环境下的传输损失 ;
        public double sphereEffect;//晴0云1 雾2 雨3 雪4

        internal eprobdistr AProbDistr
        {
            get
            {
                return aProbDistr;
            }

            set
            {
                aProbDistr = value;
            }
        }

        internal Tgtfluct MyTrgtfluct
        {
            get
            {
                return myTrgtfluct;
            }

            set
            {
                myTrgtfluct = value;
            }
        }

        public double Probparam1
        {
            get
            {
                return probparam1;
            }

            set
            {
                probparam1 = value;
            }
        }

        public double Probparam2
        {
            get
            {
                return probparam2;
            }

            set
            {
                probparam2 = value;
            }
        }

        public double Probparam3
        {
            get
            {
                return probparam3;
            }

            set
            {
                probparam3 = value;
            }
        }

        public double Probparam4
        {
            get
            {
                return probparam4;
            }

            set
            {
                probparam4 = value;
            }
        }
    }
}
