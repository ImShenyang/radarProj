using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mRadarProcessings;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using System.IO;
using radarEchoSimulator.parameters;

namespace radarEchoSimulator
{

    class cMainRadar :cComm
    {
        #region staticVar
        static string[] JamParamNames = {
                               "range_start",//unused
                               "range_end",//unused
                               "num",
                               "rangesample",
                               "Sigma",
                               "JamType",
                               "onoff",
                               "jsr",
                               "vr_start",//unused
                               "vr_end",//unused
                               "vsample",
                               "B"//12.23 add
                               };


        static string[] paramNames = {
                                     "c",
                                     "range_star_cltr",
                                     "range_end_cltr",
                                     "range_star_mov",
                                     "range_end_mov",
                                     "number_mov",
                                     "vr_min",
                                     "vr_max",
                                     "rg_min",
                                     "rg_max",
                                     "rg_mov",
                                     "vr",
                                      "probdistr",
                                     "probparam1",
                                     "probparam2",
                                     "probparam3",
                                     "probparam4",
                                     "tgtfluct",
                                     "sphereeffect",
                                    "fc",
                                     "prf",
                                     "bandwidth",
                                     "snr",
                                     "Fs",
                                     "deltaR",
                                     "nan",
                                     "nrn",
                                     "Tr",
                                      "radarMode",
                                     "sphere_factor",
                                     "slow_time_start"
                                };
        static string[] paramNames_PD = {
                                     "dutyRatio",
                                     "K"
                                };

        static string[] paramNames_PC_LFM = {
                                     "dutyRatio",
                                     "K"
                               };

        static string[] paramNames_PC_SFM = {
                                     "dutyRatio",
                                     "m_f",
                                     "f_m"
                                     //"K",
                                 };

        static string[] paramNames_PC_BPSK = {
                                     "bnum",
                                     "codeT",
                                     "wvfm"
                                 };

        static string[] paramNames_FA = {
                                     "dutyRatio",
                                     "K",
                                     "N_freq",
                                     "scanOffset",
                                     "plsgroupNum"
                                 };

        static string[] paramNames_FMCW = {
                                     "K"
                                 };

        #endregion

        #region RadarParam

        //private mRadarProcessings.mRadarLib mRadarV2 = new mRadarProcessings.mRadarLib();
        private mRadarLib mRadar = new mRadarLib();

        //inputs
        private MWStructArray MWParam = null;// = new MWStructArray(1, 1, paramNames_PD);
        private MWStructArray MWJamParam = null;// new MWStructArray(1, 1, JamParamNames);

        //outputs
        private MWStructArray MWRadarResult = new MWStructArray();

        //results:
        private long rowOfEchoData;
        private long columnOfEchoData;
        private double maxValOfEchoData;
        private MWArray echoDataOfFile;
        private bool selfCycle = false;
        private Array echoReal;
        private Array echoImag;
        private Array imgReal;
        private Array imgImag;
        private Array mtiimgReal;
        private Array mtiimgImag;
        private Array detected;
        private Array aggmask;
        private Array idx_r_v;
        private Array idx_r_v_a;//目标距离，速度，角度
        private Array xValue;
        private Array yValue1;
        private Array yValue2;
        private Array yValue3;

        private Array yValue_FFT;
        private Array yValue_time;

        public long RowOfEchoData
        {
            get
            {
                return rowOfEchoData;
            }

            set
            {
                rowOfEchoData = value;
            }
        }

        public long ColumnOfEchoData
        {
            get
            {
                return columnOfEchoData;
            }

            set
            {
                columnOfEchoData = value;
            }
        }

        public double MaxValOfEchoData
        {
            get
            {
                return maxValOfEchoData;
            }

            set
            {
                maxValOfEchoData = value;
            }
        }

        public MWArray EchoDataOfFile
        {
            get
            {
                return echoDataOfFile;
            }

            set
            {
                echoDataOfFile = value;
            }
        }

        public bool SelfCycle
        {
            get
            {
                return selfCycle;
            }

            set
            {
                selfCycle = value;
            }
        }

        public Array EchoReal
        {
            get
            {
                return echoReal;
            }

            set
            {
                echoReal = value;
            }
        }

        public Array EchoImag
        {
            get
            {
                return echoImag;
            }

            set
            {
                echoImag = value;
            }
        }

        public Array ImgReal
        {
            get
            {
                return imgReal;
            }

            set
            {
                imgReal = value;
            }
        }

        public Array ImgImag
        {
            get
            {
                return imgImag;
            }

            set
            {
                imgImag = value;
            }
        }

        public Array MtiimgReal
        {
            get
            {
                return mtiimgReal;
            }

            set
            {
                mtiimgReal = value;
            }
        }

        public Array MtiimgImag
        {
            get
            {
                return mtiimgImag;
            }

            set
            {
                mtiimgImag = value;
            }
        }

        public Array Detected
        {
            get
            {
                return detected;
            }

            set
            {
                detected = value;
            }
        }

        public Array Aggmask
        {
            get
            {
                return aggmask;
            }

            set
            {
                aggmask = value;
            }
        }

        public Array Idx_r_v
        {
            get
            {
                return idx_r_v;
            }

            set
            {
                idx_r_v = value;
            }
        }

        public Array Idx_r_v_a
        {
            get
            {
                return idx_r_v_a;
            }

            set
            {
                idx_r_v_a = value;
            }
        }

        public Array XValue
        {
            get
            {
                return xValue;
            }

            set
            {
                xValue = value;
            }
        }

        public Array YValue1
        {
            get
            {
                return yValue1;
            }

            set
            {
                yValue1 = value;
            }
        }

        public Array YValue2
        {
            get
            {
                return yValue2;
            }

            set
            {
                yValue2 = value;
            }
        }

        public Array YValue3
        {
            get
            {
                return yValue3;
            }

            set
            {
                yValue3 = value;
            }
        }

        public Array YValue_FFT
        {
            get
            {
                return yValue_FFT;
            }

            set
            {
                yValue_FFT = value;
            }
        }

        public Array YValue_time
        {
            get
            {
                return yValue_time;
            }

            set
            {
                yValue_time = value;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// 参数操作方法
        /// </summary>
        /// 
        private void setRadarMode(cRadarMode aRadarMode)
        {

        }
        private cRadarMode getRadarMode()
        {
            cRadarMode aRadarMode = new cRadarMode();

            return aRadarMode;
        }

        private void initiAllParam(cRadarMode aRadarMode)
        {
            List<string> paramList = new List<string>();
            foreach (string str in paramNames)
                paramList.Add(str); //

            switch (aRadarMode.OperateMode)
            {
                case eOperateMode.pulsedDopplerRadar:
                    foreach (string str in paramNames_PD)
                        paramList.Add(str);
                    break;
                case eOperateMode.pulseCompressionRadar:
                    if (aRadarMode.APulseCmprsType == ePulseCmprsType.LFM)
                        foreach (string str in paramNames_PC_LFM)
                            paramList.Add(str);
                    else if (aRadarMode.APulseCmprsType == ePulseCmprsType.SFM)
                        foreach (string str in paramNames_PC_SFM)
                            paramList.Add(str);
                    else if (aRadarMode.APulseCmprsType == ePulseCmprsType.BPSK)
                        foreach (string str in paramNames_PC_BPSK)
                            paramList.Add(str);
                    break;
                case eOperateMode.frequencyAgileRadar:
                    foreach (string str in paramNames_FA)
                        paramList.Add(str);
                    break;
                case eOperateMode.FMCW:
                    foreach (string str in paramNames_FMCW)
                        paramList.Add(str);
                    break;
                default:
                    break;
            }
            MWParam = new MWStructArray(1, 1, paramList.ToArray());
            MWJamParam = new MWStructArray(1, 1, JamParamNames);


            return;
        }

        private void setRadarParams(cRadarParam aRadarParm)
        {
            return;
        }
        private cRadarParam getRadarParams()
        {
            cRadarParam aRadarParam = new cRadarParam();

            return aRadarParam;
        }

        private void setTargetParam(cTargetParam aTargetParam)
        {

        }
        private cTargetParam getTargetParam()
        {
            cTargetParam aTargetParam = new cTargetParam();

            return aTargetParam;
        }

        private void setJamParams(cJamParam aJamParam)
        {
            //MWJamParam.SetField("range_start", aJamParam.RangeStart);
            //MWJamParam.SetField("range_end", aJamParam.RangeEnd);
            MWJamParam.SetField("num", aJamParam.Num);
            MWJamParam.SetField("Sigma", aJamParam.Sigma);
            MWJamParam.SetField("JamType", aJamParam.MyJamType.ToString());//干扰类型
            MWJamParam.SetField("onoff", aJamParam.Onoff);//干扰开关
            MWJamParam.SetField("jsr", (MWNumericArray)aJamParam.JSR);//干性比
            MWJamParam.SetField("B", aJamParam.B);
            //MWJamParam.SetField("vr_start", aJamParam.vrStart);
            //MWJamParam.SetField("vr_end", aJamParam.vrEnd);

            MWJamParam.SetField("rangesample", (MWNumericArray)aJamParam.RangeSample);//欺骗目标距离
            MWJamParam.SetField("vsample", (MWNumericArray)aJamParam.Vsample);//欺骗目标速度

        }
        private cJamParam getJamParams()
        {
            cJamParam aJamParam = new cJamParam();
            //aJamParam.rangeStart = ((MWNumericArray)MWJamParam.GetField("range_start")).ToScalarDouble();
            //aJamParam.rangeEnd = ((MWNumericArray)MWJamParam.GetField("range_end")).ToScalarDouble();
            aJamParam.Num = ((MWNumericArray)MWJamParam.GetField("num")).ToScalarDouble();
            aJamParam.Sigma = ((MWNumericArray)MWJamParam.GetField("Sigma")).ToScalarDouble();
            aJamParam.Onoff = ((MWNumericArray)MWJamParam.GetField("onoff")).ToScalarInteger();
            aJamParam.JSR = ((MWNumericArray)MWJamParam.GetField("jsr")).ToArray();
            aJamParam.B = ((MWNumericArray)MWJamParam.GetField("B")).ToScalarDouble();

            if ("VelocityJamming" == ((MWCharArray)MWJamParam.GetField("JamType")).ToString())
            {
                aJamParam.MyJamType = JamType.VelocityJamming;
                //aJamParam.vrStart = ((MWNumericArray)MWJamParam.GetField("vr_start")).ToScalarDouble();
                //aJamParam.vrEnd = ((MWNumericArray)MWJamParam.GetField("vr_end")).ToScalarDouble();
                aJamParam.Vsample = ((MWNumericArray)MWJamParam.GetField("vsample")).ToArray();
                aJamParam.RangeSample = ((MWNumericArray)MWJamParam.GetField("rangesample")).ToArray();

            }
            else if ("BlanketJamming" == ((MWCharArray)MWJamParam.GetField("JamType")).ToString())
            {
                aJamParam.MyJamType = JamType.BlanketJamming;
            }
            return aJamParam;
        }

        private void setOtherParam(cOtherParam aOtherParam)
        {

        }
        private cOtherParam getOtherParam()
        {
            cOtherParam aOtherParam = new cOtherParam();

            return aOtherParam;
        }

        public void saveParams(string fileName)
        {

        }
        public void loadParams(string fileName)
        {

        }

        /// <summary>
        /// 雷达处理方法
        /// </summary>
        private void processings()
        {

        }
        private void processings_save(string saveDirName)
        {

        }
        private void performanceAnalysis()
        {

        }

        /// <summary>
        /// 雷达结果处理
        /// </summary>
        /// <param name="echo"></param>
        /// <param name="saveDirName"></param>
        
        //private void saveEchoDataAsFile(MWNumericArray echo, string saveDirName)
        //{
        //    Array echoReal = echo.ToArray(MWArrayComponent.Real);
        //    Array echoImag = echo.ToArray(MWArrayComponent.Imaginary);

        //    FileStream fs_initial = new FileStream(saveDirName + "_echoInitial.dat", FileMode.Append, FileAccess.Write);
        //    FileStream fs_guiyi = new FileStream(saveDirName + "_echoGuiyi.dat", FileMode.Append, FileAccess.Write);
        //    FileStream fs_initial_tx = new FileStream(saveDirName + "_echoInitial.txt", FileMode.Append, FileAccess.Write);
        //    BinaryWriter bw_initial = new BinaryWriter(fs_initial);
        //    BinaryWriter bw_guiyi = new BinaryWriter(fs_guiyi);
        //    StreamWriter tx_initial = new StreamWriter(fs_initial_tx);

        //    long row = 0;
        //    long column = 0;
        //    if (echoReal != null)
        //    {
        //        row = echoReal.GetLongLength(0);
        //        column = echoImag.GetLongLength(1);
        //        rowOfEchoData = row;
        //        columnOfEchoData = column;

        //        #region  save memory method
        //        // 找到绝对值最大的数
        //        double maxVal = 0;
        //        double currVal = 1;
        //        for (int i = 0; i < row; i++)
        //        {
        //            for (int j = 0; j < column; j++)
        //            {
        //                currVal = (double)echoImag.GetValue(i, j);
        //                if (Math.Abs(currVal) > maxVal)
        //                {
        //                    maxVal = Math.Abs(currVal);
        //                }
        //                tx_initial.Write(String.Format("{0,8:F2}", currVal));
        //            }
        //            tx_initial.Write("\r\n");

        //            for (int j = 0; j < column; j++)
        //            {
        //                currVal = (double)echoReal.GetValue(i, j);
        //                if (Math.Abs(currVal) > maxVal)
        //                {
        //                    maxVal = Math.Abs(currVal);
        //                }
        //                tx_initial.Write(String.Format("{0,8:F2}", currVal));
        //            }
        //            tx_initial.Write("\r\n");
        //        }
        //        tx_initial.Close();

        //        maxValOfEchoData = maxVal;
        //        long totalNum = row * column;
        //        long currNum = 0;
        //        long currRow = 0;
        //        long currColumn = 0;
        //        double val = 0.0;
        //        int segment = 0;
        //        sbyte guiyiVal = 1;

        //        while ((currNum + 4) <= totalNum)
        //        {
        //            for (int n = 4; n >= 1; n--)
        //            {
        //                currRow = (int)((currNum + n) / column);
        //                currColumn = (int)((currNum + n) % column);
        //                if ((currRow >= 1) && (currColumn == 0))  //每行最末尾一个数字
        //                {
        //                    currRow = currRow - 1;
        //                }
        //                if (currColumn == 0)  //因为行列下表索引从0开始，故要减去1
        //                {
        //                    currColumn = column - 1;
        //                }
        //                else if (currColumn > 0)
        //                {
        //                    currColumn = currColumn - 1;
        //                }
        //                val = (double)echoImag.GetValue(currRow, currColumn);
        //                guiyiVal = (sbyte)((val / maxVal) * 127);
        //                bw_guiyi.Write((sbyte)guiyiVal);
        //            }

        //            for (int n = 4; n >= 1; n--)
        //            {
        //                currRow = (int)((currNum + n) / column);
        //                currColumn = (int)((currNum + n) % column);
        //                if ((currRow >= 1) && (currColumn == 0))  // 每行最末尾一个数字
        //                {
        //                    currRow = currRow - 1;
        //                }
        //                if (currColumn == 0)  //因为行列下表索引从0开始，故要减去1
        //                {
        //                    currColumn = column - 1;
        //                }
        //                else if (currColumn > 0)
        //                {
        //                    currColumn = currColumn - 1;
        //                }
        //                val = (double)echoReal.GetValue(currRow, currColumn);
        //                guiyiVal = (sbyte)((val / maxVal) * 127);
        //                bw_guiyi.Write((sbyte)guiyiVal);
        //            }
        //            currNum += 4;
        //        }

        //        for (int i = 0; i < row; i++)
        //        {
        //            for (int j = 0; j < column; j++)
        //            {
        //                val = (double)echoImag.GetValue(i, j);
        //                bw_initial.Write((double)val);
        //            }

        //            for (int j = 0; j < column; j++)
        //            {
        //                val = (double)echoReal.GetValue(i, j);
        //                bw_initial.Write((double)val);
        //            }
        //        }
        //        #endregion

        //    }
        //    else
        //    {
        //        bw_initial.Write((double)-1);
        //        bw_guiyi.Write((double)-1);
        //    }
        //    bw_initial.Close();
        //    bw_guiyi.Close();

        //}
        //private void saveEchoDataAsTxt(MWNumericArray echo, string saveDirName)
        //{
        //    Array echoReal = echo.ToArray(MWArrayComponent.Real);
        //    Array echoImag = echo.ToArray(MWArrayComponent.Imaginary);

        //    FileStream fs_initial_tx = new FileStream(saveDirName + "_echoInitial_luo.txt", FileMode.Append, FileAccess.Write);
        //    StreamWriter tx_initial = new StreamWriter(fs_initial_tx);

        //    long row = 0;
        //    long column = 0;
        //    if (echoReal != null)
        //    {
        //        row = echoReal.GetLongLength(0);
        //        column = echoImag.GetLongLength(1);
        //        rowOfEchoData = row;
        //        columnOfEchoData = column;

        //        //写入行号和列号
        //        tx_initial.Write(String.Format("{0,8:F2}", row * 2));
        //        tx_initial.Write(String.Format("{0,8:F2}", column));
        //        tx_initial.Write("\r\n");

        //        // 找到绝对值最大的数
        //        double maxVal = 0;
        //        double currVal = 1;
        //        for (int i = 0; i < row; i++)
        //        {
        //            for (int j = 0; j < column; j++)
        //            {
        //                currVal = (double)echoImag.GetValue(i, j);
        //                if (Math.Abs(currVal) > maxVal)
        //                {
        //                    maxVal = Math.Abs(currVal);
        //                }
        //                tx_initial.Write(String.Format("{0,8:F2}", currVal));
        //            }
        //            tx_initial.Write("\r\n");

        //            for (int j = 0; j < column; j++)
        //            {
        //                currVal = (double)echoReal.GetValue(i, j);
        //                if (Math.Abs(currVal) > maxVal)
        //                {
        //                    maxVal = Math.Abs(currVal);
        //                }
        //                tx_initial.Write(String.Format("{0,8:F2}", currVal));
        //            }
        //            tx_initial.Write("\r\n");
        //        }
        //    }

        //    tx_initial.Close();
        //}
        //public void readEchoData(string echoDataDirName)
        //{

        //    double[,] realArray = new double[rowOfEchoData, columnOfEchoData];
        //    double[,] imageArray = new double[rowOfEchoData, columnOfEchoData];

        //    FileStream fs_guiyi = new FileStream(echoDataDirName, FileMode.Open, FileAccess.Read);
        //    BinaryReader bw_guiyi = new BinaryReader(fs_guiyi);

        //    FileStream fs_readGuiyiData = new FileStream(echoDataDirName + "_fs_readGuiyiData.dat", FileMode.Append, FileAccess.Write);
        //    FileStream tx_readGuiyiData = new FileStream(echoDataDirName + "_fs_readGuiyiData.txt", FileMode.Append, FileAccess.Write);
        //    BinaryWriter bw_readGuiyiData = new BinaryWriter(fs_readGuiyiData);
        //    StreamWriter tx_writeGuiyiData = new StreamWriter(tx_readGuiyiData);
        //    long row = rowOfEchoData;
        //    long column = columnOfEchoData;
        //    long totalNum = row * column;
        //    long currNum = 0;
        //    long currRow = 0;
        //    long currColumn = 0;
        //    double val = 0.0;
        //    int segment = 0;
        //    sbyte guiyiVal = 1;
        //    sbyte sval;

        //    while ((currNum + 4) <= totalNum)
        //    {
        //        for (int n = 4; n >= 1; n--)
        //        {
        //            currRow = (int)((currNum + n) / column);
        //            currColumn = (int)((currNum + n) % column);
        //            if ((currRow >= 1) && (currColumn == 0))  //每行最末尾一个数字
        //            {
        //                currRow = currRow - 1;
        //            }
        //            if (currColumn == 0)  //因为行列下表索引从0开始，故要减去1
        //            {
        //                currColumn = column - 1;
        //            }
        //            else if (currColumn > 0)
        //            {
        //                currColumn = currColumn - 1;
        //            }

        //            sval = bw_guiyi.ReadSByte();
        //            imageArray[currRow, currColumn] = sval * maxValOfEchoData / 127;

        //            bw_readGuiyiData.Write(sval);
        //        }

        //        for (int n = 4; n >= 1; n--)
        //        {
        //            currRow = (int)((currNum + n) / column);
        //            currColumn = (int)((currNum + n) % column);
        //            if ((currRow >= 1) && (currColumn == 0))  // 每行最末尾一个数字
        //            {
        //                currRow = currRow - 1;
        //            }
        //            if (currColumn == 0)  //因为行列下表索引从0开始，故要减去1
        //            {
        //                currColumn = column - 1;
        //            }
        //            else if (currColumn > 0)
        //            {
        //                currColumn = currColumn - 1;
        //            }

        //            sval = bw_guiyi.ReadSByte();
        //            if (sval != 0)
        //            {
        //                System.Threading.Thread.Sleep(0);
        //            }
        //            realArray[currRow, currColumn] = sval * maxValOfEchoData / 127;
        //            bw_readGuiyiData.Write(sval);

        //        }

        //        currNum += 4;
        //    }

        //    for (long i = 0; i < row; i++)
        //    {
        //        for (long j = 0; j < column; j++)
        //        {
        //            double dVal = imageArray[i, j];
        //            tx_writeGuiyiData.Write(String.Format("{0,8:F2}", imageArray[i, j]));
        //        }
        //        tx_writeGuiyiData.Write("\r\n");

        //        for (long j = 0; j < column; j++)
        //        {
        //            double dVal = realArray[i, j];
        //            tx_writeGuiyiData.Write(String.Format("{0,8:F2}", realArray[i, j]));
        //        }

        //        tx_writeGuiyiData.Write("\r\n");
        //    }

        //    fs_guiyi.Close();
        //    bw_guiyi.Close();
        //    fs_readGuiyiData.Close();
        //    bw_readGuiyiData.Close();
        //    tx_writeGuiyiData.Close();
        //    MWNumericArray echo;
        //    echo = new MWNumericArray(realArray, imageArray);
        //    echoDataOfFile = echo;
        //}
        //public void readEchoDataFromTxt(string echoDataDirName)
        //{

        //    double[,] realArray = new double[rowOfEchoData, columnOfEchoData];
        //    double[,] imageArray = new double[rowOfEchoData, columnOfEchoData];

        //    //打开TXT文件
        //    using (StreamReader sr = File.OpenText(echoDataDirName))
        //    {
        //        String input;
        //        UInt32 row = 0;
        //        UInt32 colomn = 0;
        //        String[] rowAndColumn = new String[2];

        //        char[] separator = new char[1];
        //        separator[0] = ' ';
        //        if ((input = sr.ReadLine()) != null)
        //        {
        //            //获取行号和列号
        //            //rowAndColumn = input.Split(separator);
        //            rowAndColumn = System.Text.RegularExpressions.Regex.Split(input.Trim(), @"\s+");
        //            //row = Convert.ToUInt32(rowAndColumn[0]);
        //            //colomn = Convert.ToUInt32(rowAndColumn[1]);

        //            row = UInt32.Parse(rowAndColumn[0], System.Globalization.NumberStyles.AllowDecimalPoint);
        //            colomn = UInt32.Parse(rowAndColumn[1], System.Globalization.NumberStyles.AllowDecimalPoint);
        //        }

        //        String[] qStrData = new String[colomn];
        //        String[] iStrData = new String[colomn];
        //        double[] qDecimalData = new double[colomn];
        //        double[] iDecimalData = new double[colomn];

        //        #region 解析采集回的数据
        //        //将文件数据读入二维缓冲区数组
        //        for (int j = 0; j < row / 2; j++)
        //        {
        //            if ((input = sr.ReadLine()) != null)
        //            {
        //                //qStrData = input.Split(separator);
        //                qStrData = System.Text.RegularExpressions.Regex.Split(input.Trim(), @"\s+");
        //                for (int i = 0; i < colomn; i++)
        //                {
        //                    //虚部
        //                    imageArray[j, i] = Convert.ToDouble(qStrData[i]);
        //                }
        //            }

        //            if ((input = sr.ReadLine()) != null)
        //            {
        //                //iStrData = input.Split(separator);
        //                iStrData = System.Text.RegularExpressions.Regex.Split(input.Trim(), @"\s+");
        //                for (int i = 0; i < colomn; i++)
        //                {
        //                    //实部
        //                    realArray[j, i] = Convert.ToDouble(iStrData[i]);
        //                }
        //            }
        //        }
        //        #endregion 成电生成原始数据处理
        //    }

        //    MWNumericArray echo;
        //    echo = new MWNumericArray(realArray, imageArray);
        //    echoDataOfFile = echo;
        //}
        //private void saveDataFruit(MWStructArray data, string saveDirName)
        //{
        //    //获取和设置包含该应用程序的目录的名称。(推荐)            
        //    //result: X:\xxx\xxx\ (.exe文件所在的目录+"\")
        //    //if (!Directory.Exists(saveDirName))
        //    //{
        //    //    // Create the directory it does not exist.
        //    //    Directory.CreateDirectory(saveDirName);
        //    //}
        //    MWNumericArray echo = (MWNumericArray)data.GetField("echo");
        //    Array echoReal = echo.ToArray(MWArrayComponent.Real);
        //    Array echoImag = echo.ToArray(MWArrayComponent.Imaginary);
        //    MWNumericArray img = (MWNumericArray)data.GetField("img");
        //    Array imgReal = img.ToArray(MWArrayComponent.Real);
        //    Array imgImag = img.ToArray(MWArrayComponent.Imaginary);
        //    MWNumericArray mtiimg = (MWNumericArray)data.GetField("mtiimg");
        //    Array mtiimgReal = mtiimg.ToArray(MWArrayComponent.Real);
        //    Array mtiimgImag = mtiimg.ToArray(MWArrayComponent.Imaginary);
        //    Array detected = data.GetField("detected").ToArray();
        //    Array aggmask = data.GetField("aggmask").ToArray();
        //    Array idx_r_v = data.GetField("idx_r_v").ToArray();

        //    #region save echo data
        //    //long row = 0;            
        //    //long column = 0;
        //    //FileStream fs = new FileStream(saveDirName + "_echo.dat", FileMode.Append, FileAccess.Write);                
        //    //BinaryWriter bw = new BinaryWriter(fs);
        //    //if (echoReal != null)
        //    //{
        //    //    bw.Write((double)CPINum);
        //    //    row = echoReal.GetLongLength(0);
        //    //    column = echoImag.GetLongLength(1);
        //    //    bw.Write((double)row);
        //    //    bw.Write((double)column);
        //    //    for (int i = 0; i < row; i++)
        //    //    {
        //    //        for (int j = 0; j < column; j++)
        //    //        {
        //    //            bw.Write((double)echoReal.GetValue(i, j));
        //    //        }
        //    //        for (int j = 0; j < column; j++)
        //    //        {
        //    //            bw.Write((double)echoImag.GetValue(i, j));
        //    //        }
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    bw.Write((double)-1);
        //    //}
        //    //fs.Close();
        //    //bw.Close();
        //    #endregion  save echo data

        //    long row = 0;
        //    long column = 0;
        //    FileStream fs_initial = new FileStream(saveDirName + "_echoInitial.dat", FileMode.Append, FileAccess.Write);
        //    FileStream fs_initial_guiyi = new FileStream(saveDirName + "_echoInitialGuiyi.dat", FileMode.Append, FileAccess.Write);
        //    FileStream fs_guiyi = new FileStream(saveDirName + "_echoGuiyi.dat", FileMode.Append, FileAccess.Write);
        //    BinaryWriter bw_initial = new BinaryWriter(fs_initial);
        //    BinaryWriter bw_initial_guiyi = new BinaryWriter(fs_initial_guiyi);
        //    BinaryWriter bw_guiyi = new BinaryWriter(fs_guiyi);
        //    if (echoReal != null)
        //    {
        //        //bw.Write((double)CPINum);
        //        row = echoReal.GetLongLength(0);
        //        column = echoImag.GetLongLength(1);

        //        //生成文件暂时不写行列号
        //        //bw.Write((double)row);
        //        //bw.Write((double)column);

        //        #region applicate memory space
        //        // 找到绝对值最大的数
        //        //double maxVal = 0;
        //        //double currVal = 1;
        //        //double[] realArray = new double[row * column];
        //        //double[] imageArray = new double[row * column];
        //        //long realIndex = 0;
        //        //long imageIndex = 0;
        //        //sbyte guiyiVal = 1;

        //        //for (int i = 0; i < row; i++)
        //        //{
        //        //    for (int j = 0; j < column; j++)
        //        //    {
        //        //        currVal = (double)echoImag.GetValue(i, j);
        //        //        imageArray[imageIndex++] = currVal;
        //        //        if (Math.Abs(currVal) > maxVal)
        //        //        {
        //        //            maxVal = Math.Abs(currVal);
        //        //        }
        //        //        bw_initial.Write(currVal);
        //        //    }
        //        //    for (int j = 0; j < column; j++)
        //        //    {
        //        //        currVal = (double)echoReal.GetValue(i, j);
        //        //        realArray[realIndex++] = currVal;
        //        //        if (Math.Abs(currVal) > maxVal)
        //        //        {
        //        //            maxVal = Math.Abs(currVal);
        //        //        }
        //        //        bw_initial.Write(currVal);
        //        //    }
        //        //}

        //        //for (int m = 0; m < row; m++)
        //        //{
        //        //    for (int n = 0; n < column; n++)
        //        //    {
        //        //        currVal = imageArray[m * column + n];
        //        //        guiyiVal = (sbyte)((currVal / maxVal) * 127);
        //        //        bw_initial_guiyi.Write((sbyte)guiyiVal);

        //        //        currVal = realArray[m * column + n];
        //        //        guiyiVal = (sbyte)((currVal / maxVal) * 127);
        //        //        bw_initial_guiyi.Write((sbyte)guiyiVal);
        //        //    }
        //        //}

        //        //long dataIndex = 0;
        //        //long totalNum = row * column;
        //        //while ((dataIndex + 4) <= totalNum)
        //        //{
        //        //    for (int n = 3; n >= 0; n--)  //image
        //        //    {
        //        //        currVal = imageArray[dataIndex + n];
        //        //        guiyiVal = (sbyte)((currVal / maxVal) * 127);
        //        //        bw_guiyi.Write((sbyte)guiyiVal);
        //        //    }

        //        //    for (int n = 3; n >= 0; n--)  //real
        //        //    {
        //        //        currVal = realArray[dataIndex + n];
        //        //        guiyiVal = (sbyte)((currVal / maxVal) * 127);
        //        //        bw_guiyi.Write((sbyte)guiyiVal);
        //        //    }
        //        //    dataIndex += 4;
        //        //}
        //        #endregion


        //        //#region  save memory method
        //        //// 找到绝对值最大的数
        //        //double maxVal = 0;
        //        //double currVal = 1;
        //        //for (int i = 0; i < row; i++)
        //        //{
        //        //    for (int j = 0; j < column; j++)
        //        //    {
        //        //        currVal = (double)echoReal.GetValue(i, j);
        //        //        if (Math.Abs(currVal) > maxVal)
        //        //        {
        //        //            maxVal = Math.Abs(currVal);
        //        //        }
        //        //    }
        //        //    for (int j = 0; j < column; j++)
        //        //    {
        //        //        currVal = (double)echoImag.GetValue(i, j);
        //        //        if (Math.Abs(currVal) > maxVal)
        //        //        {
        //        //            maxVal = Math.Abs(currVal);
        //        //        }
        //        //    }
        //        //}

        //        //long totalNum = row * column;
        //        //long currNum = 0;
        //        //long currRow = 0;
        //        //long currColumn = 0;
        //        //double val = 0.0;
        //        //int segment = 0;
        //        //sbyte guiyiVal = 1;


        //        ////bw_guiyi.Write(row);
        //        ////bw_guiyi.Write(column);
        //        //while ((currNum + 4) <= totalNum)
        //        //{
        //        //    for (int n = 4; n >= 1; n--)
        //        //    {
        //        //        currRow = (int)((currNum + n) / column);
        //        //        currColumn = (int)((currNum + n) % column);
        //        //        if ((currRow >= 1) && (currColumn == 0))  //每行最末尾一个数字
        //        //        {
        //        //            currRow = currRow - 1;
        //        //        }
        //        //        if (currColumn == 0)  //因为行列下表索引从0开始，故要减去1
        //        //        {
        //        //            currColumn = column - 1;
        //        //        }
        //        //        else if (currColumn > 0)
        //        //        {
        //        //            currColumn = currColumn - 1;
        //        //        }
        //        //        val = (double)echoImag.GetValue(currRow, currColumn);
        //        //        guiyiVal = (sbyte)((val / maxVal) * 127);
        //        //        bw_guiyi.Write((sbyte)guiyiVal);
        //        //    }

        //        //    for (int n = 4; n >= 1; n--)
        //        //    {
        //        //        currRow = (int)((currNum + n) / column);
        //        //        currColumn = (int)((currNum + n) % column);
        //        //        if ((currRow >= 1) && (currColumn == 0))  // 每行最末尾一个数字
        //        //        {
        //        //            currRow = currRow - 1;
        //        //        }
        //        //        if (currColumn == 0)  //因为行列下表索引从0开始，故要减去1
        //        //        {
        //        //            currColumn = column - 1;
        //        //        }
        //        //        else if (currColumn > 0)
        //        //        {
        //        //            currColumn = currColumn - 1;
        //        //        }
        //        //        val = (double)echoReal.GetValue(currRow, currColumn);
        //        //        guiyiVal = (sbyte)((val / maxVal) * 127);
        //        //        bw_guiyi.Write((sbyte)guiyiVal);
        //        //    }
        //        //    currNum += 4;
        //        //}

        //        //for (int i = 0; i < row; i++)
        //        //{
        //        //    for (int j = 0; j < column; j++)
        //        //    {
        //        //        val = (double)echoImag.GetValue(i, j);
        //        //        bw_initial.Write((double)val);
        //        //        guiyiVal = (sbyte)((val / maxVal) * 127);
        //        //        bw_initial_guiyi.Write((sbyte)guiyiVal);
        //        //    }

        //        //    for (int j = 0; j < column; j++)
        //        //    {
        //        //        val = (double)echoReal.GetValue(i, j);
        //        //        bw_initial.Write((double)val);
        //        //        guiyiVal = (sbyte)((val / maxVal) * 127);
        //        //        bw_initial_guiyi.Write((sbyte)guiyiVal);
        //        //    }
        //        //}
        //        //#endregion


        //        #region save data
        //        // 找到绝对值最大的数
        //        double maxVal = 0;
        //        double currVal = 1;
        //        for (int i = 0; i < row; i++)
        //        {
        //            for (int j = 0; j < column; j++)
        //            {
        //                currVal = (double)echoReal.GetValue(i, j);
        //                if (Math.Abs(currVal) > maxVal)
        //                {
        //                    maxVal = Math.Abs(currVal);
        //                }
        //            }
        //            for (int j = 0; j < column; j++)
        //            {
        //                currVal = (double)echoImag.GetValue(i, j);
        //                if (Math.Abs(currVal) > maxVal)
        //                {
        //                    maxVal = Math.Abs(currVal);
        //                }
        //            }
        //        }

        //        double val = 0.0;
        //        sbyte guiyiVal = 1;
        //        long loop = 0;
        //        for (int m = 0; m < column; m++)
        //        {
        //            loop = row / 4;
        //            for (long l = 0; l < loop; l++)
        //            {
        //                for (int n = 0; n < 4; n++)
        //                {
        //                    val = (double)echoImag.GetValue(n + l * 4, m);
        //                    bw_initial.Write((double)val);
        //                    guiyiVal = (sbyte)((val / maxVal) * 127);
        //                    bw_guiyi.Write((sbyte)guiyiVal);
        //                }
        //                for (int n = 0; n < 4; n++)
        //                {
        //                    val = (double)echoReal.GetValue(n + l * 4, m);
        //                    bw_initial.Write((double)val);
        //                    guiyiVal = (sbyte)((val / maxVal) * 127);
        //                    bw_guiyi.Write((sbyte)guiyiVal);
        //                }
        //            }
        //        }

        //        #endregion save data
        //    }
        //    else
        //    {
        //        bw_initial.Write((double)-1);
        //        bw_initial_guiyi.Write((double)-1);
        //        bw_guiyi.Write((double)-1);
        //    }
        //    bw_initial.Close();
        //    bw_initial_guiyi.Close();
        //    bw_guiyi.Close();



        //    FileStream fs = new FileStream(saveDirName + "_img.dat", FileMode.Append, FileAccess.Write);
        //    //StreamWriter sw = new StreamWriter(fs);
        //    BinaryWriter bw = new BinaryWriter(fs);
        //    if (imgReal != null)
        //    {
        //        bw.Write((double)CPINum);//
        //        row = imgReal.GetLongLength(0);
        //        column = imgReal.GetLongLength(1);
        //        bw.Write((double)row);
        //        bw.Write((double)column);
        //        for (int i = 0; i < row; i++)
        //        {
        //            for (int j = 0; j < column; j++)
        //            {
        //                bw.Write((double)imgReal.GetValue(i, j));
        //            }
        //            for (int j = 0; j < column; j++)
        //            {
        //                bw.Write((double)imgImag.GetValue(i, j));
        //            }
        //        }
        //    }
        //    else
        //        bw.Write((double)-1);
        //    bw.Close();
        //    fs.Close();

        //    fs = new FileStream(saveDirName + "_detected.dat", FileMode.Append, FileAccess.Write);
        //    //StreamWriter sw = new StreamWriter(fs);
        //    bw = new BinaryWriter(fs);
        //    if (detected != null)
        //    {
        //        bw.Write((double)CPINum);
        //        row = detected.GetLongLength(0);
        //        column = detected.GetLongLength(1);
        //        bw.Write((double)row);
        //        bw.Write((double)column);
        //        for (int i = 0; i < row; i++)
        //        {
        //            for (int j = 0; j < column; j++)
        //            {
        //                bw.Write((double)detected.GetValue(i, j));
        //            }
        //        }
        //    }
        //    else
        //        bw.Write(-1);
        //    bw.Close();
        //    fs.Close();

        //}

        /// <summary>
        /// 求质数
        /// </summary>
        /// <param name="prf"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public double[] Primes(double prf, double Num)
        {
            MWArray[] prfsets = mRadar.Primes(1, prf, Num);
            if (Num > 6)
                Num = 6;
            double[] prfset = new double[6];
            //Array ary = prfsets[0].ToArray();
            //MWArray aObj = prfsets[0];
            for (int i = 0; i < Num; i++)
            {
                prfset[i] = (double)prfsets[0].ToArray().GetValue(0, i);
            }
            return prfset;
        }

        #endregion
    }
}
