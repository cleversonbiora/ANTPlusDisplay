using ANT.Framework;
using ANT.Framework.DataSources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ANT_Display
{
    public partial class Form1 : Form
    {
        public ANTManger antMgr;
        System.Timers.Timer updateTimer = new System.Timers.Timer(1000);

        public Form1()
        {
            InitializeComponent();

             antMgr = new ANTManger();
            
            foreach (ANTManger.AntPlus_Connection i in antMgr.deviceList)
            {
                Debug.WriteLine(i.dataSource.getCurrentDistance().ToString());
            }
            updateTimer.Elapsed += new System.Timers.ElapsedEventHandler(updateTimer_Tick);
            updateTimer.Start();
        }

        private void updateTimer_Tick(object sender, ElapsedEventArgs e)
        {
            var lastSeenData = antMgr.deviceList[0].dataSource.getLastDataRcvd();
            var lastSeenData2 = antMgr.deviceList[1].dataSource.getLastDataRcvd();
            ReceberMensagem(lastSeenData, lastSeenData2);
            //foreach (ANTManger.AntPlus_Connection i in antMgr.deviceList)
            //{
            //    if(i.connectedChannel != null)
            //    {
            //        Debug.WriteLine(i.dataSource.getCurrentDistance().ToString());
            //    }
            //}
        }

        delegate void ReceberMensagemCallback(DataSourcePacket lastSeenData, DataSourcePacket lastSeenData2);
        void ReceberMensagem(DataSourcePacket lastSeenData, DataSourcePacket lastSeenData2)
        {
            if (InvokeRequired)
            {
                ReceberMensagemCallback callback = ReceberMensagem;
                Invoke(callback, lastSeenData, lastSeenData2);
            }
            else
            {
                if (lastSeenData != null)
                {
                    lblDist.Text = Math.Round(lastSeenData.distance).ToString()+ " m";
                    lblVel.Text = Math.Round(lastSeenData.speed_ms * 3.6).ToString() + " km/h";
                }
                if (lastSeenData2 != null)
                {
                    lblCad.Text = Math.Round(lastSeenData2.cadence, 2).ToString() + " rpm";
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
