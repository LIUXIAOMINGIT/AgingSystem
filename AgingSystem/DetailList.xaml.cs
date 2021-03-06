﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Cmd;
using Analyse;
using log4net;

namespace  AgingSystem
{
    /// <summary>
    /// Interaction logic for DetailList.xaml
    /// </summary>
    public partial class DetailList : Window
    {
        private int                      m_DockNo;
        private AgingParameter           m_Parameter;
        private List<Tuple<int, int, int, string>> m_PumpLocationList;//int pumpLocation,int rowNo,int colNo 
        private List<AgingPump>          m_AgingPumpList;

        public DetailList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 查看详细老化信息，一定要传入这些参数
        /// </summary>
        /// <param name="dockNo"></param>
        /// <param name="parameter"></param>
        /// <param name="pumpLocationList"></param>
        /// <param name="AgingPumpList"></param>
        public DetailList(int dockNo, AgingParameter parameter, List<Tuple<int, int, int, string>> pumpLocationList, List<AgingPump> AgingPumpList)
        {
            InitializeComponent();
            m_DockNo = dockNo;
            m_Parameter = parameter;
            m_PumpLocationList = pumpLocationList;
            m_AgingPumpList = AgingPumpList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ProductID pid = ProductID.Unknow;
            CustomProductID cid = CustomProductID.Unknow;
            if (Enum.IsDefined(typeof(CustomProductID), m_Parameter.PumpType))
            {
                cid = (CustomProductID)Enum.Parse(typeof(CustomProductID), m_Parameter.PumpType);
                pid = ProductIDConvertor.Custom2ProductID(cid);
            }
            int pumpCount = m_PumpLocationList.Count;
            if (pid == ProductID.GrasebyF8)
            {
                pumpCount = pumpCount * 2;
            }

            if (m_PumpLocationList != null && pumpCount <= 15)
            {
                this.Height += pumpCount * 40;
                this.Height += 8;
                scroll.Height += pumpCount * 40;
                scroll.Height += 8;
            }
            else
            {
                this.Height += 16*40;
                this.Height += 8;
                scroll.Height +=15*40;
            }

            LoadDetailList();
        }

        /// <summary>
        /// 只显示那些用户选中的泵信息
        /// </summary>
        private void LoadDetailList()
        {
            //列表中要显示几台泵
            int pumpCount = m_PumpLocationList.Count;
           
          
            if (pumpCount <= 0)
            {
                Logger.Instance().Info("老化泵数量等于0。");
                return;
            }
            CustomProductID cid = ProductIDConvertor.Name2CustomProductID(m_Parameter.PumpType);
            ProductID pid = ProductIDConvertor.Custom2ProductID(cid);
            if (pid == ProductID.GrasebyF8)
                pumpCount = pumpCount * 2;//F8则需要X2

           
            for (int i = 0; i < pumpCount; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(40);
                pumpListGrid.RowDefinitions.Add(row);
            }
          
            for (int i = 0; i < pumpCount; i++)
            {
                SingleDetail detail = new SingleDetail();
                //F6,F8双道泵显示详细内容与其他泵不一样，要体现双道信息
                if (cid == CustomProductID.GrasebyF6_Double || cid == CustomProductID.WZS50F6_Double)
                {
                    if (m_PumpLocationList[i].Item3 % 2 == 0)
                    {
                        detail.lbPumpLocation.Content = string.Format("{0}-{1}-{2}(2道泵)", m_DockNo, m_PumpLocationList[i].Item2, m_PumpLocationList[i].Item3);
                    }
                    else
                    {
                        detail.lbPumpLocation.Content = string.Format("{0}-{1}-{2}(1道泵)", m_DockNo, m_PumpLocationList[i].Item2, m_PumpLocationList[i].Item3);
                    }
                    detail.lbPumpSN.Content = m_PumpLocationList[i].Item4;
                }
                else if (cid == CustomProductID.GrasebyF8)
                {
                    if(i%2==0)
                        detail.lbPumpLocation.Content = string.Format("{0}-{1}-{2}(1道泵)", m_DockNo, m_PumpLocationList[i/2].Item2, m_PumpLocationList[i/2].Item3);
                    else
                        detail.lbPumpLocation.Content = string.Format("{0}-{1}-{2}(2道泵)", m_DockNo, m_PumpLocationList[i/2].Item2, m_PumpLocationList[i/2].Item3);
                    detail.lbPumpSN.Content = m_PumpLocationList[i / 2].Item4;
                }
                else
                {
                    detail.lbPumpSN.Content = m_PumpLocationList[i].Item4;
                    detail.lbPumpLocation.Content = string.Format("{0}-{1}-{2}", m_DockNo, m_PumpLocationList[i].Item2, m_PumpLocationList[i].Item3);
                }
                detail.Name = "detail" + (i + 1).ToString();
                detail.Tag = i + 1;
                detail.Margin = new Thickness(1, 1, 1, 1);
                detail.lbNo.Content = string.Format("{0}",i + 1);
                detail.lbPumpType.Content = m_Parameter.PumpType;
                detail.lbRate.Content = m_Parameter.Rate.ToString();
                AgingPump AgingPump = null;
                if (cid == CustomProductID.GrasebyF8)
                {
                    //F8双通道共用一个串口，在查找泵时需要指定通道编号从0开始
                    AgingPump = m_AgingPumpList.Find((x) => { return x.DockNo == m_DockNo && x.RowNo == m_PumpLocationList[i/2].Item2 && x.Channel == m_PumpLocationList[i/2].Item3 && x.SubChannel==i % 2; });
                }
                else
                    AgingPump = m_AgingPumpList.Find((x)=>{return x.DockNo==m_DockNo && x.RowNo==m_PumpLocationList[i].Item2 && x.Channel==m_PumpLocationList[i].Item3;});
                if (AgingPump != null)
                {
                    if (AgingPump.BeginAgingTime.Year > 2000)
                        detail.lbAgingStartTime.Content = AgingPump.BeginAgingTime.ToString("MM-dd HH:mm:ss");
                    if (AgingPump.BeginDischargeTime.Year > 2000)
                        detail.lbDischargeStartTime.Content = AgingPump.BeginDischargeTime.ToString("MM-dd HH:mm:ss");
                    if (AgingPump.BeginLowVoltageTime.Year > 2000)
                        detail.lbLowVoltageStartTime.Content = AgingPump.BeginLowVoltageTime.ToString("MM-dd HH:mm:ss");
                    if (AgingPump.BeginBattaryDepleteTime.Year > 2000)
                        detail.lbDepleteStartTime.Content = AgingPump.BeginBattaryDepleteTime.ToString("MM-dd HH:mm:ss");
                    if (AgingPump.EndAgingTime.Year > 2000)
                        detail.lbAgingEndTime.Content = AgingPump.EndAgingTime.ToString("MM-dd HH:mm:ss");
                    detail.lbAlarm.Content = AgingPump.GetAlarmString();
                    detail.lbAgingStatus.Content = AgingStatusMetrix.Instance().GetAgingStatus(AgingPump.AgingStatus);
                    if (AgingPump.AgingStatus == EAgingStatus.Unknown)
                        Logger.Instance().InfoFormat("泵状态显示异常，为未状态开始老化时间＝{0}", AgingPump.BeginAgingTime.ToString());
                }
                else
                {
                    detail.lbAgingStartTime.Content = "";
                    detail.lbDischargeStartTime.Content = "";
                    detail.lbLowVoltageStartTime.Content = "";
                    detail.lbDepleteStartTime.Content = "";
                    detail.lbAgingEndTime.Content = "";
                    detail.lbAlarm.Content = "";
                    detail.lbAgingStatus.Content = "";
                }
                pumpListGrid.Children.Add(detail);
                Grid.SetRow(detail, i+1);
                if (i % 2 == 0)
                    detail.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0x00,0xA2,0xE8));
            }

        }

        /// <summary>
        /// 窗口关闭的时候清空所有对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int pumpCount = m_PumpLocationList.Count;
            pumpListGrid.Children.Clear();
            pumpListGrid.RowDefinitions.Clear();
            GC.Collect();
        }

      
    }
}
