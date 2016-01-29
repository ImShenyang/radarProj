using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace radarEchoSimulator
{
    public partial class startForm : Form
    {
        //loadRadar
        private cRadar myRadar = null;// = new cRadar();
        //loadUI
        private mainForm myMainForm = null;

        public startForm()
        {
            InitializeComponent();
        }

        private void startForm_Shown(object sender, EventArgs e)
        {
            myMainForm = new mainForm();
            myMainForm.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.mainForm_FormClosed);

            if (this.backgroundWorker_loadRadar.IsBusy != true)
                this.backgroundWorker_loadRadar.RunWorkerAsync();//启动线程
        }

        private void mainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void backgroundWorker_loadRadar_DoWork(object sender, DoWorkEventArgs e)
        {
            myRadar = new cRadar();
        }

        private void backgroundWorker_loadRadar_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            myMainForm.Show();
            this.Hide();
        }

        private void backgroundWorker_loadRadar_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }
    }
}
