using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Timers;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using System.Reflection;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Drawing2D;//0114
using radarEchoSimulator.parameters;

namespace radarEchoSimulator
{
    public partial class mainForm : Form
    {
        //radarParam
        //private cRadarMode myRadarMode;
        //private cRadarParam myRadarParam;
        //private cTargetParam myTargetParam;
        //private cJamParam myJamParam;
        //private cOtherParam myOtherParam;
        //private cMainRadar myMainRadar;

        public mainForm()
        {
            InitializeComponent();
        }

        #region Event Handlers

        private void mainForm_Shown(object sender, EventArgs e)
        {
            this.TopMost = false;

        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            mainForm_myResize();
            button_ClearUp_Click(sender, e);
            this.labTextCtrl_param1.Hide();
            this.labTextCtrl_param2.Hide();
            this.labTextCtrl_param3.Hide();
            this.label9.Hide();
            this.comboBox_waveform.Hide();
            this.labTextCtrl_rParam1.Hide();
            this.labTextCtrl_rParam2.Hide();
            this.labTextCtrl_rParam3.Hide();
        }

        private void mainForm_Resize(object sender, EventArgs e)
        {
            mainForm_myResize();
        }

        /// <summary>
        /// 杂波类型选择及其参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>z
        private void comboBoxZ_SelectedValueChanged(object sender, EventArgs e)
        {

            switch (this.comboBox_typeZ.SelectedItem.ToString())
            {
                case "NULL":
                    myRadar.aProbDistr = Radar.eprobdistr.NULL;
                    this.labTextCtrl_param1.Hide();
                    this.labTextCtrl_param2.Hide();
                    this.labTextCtrl_param3.Hide();
                    break;
                case "Rayleigh":
                    myRadar.aProbDistr = Radar.eprobdistr.Rayleigh;
                    this.labTextCtrl_param1.Show();
                    this.labTextCtrl_param1.Text = "r:";
                    this.labTextCtrl_param3.Hide();
                    this.labTextCtrl_param2.Hide();
                    //this.labTextCtrl_param4.Hide(); 
                    break;
                case "Weibull":
                    myRadar.aProbDistr = Radar.eprobdistr.Weibull;
                    this.labTextCtrl_param1.Show();
                    this.labTextCtrl_param1.Text = "a:";
                    this.labTextCtrl_param3.Hide();
                    this.labTextCtrl_param2.Show();
                    this.labTextCtrl_param2.Text = "b:";
                    //this.labTextCtrl_param4.Hide(); 
                    break;
                case "Lognormal":
                    myRadar.aProbDistr = Radar.eprobdistr.lognormal;
                    this.labTextCtrl_param1.Show();
                    this.labTextCtrl_param1.Text = "mu:";
                    this.labTextCtrl_param3.Hide();
                    this.labTextCtrl_param2.Show();
                    this.labTextCtrl_param2.Text = "sigma:";
                    //this.labTextCtrl_param4.Hide(); 
                    break;
                case "K":
                    myRadar.aProbDistr = Radar.eprobdistr.K;
                    this.labTextCtrl_param1.Show();
                    this.labTextCtrl_param1.Text = "mu:";
                    this.labTextCtrl_param3.Show();
                    this.labTextCtrl_param3.Text = "L:";
                    this.labTextCtrl_param2.Show();
                    this.labTextCtrl_param2.Text = "v:";
                    //this.labTextCtrl_param4.Hide(); 
                    break;
                case "Sea":
                    myRadar.aProbDistr = Radar.eprobdistr.Sea;
                    this.labTextCtrl_param1.Hide();
                    this.labTextCtrl_param2.Hide();
                    this.labTextCtrl_param3.Hide();
                    break;
                default:
                    myRadar.aProbDistr = Radar.eprobdistr.NULL;
                    break;
            }
        }

        /// <summary>
        /// 干扰类型选择及其参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_Jam_SelectedValueChanged(object sender, EventArgs e)
        {
            switch (this.comboBox_typeJam.SelectedItem.ToString())
            {
                case "无":
                    myRadar.aJamParam.myJamType = Radar.JamParam.JamType.NULL;
                    myRadar.aJamParam.onoff = 0;
                    break;
                case "压制干扰":
                    myRadar.aJamParam.myJamType = Radar.JamParam.JamType.BlanketJamming;
                    myRadar.aJamParam.onoff = 1;
                    this.labTextCtrl_jsr.TipTextBox = myRadar.aJamParam.jsr.ToString();
                    break;
                case "距离欺骗干扰":
                    myRadar.aJamParam.myJamType = Radar.JamParam.JamType.RangeJamming;
                    myRadar.aJamParam.onoff = 1;
                    break;
                case "欺骗干扰":
                    myRadar.aJamParam.myJamType = Radar.JamParam.JamType.VelocityJamming;
                    myRadar.aJamParam.onoff = 1;

                    labTextCtrl_JamStart.TipTextBox = (myRadar.aJamParam.rangeStart / 1000).ToString();//干扰源起始距离
                    labTextCtrl_JamEnd.TipTextBox = (myRadar.aJamParam.rangeEnd / 1000).ToString();//干扰源起始距离

                    this.labTextCtrl_JamPa1.TipTextBox = (myRadar.aJamParam.vrStart).ToString();
                    this.labTextCtrl_JamPa2.TipTextBox = (myRadar.aJamParam.vrEnd).ToString();
                    break;
                default:
                    myRadar.aJamParam.myJamType = Radar.JamParam.JamType.NULL;
                    myRadar.aJamParam.onoff = 0;
                    break;
            }
        }

        /// <summary>
        /// 天气类型选择及其参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxS_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBox CBOX = (ComboBox)sender as ComboBox;
            switch (CBOX.SelectedItem.ToString())
            {
                case "无":
                    myRadar.sphereEffect = 0;
                    break;
                case "云":
                    myRadar.sphereEffect = 1;
                    break;
                case "雾":
                    myRadar.sphereEffect = 2;
                    break;
                case "雨":
                    myRadar.sphereEffect = 3;
                    break;
                case "雪":
                    myRadar.sphereEffect = 4;
                    break;
                default:
                    myRadar.sphereEffect = 0;
                    break;
            }
        }

        /// <summary>
        /// 波形选择：    二相编码信号 (BPSK )；线性调频信号 (LFM)；正弦调频信号 (SFM)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_pulseCmplsForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ComboBox cbox = (ComboBox)sender as ComboBox;
            switch (this.comboBox_waveform.SelectedItem.ToString())
            {
                case "无":
                    myRadar.aPulseCmprsType = Radar.ePulseCmprsType.NULL;
                    this.labTextCtrl_rParam1.Hide();
                    this.labTextCtrl_rParam2.Hide();
                    this.labTextCtrl_rParam3.Hide();
                    break;
                case "二相编码信号 (BPSK )":
                    myRadar.aPulseCmprsType = Radar.ePulseCmprsType.BPSK;
                    this.labTextCtrl_rParam1.Text = "    二进制个数:";
                    this.labTextCtrl_rParam1.TipTextBox = myRadar.numByte.ToString();
                    this.labTextCtrl_rParam1.Show();
                    this.labTextCtrl_rParam2.Hide();
                    this.labTextCtrl_rParam3.Hide();
                    break;
                case "线性调频信号 (LFM)":
                    myRadar.aPulseCmprsType = Radar.ePulseCmprsType.LFM;
                    this.labTextCtrl_rParam1.Hide();
                    this.labTextCtrl_rParam2.Hide();
                    this.labTextCtrl_rParam3.Hide();
                    break;
                case "正弦调频信号 (SFM)":
                    myRadar.aPulseCmprsType = Radar.ePulseCmprsType.SFM;
                    this.labTextCtrl_rParam1.Text = "正弦调制指数:";
                    this.labTextCtrl_rParam1.TipTextBox = myRadar.m_f.ToString();
                    this.labTextCtrl_rParam1.Show();
                    this.labTextCtrl_rParam2.Text = "正弦调制频率:";
                    this.labTextCtrl_rParam2.TipTextBox = myRadar.f_m.ToString();
                    this.labTextCtrl_rParam2.Show();
                    this.labTextCtrl_rParam3.Hide();
                    break;
                default:
                    myRadar.aPulseCmprsType = Radar.ePulseCmprsType.NULL;
                    break;
            }
        }

        /// <summary>
        /// 雷达类型选择及其参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_Radar_ItemCheck(object sender, EventArgs e)
        {
            switch (comboBox_Radar.SelectedIndex)
            {
                case 0:// "脉冲多普勒雷达":
                    myRadar.operateMode = Radar.eOperateMode.pulsedDopplerRadar;
                    this.label9.Hide();
                    this.comboBox_waveform.Hide();
                    this.labTextCtrl_rParam1.Hide();
                    this.labTextCtrl_rParam2.Hide();
                    this.labTextCtrl_rParam3.Hide();
                    break;
                case 1:// "脉冲压缩雷达":
                    myRadar.operateMode = Radar.eOperateMode.pulseCompressionRadar;
                    this.label9.Show();
                    this.comboBox_waveform.Show();
                    this.comboBox_pulseCmplsForm_SelectedIndexChanged(null, null);
                    //this.labTextCtrl_rParam1.Hide();
                    //this.labTextCtrl_rParam2.Hide();
                    //this.labTextCtrl_rParam3.Hide();
                    break;
                case 2:// "频率捷变雷达":
                    myRadar.operateMode = Radar.eOperateMode.frequencyAgileRadar;
                    this.label9.Hide();
                    this.comboBox_waveform.Hide();
                    this.labTextCtrl_rParam1.Text = "捷变频个数:";
                    this.labTextCtrl_rParam1.Show();
                    this.labTextCtrl_rParam1.TipTextBox = myRadar.NumFreq.ToString();
                    this.labTextCtrl_rParam2.Text = "跳变间隔(MHz):";
                    this.labTextCtrl_rParam2.Show();
                    this.labTextCtrl_rParam2.TipTextBox = (myRadar.scanOffset / 1000000).ToString();//"10000000";
                    this.labTextCtrl_rParam3.Text = "捷变频脉冲数";
                    this.labTextCtrl_rParam3.Show();
                    this.labTextCtrl_rParam3.TipTextBox = myRadar.NumPlsGrounp.ToString();//"32";
                    break;
                case 3:// "连续波雷达":
                    myRadar.operateMode = Radar.eOperateMode.FMCW;
                    this.label9.Hide();
                    this.comboBox_waveform.Hide();
                    this.labTextCtrl_rParam1.Hide();
                    this.labTextCtrl_rParam2.Hide();
                    this.labTextCtrl_rParam3.Hide();
                    break;
                default:
                    MessageBox.Show("请选择雷达模式！");
                    break;
            }
            //this.groupBox5.Show();
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_run_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectTab(tabPage1);
            setParams();

            //Save results
            if (checkBox_SvDat.CheckState == CheckState.Checked)
            {
                if (saveDirName == null)
                {
                    MessageBox.Show("请选择存储路径！");
                    return;
                }
            }
            else if (checkBox_SvDat.CheckState == CheckState.Unchecked)
            {
            }
            else
            {
                MessageBox.Show("checkBox1 控件处于不确定状态");
            }

            //
            if (myRadar.operateMode != null)
            {
                if (myRadar.operateMode == Radar.eOperateMode.pulseCompressionRadar
                    && myRadar.aPulseCmprsType == Radar.ePulseCmprsType.NULL)
                {
                    MessageBox.Show("请选择脉冲压缩雷达波形！");
                    this.comboBox_waveform.Focus();
                }
                else
                {
                    this.label_RunStatus.Text = "运行状态：正在运行";

                    if (this.backgroundWorker1.IsBusy)
                        return;

                    myRadar.selfCycle = false;
                    this.backgroundWorker1.RunWorkerAsync();//启动多线程
                    if (checkBox_SvDat.CheckState == CheckState.Checked)
                    {
                        if (this.backgroundWorker2.IsBusy)
                            return;
                        while (true)
                        {
                            if (File.Exists(saveDirName + "_echo.dat") || File.Exists(saveDirName + "_img.dat") || File.Exists(saveDirName + "_detected.dat"))
                            {
                                if (MessageBox.Show(saveDirName + "_echo.dat" + "文件已存在，是否覆盖?", "确认信息", MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                                {
                                    File.Delete(saveDirName + "_echo.dat");
                                    File.Delete(saveDirName + "_img.dat");
                                    File.Delete(saveDirName + "_detected.dat");
                                    break;
                                }
                                else
                                {
                                    this.saveFileDialog1.ShowDialog();
                                    saveDirName = saveFileDialog1.FileName;
                                    this.textBox1.Text = saveDirName;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        this.backgroundWorker2.RunWorkerAsync();
                    }
                }

            }

        }

        /// <summary>
        /// performacne Analysis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_perfAna_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectTab(tabPage2);
            setParams();
            if (myRadar.operateMode != null)
            {
                if (myRadar.operateMode == Radar.eOperateMode.pulseCompressionRadar
                    && myRadar.aPulseCmprsType == Radar.ePulseCmprsType.NULL)
                {
                    MessageBox.Show("请选择脉冲压缩雷达波形！");
                    this.comboBox_waveform.Focus();
                }
                else
                {
                    if (this.backgroundWorker3.IsBusy)
                        return;
                    this.label_perfAna.Text = "性能分析：正在运行";
                    if (checkedListBox_X.GetItemChecked(0))
                        myRadar.Case[0] = true;
                    else
                        myRadar.Case[0] = false;
                    if (checkedListBox_X.GetItemChecked(1))
                        myRadar.Case[1] = true;
                    else
                        myRadar.Case[1] = false;
                    //if (checkedListBox1.GetItemChecked(2))
                    //    myRadar.Case[2] = true;
                    //else
                    //    myRadar.Case[2] = false;

                    this.backgroundWorker3.RunWorkerAsync();//启动多线程
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.backgroundWorker3.IsBusy)
                return;
            this.label_perfAna.Text = "性能分析：未运行";
            this.chart1.Series.Clear();
            this.chart2.Series.Clear();
            this.chart3.Series.Clear();
            this.chart4.Series.Clear();
            this.chart5.Series.Clear();
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            if (myRadar.Case[0])
            {
                myRadar.xValue = null; myRadar.yValue1 = null; myRadar.yValue2 = null; myRadar.yValue3 = null;
                myRadar.performanceAnalysis(myRadar.operateMode, "1");
                this.backgroundWorker3.ReportProgress(33, (int)1);
                Thread.Sleep(1000);
            }
            if (myRadar.Case[1])
            {
                myRadar.xValue = null; myRadar.yValue1 = null; myRadar.yValue2 = null; myRadar.yValue3 = null;
                myRadar.performanceAnalysis(myRadar.operateMode, "2");
                this.backgroundWorker3.ReportProgress(66, (int)2);
                Thread.Sleep(1000);
            }

        }
        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.label_perfAna.Text = "性能分析：结束运行";
        }
        private void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int state = (int)e.UserState;
            if (state == 1)
            {
                chart_ctrl(chart1, myRadar.xValue, myRadar.yValue1, "距离估计误差(m)", "信噪比(dB)");
                chart_ctrl(chart2, myRadar.xValue, myRadar.yValue2, "速度估计误差(m/s)", "信噪比(dB)");
                chart_ctrl(chart3, myRadar.xValue, myRadar.yValue3, "角度估计误差(rad)", "信噪比(dB)");
            }
            else if (state == 2)
            {
                chart_ctrl(chart4, myRadar.xValue, myRadar.yValue1, "检测概率", "信噪比(dB)");
                chart_ctrl(chart5, myRadar.xValue, myRadar.yValue2, "虚警概率", "信噪比(dB)");
            }

        }

        /// <summary>
        /// 线程1：运行M代码，生产仿真结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            myRadar.RadarProcessingsOnce(myRadar.operateMode);
        }

        /// <summary>
        /// 线程结束：完成结果绘制，保存，启动线程2从而保存剩下CPI数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (myRadar.echoImag == null)
                return;
            Image image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(image);
            Bitmap[] bitmaps =
                    {
                        ImgConvert(myRadar.echoReal, false),
                       ImgConvert(myRadar.imgReal, true),
                        //ImgConvert(myRadar.mtiimgReal),
                        ImgConvert(myRadar.detected, true)
                    };
            //DrawPic(ref g, 20, 20, pictureBox1.Width - 20, pictureBox1.Height/3, ref tmp2);
            string[][] titles = new string[][]
                    {
                        new string [] { "原始图像", "脉冲序号", "距离单元" },
                        new string [] { "距离—多普勒图像", "多普勒", "距离(m)" },
                        //new string [] { "MTI之后图像", "多普勒", "距离(m)" },
                        new string [] { "检测结果", "多普勒", "距离(m)" }
                    };
            this.idx_r_v_a_setvalue(myRadar.idx_r_v, myRadar.idx_r_v_a);
            int div = 3;
            for (int i = 0; i < div; i++)
            {
                this.DrawPic(ref g, 30, (image.Height / div) * i + 25, image.Width - 30,
                    image.Height / div - 50, ref bitmaps[i], ref titles[i]);
                this.DrawTag(ref g, 30, (image.Height * 2 / 3) + 25, image.Width - 30, image.Height / div - 50,
                    myRadar.idx_r_v_a, myRadar.idx_r_v, ref bitmaps[2]);
                //0114 draw tabpage4
                this.draw_tabPage4(D, myRadar.idx_r_v_a, this.tabPage4.Width, this.tabPage4.Height);
            }
            //g.DrawImage(tmp2, 10, 0, pictureBox1.Width-10, pictureBox1.Height-10);
            //DrawXY(ref g, pictureBox1);

            //this.pictureBox1.Dock = DockStyle.Fill;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox1.Image = image;
            for (int i = 0; i < div; i++)
            {
                if (bitmaps[i] != null)
                    bitmaps[i].Dispose();
            }

            charts_FFT_T();

            if (backgroundWorker2.IsBusy)
                return;
            this.label_RunStatus.Text = "运行状态：运行结束";
        }

        /// <summary>
        /// 线程2：运行M代码，产生并保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            myRadar.ProcessingsSave(myRadar.operateMode, saveDirName);
        }
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (backgroundWorker1.IsBusy)
                return;
            this.label_RunStatus.Text = "运行状态：运行结束";
        }

        /// <summary>
        /// 显示选择保存路径窗口按钮响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //this.saveFileDialog1.Filter = "dat Files (*.dat)|*.dat|All Files(*.*)|*.*";
            //this.saveFileDialog1.DefaultExt = "untitled.dat";
            this.saveFileDialog1.FileName = "输入文件名称";
            this.saveFileDialog1.ShowDialog();
        }

        /// <summary>
        /// 保存文件弹框点击OK按钮：文件路径保存到saveDirName
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                // Get file name.
                saveDirName = saveFileDialog1.FileName;
                // Write to the file name selected.
                // ... You can write the text from a TextBox instead of a string literal.
                this.textBox1.Text = saveDirName;
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

        }

        /// <summary>
        /// 清屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_ClearUp_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                Image image = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
                Pen pen = new Pen(Color.Black, 4);
                SolidBrush sb = new SolidBrush(Color.Red);//画刷
                StringFormat sf = new StringFormat(StringFormatFlags.DirectionVertical);//字符串格式，竖着写
                Font ft = new System.Drawing.Font("宋体", 30);

                Graphics g = Graphics.FromImage(image);
                g.Clear(SystemColors.Window);
                Image bkg = global::radarEchoSimulator.Properties.Resources.bkgBig;//Image.FromFile(@"bkg3.jpg");
                this.pictureBox1.Image = bkg;
                this.pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
                this.label_RunStatus.Text = "运行状态：未运行";
            }
            if (!backgroundWorker3.IsBusy)
                this.label_perfAna.Text = "性能分析：未运行";
        }

        /// <summary>
        /// 重频选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_CP_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox CBox = (ComboBox)sender as ComboBox;
            switch (CBox.SelectedIndex)
            {
                case 0://固定
                    myRadar.aRFType = Radar.eRepeatedFrequancyType.typeConstant;
                    this.checkBox_SvDat.CheckState = CheckState.Unchecked;
                    this.radioButton1.Checked = true;
                    this.textBox_prf1.Text = this.labTextCtrl_prf.BoxText;
                    this.groupBox9.Enabled = false;
                    this.groupBox8.Enabled = false;

                    //this.textBox_prf1.Text = myRadar.prfSet[0].ToString();
                    this.textBox_prf2.Text = "";//myRadar.prfSet[1].ToString();
                    this.textBox_prf3.Text = "";// myRadar.prfSet[2].ToString();
                    this.textBox_prf4.Text = "";//myRadar.prfSet[3].ToString();
                    this.textBox_prf5.Text = "";//myRadar.prfSet[4].ToString();
                    this.textBox_prf6.Text = "";//myRadar.prfSet[5].ToString();

                    break;
                case 1://参差
                    myRadar.aRFType = Radar.eRepeatedFrequancyType.typeIrregular;
                    this.checkBox_SvDat.CheckState = CheckState.Unchecked;
                    this.groupBox9.Enabled = true;
                    this.groupBox8.Enabled = true;
                    this.radioButton1.Enabled = false;
                    this.radioButton2.Checked = true;
                    radioButton2_Click(sender, e);
                    break;
                case 2://抖动
                    myRadar.aRFType = Radar.eRepeatedFrequancyType.typeTremble;
                    this.checkBox_SvDat.CheckState = CheckState.Unchecked;
                    this.groupBox9.Enabled = true;
                    this.groupBox8.Enabled = true;
                    this.radioButton1.Enabled = false;
                    this.radioButton2.Checked = true;
                    radioButton2_Click(sender, e);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 选择prf个数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_Click(object sender, EventArgs e)
        {
            textBox_prf1.Enabled = false;
            textBox_prf2.Enabled = false;
            textBox_prf3.Enabled = false;
            textBox_prf4.Enabled = false;
            textBox_prf5.Enabled = false;
            textBox_prf6.Enabled = false;
            switch (myRadar.aRFType)
            {
                case Radar.eRepeatedFrequancyType.typeConstant:
                    break;
                case Radar.eRepeatedFrequancyType.typeIrregular:
                    break;
                case Radar.eRepeatedFrequancyType.typeTremble:
                    break;
                default:
                    break;
            };

        }
        private void radioButton2_Click(object sender, EventArgs e)
        {
            myRadar.RangeStartCltr = Convert.ToDouble(labTextCtrl_startCltr.BoxText);//仿真起始距离
            myRadar.RangeEndCltr = Convert.ToDouble(labTextCtrl_endCltr.BoxText);//仿真结束距离
            myRadar.RangeStartMov = Convert.ToDouble(labTextCtrl_startMov.BoxText);//最近目标距离
            myRadar.RangeEndMov = Convert.ToDouble(labTextCtrl_endMov.BoxText);//最远目标距离
            myRadar.prfUserSet = Convert.ToDouble(this.labTextCtrl_prf.BoxText);//prf
            myRadar.prfNum = 2;
            textBox_prf1.Enabled = true;
            textBox_prf2.Enabled = true;
            textBox_prf3.Enabled = false;
            textBox_prf4.Enabled = false;
            textBox_prf5.Enabled = false;
            textBox_prf6.Enabled = false;
            switch (myRadar.aRFType)
            {
                case Radar.eRepeatedFrequancyType.typeConstant:
                    break;
                case Radar.eRepeatedFrequancyType.typeTremble:
                    Random rand = new Random();
                    for (int i = 0; i < myRadar.prfNum; i++)
                    {
                        myRadar.prfSet[i] = rand.Next((int)Math.Min((myRadar.prfUserSet * 0.9), (C / 2 / myRadar.rgMax() * 0.8)), (int)Math.Min((myRadar.prfUserSet * 1.1), C / 2 / myRadar.rgMax()));
                    }
                    break;
                case Radar.eRepeatedFrequancyType.typeIrregular:
                    double[] gPrfset = myRadar.Primes(myRadar.prfUserSet, myRadar.prfNum);
                    for (int i = 0; i < myRadar.prfNum; i++)
                        myRadar.prfSet[i] = gPrfset[i];
                    break;
                default:
                    break;
            }
            this.textBox_prf1.Text = myRadar.prfSet[0].ToString();
            this.textBox_prf2.Text = myRadar.prfSet[1].ToString();
            this.textBox_prf3.Text = "";// myRadar.prfSet[2].ToString();
            this.textBox_prf4.Text = "";//myRadar.prfSet[3].ToString();
            this.textBox_prf5.Text = "";//myRadar.prfSet[4].ToString();
            this.textBox_prf6.Text = "";//myRadar.prfSet[5].ToString();
        }
        private void radioButton3_Click(object sender, EventArgs e)
        {
            myRadar.RangeStartCltr = Convert.ToDouble(labTextCtrl_startCltr.BoxText);//仿真起始距离
            myRadar.RangeEndCltr = Convert.ToDouble(labTextCtrl_endCltr.BoxText);//仿真结束距离
            myRadar.RangeStartMov = Convert.ToDouble(labTextCtrl_startMov.BoxText);//最近目标距离
            myRadar.RangeEndMov = Convert.ToDouble(labTextCtrl_endMov.BoxText);//最远目标距离
            myRadar.prfUserSet = Convert.ToDouble(this.labTextCtrl_prf.BoxText);//最远目标距离
            myRadar.prfNum = 3;
            textBox_prf1.Enabled = true;
            textBox_prf2.Enabled = true;
            textBox_prf3.Enabled = true;
            textBox_prf4.Enabled = false;
            textBox_prf5.Enabled = false;
            textBox_prf6.Enabled = false;
            switch (myRadar.aRFType)
            {
                case Radar.eRepeatedFrequancyType.typeConstant:
                    break;
                case Radar.eRepeatedFrequancyType.typeTremble:
                    Random rand = new Random();
                    for (int i = 0; i < myRadar.prfNum; i++)
                    {
                        myRadar.prfSet[i] = rand.Next((int)Math.Min((myRadar.prfUserSet * 0.9), (C / 2 / myRadar.rgMax() * 0.8)), (int)Math.Min((myRadar.prfUserSet * 1.1), C / 2 / myRadar.rgMax()));
                    }
                    break;
                case Radar.eRepeatedFrequancyType.typeIrregular:
                    double[] gPrfset = myRadar.Primes(myRadar.prfUserSet, myRadar.prfNum);
                    for (int i = 0; i < myRadar.prfNum; i++)
                        myRadar.prfSet[i] = gPrfset[i];
                    break;
                default:
                    break;
            }
            this.textBox_prf1.Text = myRadar.prfSet[0].ToString();
            this.textBox_prf2.Text = myRadar.prfSet[1].ToString();
            this.textBox_prf3.Text = myRadar.prfSet[2].ToString();
            this.textBox_prf4.Text = "";//myRadar.prfSet[3].ToString();
            this.textBox_prf5.Text = "";//myRadar.prfSet[4].ToString();
            this.textBox_prf6.Text = "";//myRadar.prfSet[5].ToString();
        }
        private void radioButton4_Click(object sender, EventArgs e)
        {
            myRadar.RangeStartCltr = Convert.ToDouble(labTextCtrl_startCltr.BoxText);//仿真起始距离
            myRadar.RangeEndCltr = Convert.ToDouble(labTextCtrl_endCltr.BoxText);//仿真结束距离
            myRadar.RangeStartMov = Convert.ToDouble(labTextCtrl_startMov.BoxText);//最近目标距离
            myRadar.RangeEndMov = Convert.ToDouble(labTextCtrl_endMov.BoxText);//最远目标距离
            myRadar.prfUserSet = Convert.ToDouble(this.labTextCtrl_prf.BoxText);//最远目标距离
            myRadar.prfNum = 4;
            textBox_prf1.Enabled = true;
            textBox_prf2.Enabled = true;
            textBox_prf3.Enabled = true;
            textBox_prf4.Enabled = true;
            textBox_prf5.Enabled = false;
            textBox_prf6.Enabled = false;
            switch (myRadar.aRFType)
            {
                case Radar.eRepeatedFrequancyType.typeConstant:
                    break;
                case Radar.eRepeatedFrequancyType.typeTremble:
                    Random rand = new Random();
                    for (int i = 0; i < myRadar.prfNum; i++)
                        myRadar.prfSet[i] = rand.Next((int)Math.Min((myRadar.prfUserSet * 0.9), (C / 2 / myRadar.rgMax() * 0.8)), (int)Math.Min((myRadar.prfUserSet * 1.1), C / 2 / myRadar.rgMax()));
                    break;
                case Radar.eRepeatedFrequancyType.typeIrregular:
                    double[] gPrfset = myRadar.Primes(myRadar.prfUserSet, myRadar.prfNum);
                    for (int i = 0; i < myRadar.prfNum; i++)
                        myRadar.prfSet[i] = gPrfset[i];
                    break;
                default:
                    break;
            }
            this.textBox_prf1.Text = myRadar.prfSet[0].ToString();
            this.textBox_prf2.Text = myRadar.prfSet[1].ToString();
            this.textBox_prf3.Text = myRadar.prfSet[2].ToString();
            this.textBox_prf4.Text = myRadar.prfSet[3].ToString();
            this.textBox_prf5.Text = "";//myRadar.prfSet[4].ToString();
            this.textBox_prf6.Text = "";//myRadar.prfSet[5].ToString();
        }
        private void radioButton5_Click(object sender, EventArgs e)
        {
            myRadar.RangeStartCltr = Convert.ToDouble(labTextCtrl_startCltr.BoxText);//仿真起始距离
            myRadar.RangeEndCltr = Convert.ToDouble(labTextCtrl_endCltr.BoxText);//仿真结束距离
            myRadar.RangeStartMov = Convert.ToDouble(labTextCtrl_startMov.BoxText);//最近目标距离
            myRadar.RangeEndMov = Convert.ToDouble(labTextCtrl_endMov.BoxText);//最远目标距离
            myRadar.prfUserSet = Convert.ToDouble(this.labTextCtrl_prf.BoxText);//最远目标距离
            myRadar.prfNum = 5;
            textBox_prf1.Enabled = true;
            textBox_prf2.Enabled = true;
            textBox_prf3.Enabled = true;
            textBox_prf4.Enabled = true;
            textBox_prf5.Enabled = true;
            textBox_prf6.Enabled = false;
            switch (myRadar.aRFType)
            {
                case Radar.eRepeatedFrequancyType.typeConstant:
                    break;
                case Radar.eRepeatedFrequancyType.typeTremble:
                    Random rand = new Random();
                    for (int i = 0; i < myRadar.prfNum; i++)
                        myRadar.prfSet[i] = rand.Next((int)Math.Min((myRadar.prfUserSet * 0.9), (C / 2 / myRadar.rgMax() * 0.8)), (int)Math.Min((myRadar.prfUserSet * 1.1), C / 2 / myRadar.rgMax()));
                    break;
                case Radar.eRepeatedFrequancyType.typeIrregular:
                    double[] gPrfset = myRadar.Primes(myRadar.prfUserSet, myRadar.prfNum);
                    for (int i = 0; i < myRadar.prfNum; i++)
                        myRadar.prfSet[i] = gPrfset[i];
                    break;
                default:
                    break;
            }
            this.textBox_prf1.Text = myRadar.prfSet[0].ToString();
            this.textBox_prf2.Text = myRadar.prfSet[1].ToString();
            this.textBox_prf3.Text = myRadar.prfSet[2].ToString();
            this.textBox_prf4.Text = myRadar.prfSet[3].ToString();
            this.textBox_prf5.Text = myRadar.prfSet[4].ToString();
            this.textBox_prf6.Text = "";//myRadar.prfSet[5].ToString();
        }
        private void radioButton6_Click(object sender, EventArgs e)
        {
            myRadar.RangeStartCltr = Convert.ToDouble(labTextCtrl_startCltr.BoxText);//仿真起始距离
            myRadar.RangeEndCltr = Convert.ToDouble(labTextCtrl_endCltr.BoxText);//仿真结束距离
            myRadar.RangeStartMov = Convert.ToDouble(labTextCtrl_startMov.BoxText);//最近目标距离
            myRadar.RangeEndMov = Convert.ToDouble(labTextCtrl_endMov.BoxText);//最远目标距离
            myRadar.prfUserSet = Convert.ToDouble(this.labTextCtrl_prf.BoxText);//最远目标距离
            myRadar.prfNum = 6;
            textBox_prf1.Enabled = true;
            textBox_prf2.Enabled = true;
            textBox_prf3.Enabled = true;
            textBox_prf4.Enabled = true;
            textBox_prf5.Enabled = true;
            textBox_prf6.Enabled = true;
            switch (myRadar.aRFType)
            {
                case Radar.eRepeatedFrequancyType.typeConstant:
                    break;
                case Radar.eRepeatedFrequancyType.typeTremble:
                    Random rand = new Random();
                    for (int i = 0; i < myRadar.prfNum; i++)
                        myRadar.prfSet[i] = rand.Next((int)Math.Min((myRadar.prfUserSet * 0.9), (C / 2 / myRadar.rgMax() * 0.8)), (int)Math.Min((myRadar.prfUserSet * 1.1), C / 2 / myRadar.rgMax()));
                    break;
                case Radar.eRepeatedFrequancyType.typeIrregular:
                    double[] gPrfset = myRadar.Primes(myRadar.prfUserSet, myRadar.prfNum);
                    for (int i = 0; i < myRadar.prfNum; i++)
                        myRadar.prfSet[i] = gPrfset[i];
                    break;
                default:
                    break;
            }
            this.textBox_prf1.Text = myRadar.prfSet[0].ToString();
            this.textBox_prf2.Text = myRadar.prfSet[1].ToString();
            this.textBox_prf3.Text = myRadar.prfSet[2].ToString();
            this.textBox_prf4.Text = myRadar.prfSet[3].ToString();
            this.textBox_prf5.Text = myRadar.prfSet[4].ToString();
            this.textBox_prf6.Text = myRadar.prfSet[5].ToString();
        }

        /// <summary>
        /// 是否保存数据按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cBox = (CheckBox)sender as CheckBox;
            if (cBox.CheckState == CheckState.Checked)
            {
                this.textBox1.Enabled = true;
                this.button1.Enabled = true;
                return;
            }
            else
            {
                this.textBox1.Enabled = false;
                this.button1.Enabled = false;
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (myRadar.operateMode != null)
            {
                setParams();
                this.saveFileDialog2.Filter = "dat Files (*.dat)|*.dat|All Files(*.*)|*.*";
                this.saveFileDialog2.DefaultExt = "untitled.dat";

                this.timeToolStripMenuItem.Text = System.DateTime.Now.ToString();

                this.saveFileDialog2.FileName = myRadar.operateMode.ToString() + "Param_" + System.DateTime.Now.ToString("yyyyMMdd_HHmm");
                if (this.saveFileDialog2.ShowDialog() == DialogResult.OK)
                    myRadar.saveParams(saveFileDialog2.FileName);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "dat Files (*.dat)|*.dat|All Files(*.*)|*.*";
            this.openFileDialog1.FileName = "选择文件";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                myRadar.loadParams(openFileDialog1.FileName);
                this.ParamTips();
            }
        }

        private void 目标信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.setParams();
            inforForm inforForm1 = new inforForm();
            inforForm1.Text = "目标信息";
            string text = null;
            double[] tmp = myRadar.rg_mov();
            text = "目标距离(m): ";
            foreach (double var in tmp)
            {

                text += String.Format("{0,10:f3}", var);//.ToString("#######.##") + "  ";
            }
            text += "\r\n";
            text += "目标速度(m/s): ";
            foreach (double var in myRadar.vrSet)
            {
                text += String.Format("{0,10:f3}", var);
            }
            text += "\r\n";
            text += "\r\n";

            //double lambda = 300.0 / Convert.ToInt32(labTextCtrl_fc.BoxText);
            Array ary;
            ary = Array.CreateInstance(typeof(double), myRadar.idx_r_v_a.GetLength(0), myRadar.idx_r_v_a.GetLength(1));
            sort(ary, myRadar.idx_r_v_a, myRadar.idx_r_v.GetLength(0));//以距离升序形式排序 2016 1 26
            //Array ary = myRadar.ConvertData(myRadar.idx_r_v, Radar.eBitNum.Bit8);
            if (ary != null)
            {
                text += "检测结果:" + "\r\n";
                string text_X = "距    离(m): ", text_num = "编    号: ";
                string text_Y = "速    度(m/s): ";
                for (int i = 0; i < ary.GetLength(0); i++)
                {
                    //for (int j = 0; j < ary.GetLength(1); j++)
                    text_num += String.Format("{0,10:d}", (i + 1));
                    text_X += String.Format("{0,10:f3}", (Convert.ToDouble(ary.GetValue(i, 0))));//%%
                    text_Y += String.Format("{0,10:f3}", (Convert.ToDouble(ary.GetValue(i, 1))));
                }
                text += text_num + "\r\n";
                text += text_X + "\r\n";
                text += text_Y + "\r\n";

            }
            inforForm1.Show();
            inforForm1.showTargetInfo(text);
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认退出?", "消息",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// 鼠标进入控件的可见部分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Panel_comParam_MouseEnter(object sender, EventArgs e)
        {
            this.panel_comonParams.Focus();
        }

        /// <summary>
        /// 滚动方法          
        /// 处理鼠标滚轮
        /// 当e.Delta > 0时鼠标滚轮是向上滚动，e.Delta < 0时鼠标滚轮向下滚动。
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        void FormSample_MouseWheel(object sender, MouseEventArgs e)
        {
            //获取光标位置  
            Point mousePoint = new Point(e.X, e.Y);
            //换算成相对本窗体的位置  
            mousePoint.Offset(this.Location.X, this.Location.Y);
            //判断是否在panel内  
            if (this.panel_comonParams.RectangleToScreen(
              this.panel_comonParams.DisplayRectangle).Contains(mousePoint))
            {
                //滚动  
                this.panel_comonParams.AutoScrollPosition = new Point(
                0, this.panel_comonParams.VerticalScroll.Value - e.Delta);
            }
        }

        private void groupBox6_MouseHover(object sender, EventArgs e)
        {
            switch (this.comboBox_CP.SelectedIndex)
            {
                case 0://固定
                    myRadar.aRFType = Radar.eRepeatedFrequancyType.typeConstant;
                    this.checkBox_SvDat.CheckState = CheckState.Unchecked;
                    this.radioButton1.Checked = true;
                    this.textBox_prf1.Text = this.labTextCtrl_prf.BoxText;
                    this.groupBox9.Enabled = false;
                    this.groupBox8.Enabled = false;

                    //this.textBox_prf1.Text = myRadar.prfSet[0].ToString();
                    this.textBox_prf2.Text = "";//myRadar.prfSet[1].ToString();
                    this.textBox_prf3.Text = "";// myRadar.prfSet[2].ToString();
                    this.textBox_prf4.Text = "";//myRadar.prfSet[3].ToString();
                    this.textBox_prf5.Text = "";//myRadar.prfSet[4].ToString();
                    this.textBox_prf6.Text = "";//myRadar.prfSet[5].ToString();

                    break;
                case 1://参差
                    myRadar.aRFType = Radar.eRepeatedFrequancyType.typeIrregular;
                    this.checkBox_SvDat.CheckState = CheckState.Unchecked;
                    this.groupBox9.Enabled = true;
                    this.groupBox8.Enabled = true;
                    this.radioButton1.Enabled = false;
                    this.radioButton2.Checked = true;
                    radioButton2_Click(sender, e);
                    break;
                case 2://抖动
                    myRadar.aRFType = Radar.eRepeatedFrequancyType.typeTremble;
                    this.checkBox_SvDat.CheckState = CheckState.Unchecked;
                    this.groupBox9.Enabled = true;
                    this.groupBox8.Enabled = true;
                    this.radioButton1.Enabled = false;
                    this.radioButton2.Checked = true;
                    radioButton2_Click(sender, e);
                    break;
                default:
                    break;
            }
        }

        private void openEchoFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                // Get file name.
                echoFileDirName = openEchoFileDialog.FileName;
                // Write to the file name selected.
                // ... You can write the text from a TextBox instead of a string literal.
                this.textBox_OpenEchoData.Text = echoFileDirName;
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void button_OpenEchoData_Click(object sender, EventArgs e)
        {
            this.openEchoFileDialog.FileName = "输入文件名称";
            this.openEchoFileDialog.ShowDialog();
        }

        private void button_processEchoData_Click(object sender, EventArgs e)
        {
            myRadar.readEchoData(echoFileDirName);
        }

        private void button_ProcessData_Click(object sender, EventArgs e)
        {
            if (textBox_OpenEchoData.Text == "")
            {
                MessageBox.Show("请读入归一化数据");
                return;
            }
            myRadar.readEchoData(echoFileDirName);
            //myRadar.readEchoDataFromTxt(echoFileDirName);

            this.tabControl1.SelectTab(tabPage1);
            setParams();

            //Save results
            if (checkBox_SvDat.CheckState == CheckState.Checked)
            {
                if (saveDirName == null)
                {
                    MessageBox.Show("请选择存储路径！");
                    return;
                }
            }
            else if (checkBox_SvDat.CheckState == CheckState.Unchecked)
            {
            }
            else
            {
                MessageBox.Show("checkBox1 控件处于不确定状态");
            }

            //
            if (myRadar.operateMode != null)
            {
                if (myRadar.operateMode == Radar.eOperateMode.pulseCompressionRadar
                    && myRadar.aPulseCmprsType == Radar.ePulseCmprsType.NULL)
                {
                    MessageBox.Show("请选择脉冲压缩雷达波形！");
                    this.comboBox_waveform.Focus();
                }
                else
                {
                    this.label_RunStatus.Text = "运行状态：正在运行";

                    if (this.backgroundWorker1.IsBusy)
                        return;
                    myRadar.selfCycle = true;
                    this.backgroundWorker1.RunWorkerAsync();//启动多线程

                    if (checkBox_SvDat.CheckState == CheckState.Checked)
                    {
                        if (this.backgroundWorker2.IsBusy)
                            return;
                        while (true)
                        {
                            if (File.Exists(saveDirName + "_echo.dat") || File.Exists(saveDirName + "_img.dat") || File.Exists(saveDirName + "_detected.dat"))
                            {
                                if (MessageBox.Show(saveDirName + "_echo.dat" + "文件已存在，是否覆盖?", "确认信息", MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                                {
                                    File.Delete(saveDirName + "_echo.dat");
                                    File.Delete(saveDirName + "_img.dat");
                                    File.Delete(saveDirName + "_detected.dat");
                                    break;
                                }
                                else
                                {
                                    this.saveFileDialog1.ShowDialog();
                                    saveDirName = saveFileDialog1.FileName;
                                    this.textBox1.Text = saveDirName;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        this.backgroundWorker2.RunWorkerAsync();
                    }
                }

            }

        }

        private void button5_JamParamSettingFormShow(object sender, EventArgs e)
        {

            JamParamSettingForm myJamParamSettingForm = new JamParamSettingForm();
            myJamParamSettingForm.MovNum = Convert.ToInt32(labTextCtrl_JamNo.BoxText);
            //myJamParamSettingForm.rg_mov = new double[(int)myRadar.NumberMov]; 
            myJamParamSettingForm.rg_mov = myRadar.rg_mov();
            myJamParamSettingForm.rv = myRadar.vr();
            myJamParamSettingForm.TargetNum = (int)myRadar.NumberMov;

            myJamParamSettingForm.GetForm(this);
            myJamParamSettingForm.Show();
            //if(myJamParamSettingForm)
        }

        private void tabPage4_SizeChanged_1(object sender, EventArgs e)//使显示随窗口变化而变化
        {
            D = Math.Min(this.tabPage4.Width - 10, this.tabPage4.Height - 10);
            this.draw_tabPage4(D, myRadar.idx_r_v_a, this.tabPage4.Width, this.tabPage4.Height);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)//控制状态标签的显示
        {
            if (this.tabControl1.SelectedTab == tabPage2)
                this.label_RunStatus.Visible = false;
            else
                this.label_RunStatus.Visible = true;
        }

        #endregion

        #region AboutUI
        private void mainForm_myResize()
        {
            //Border width and titlebar height
            int BorderWidth = (this.Width - this.ClientSize.Width) / 2;
            int TitlebarHeight = this.Height - this.ClientSize.Height - 2 * BorderWidth;

            #region GroupBox Menu
            //groupBoxMenu
            Point locPoint = new Point(0, 0);
            this.groupBoxMenu.Width = 400;
            this.groupBoxMenu.Height = this.ClientSize.Height;

            //paneMenuBg
            this.panelMenuBg.Location = new Point(0, 0);
            this.panelMenuBg.Width = this.groupBoxMenu.Width;
            this.panelMenuBg.Height = this.groupBoxMenu.Height;

            //menuStrip1
            this.menuStrip1.Location = new Point(0, 0);

            ////tabControl1
            //this.tabControl1.Location = new Point(0, this.menuStrip1.Height);
            //this.tabControl1.Width = this.panelMenuBg.Width + 4;
            //this.tabControl1.Height = this.panelMenuBg.Height - this.menuStrip1.Height;

            //panel_comonParams
            this.panel_comonParams.Location = new Point(0, this.menuStrip1.Height);
            this.panel_comonParams.Width = this.panelMenuBg.Width;
            this.panel_comonParams.Height = this.panelMenuBg.Height - this.menuStrip1.Height;

            //panel1

            #endregion

            #region GroupBoxView
            //groupBoxResult
            this.groupBoxView.Location = new Point(this.groupBoxMenu.Width, 0);
            this.groupBoxView.Width = this.ClientSize.Width - this.groupBoxMenu.Width;
            this.groupBoxView.Height = this.ClientSize.Height;

            //tabControl
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Width = this.groupBoxView.Width;
            this.tabControl1.Height = this.groupBoxView.Height;

            //tabpage4 20160114
            D = Math.Min(this.tabPage4.Width - 10, this.tabPage4.Height - 10);
            this.draw_tabPage4(D, myRadar.idx_r_v_a, this.tabPage4.Width, this.tabPage4.Height);

            //tabPage1:
            this.label_RunStatus.Location = new Point(this.tabControl1.Width - this.label_RunStatus.Width - 25, 5);//this.panelView.AutoScrollPosition.Y);//this.textBox1.Location.X, this.textBox1.Location.Y + this.textBox1.Height);
            //this.panel2.Location = new Point(this.pictureBox1.Location.X, this.pictureBox1.Location.Y+pictureBox1.Height);//
            this.button_ClearUp.Location = new Point(this.tabPage1.Width - button_ClearUp.Width - 2 - SystemInformation.VerticalScrollBarWidth, this.tabPage1.AutoScrollOffset.Y);

            //panelView
            this.panelView.Location = new Point(this.tabPage1.AutoScrollPosition.X, this.AutoScrollPosition.Y);
            this.panelView.Width = this.tabPage1.Width;
            this.panelView.Height = this.tabPage1.Height;
            //this.panelView.VerticalScroll.Value = this.panelView.VerticalScroll.Minimum;

            ////textBox1
            //this.textBox1.Location = new Point(30, 0);
            //this.textBox1.Width = this.tabPage1.Width - 30 - SystemInformation.VerticalScrollBarWidth;
            //this.textBox1.Height = 200;
            //this.label_t1title.Location = new Point(10, textBox1.Location.Y + textBox1.Height / 2 - label_t1title.Height / 2);

            locPoint.X = 0;
            locPoint.Y = this.button_ClearUp.Height + 10 + this.panelView.AutoScrollPosition.Y;
            this.pictureBox1.Location = locPoint;
            this.pictureBox1.Width = this.panelView.Width - 2 - SystemInformation.VerticalScrollBarWidth;
            this.pictureBox1.Height = this.groupBoxView.Height;// -this.button_ClearUp.Height - 3;

            //tabPage2:
            this.label_perfAna.Location = new Point(0, 0);
            this.button2.Location = new Point(this.tabPage2.Width - this.button2.Width - SystemInformation.VerticalScrollBarWidth, 0);
            //charts
            this.chart1.Location = new Point(0, label_perfAna.Height + 10);
            this.chart1.Width = tabPage2.Width / 3 - 15;
            this.chart1.Height = tabPage2.Height / 2 - 30;

            this.chart2.Location = new Point(chart1.Width + 20, label_perfAna.Height + 10);
            this.chart2.Width = tabPage2.Width / 3 - 15;
            this.chart2.Height = tabPage2.Height / 2 - 30;

            this.chart3.Location = new Point((chart1.Width + 20) * 2, label_perfAna.Height + 10);
            this.chart3.Width = tabPage2.Width / 3 - 15;
            this.chart3.Height = tabPage2.Height / 2 - 30;

            this.chart4.Location = new Point(0, (chart1.Height + 20) + label_perfAna.Height + 10);
            this.chart4.Width = tabPage2.Width / 3 - 15;
            this.chart4.Height = tabPage2.Height / 2 - 30;

            this.chart5.Location = new Point(chart1.Width + 20, (chart1.Height + 20) + label_perfAna.Height + 10);
            this.chart5.Width = tabPage2.Width / 3 - 15;
            this.chart5.Height = tabPage2.Height / 2 - 30;

            this.chart_频域.Location = new Point(00, label_perfAna.Height + 10);
            this.chart_频域.Width = tabPage3.Width - 30;
            this.chart_频域.Height = tabPage3.Height / 2 - 30;

            this.chart_时域.Location = new Point(0, tabPage3.Height / 2);
            this.chart_时域.Width = tabPage3.Width - 30;
            this.chart_时域.Height = tabPage3.Height / 2 - 30;


            chart1.Titles["Title_X"].Text = "信噪比(dB)";
            chart1.Titles["Title_Y"].Text = "距离估计误差(m)";

            chart2.Titles["Title_X"].Text = "信噪比(dB)";
            chart2.Titles["Title_Y"].Text = "速度估计误差(m/s)";

            chart3.Titles["Title_X"].Text = "信噪比(dB)";
            chart3.Titles["Title_Y"].Text = "角度估计误差(rad)";

            chart4.Titles["Title_X"].Text = "信噪比(dB)";
            chart4.Titles["Title_Y"].Text = "检测概率";

            chart5.Titles["Title_X"].Text = "信噪比(dB)";
            chart5.Titles["Title_Y"].Text = "虚警概率";

            chart_频域.Titles["Title_X"].Text = "频率";
            chart_频域.Titles["Title_Y"].Text = "AMP";

            chart_时域.Titles["Title_X"].Text = "时间";
            chart_时域.Titles["Title_Y"].Text = "AMP";

            #endregion
        }

        #endregion
    }
}
