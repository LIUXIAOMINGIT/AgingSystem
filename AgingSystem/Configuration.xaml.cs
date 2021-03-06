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
using System.Windows.Shapes;
using System.Collections;
using System.Configuration;
using Cmd;
using Analyse;

namespace  AgingSystem
{
    /// <summary>
    /// Configuration.xaml 的交互逻辑
    /// </summary>
    public partial class Configuration : Window
    {
        public event EventHandler<ConfigrationArgs> OnSaveConfigration;
        public event EventHandler<SelectedPumpsArgs> OnSelectedPumps;
        
        private int m_DockCount = 0;
        private int m_DockNo = 0;       //货架编号从1开始增加
        private List<Color> m_PumpBackgroundColor = new List<Color>();
        private List<int> m_CheckedPumpLocationList = new  List<int>();//选中的泵的位置编号，在光标跳到下一序列号框中，方便查找
        
        public Configuration()
        {
            InitializeComponent();
            m_PumpBackgroundColor.Add(Colors.Blue);
            m_PumpBackgroundColor.Add(Colors.Yellow);
            m_PumpBackgroundColor.Add(Colors.Red);
            m_PumpBackgroundColor.Add(Colors.Gray);
            m_PumpBackgroundColor.Add(Colors.Green);
        }

        public Configuration(int dockNo)
        {
            InitializeComponent();
            m_DockNo = dockNo;
            m_PumpBackgroundColor.Add(Color.FromRgb(0x00,0xA2,0xE8));
            m_PumpBackgroundColor.Add(Color.FromRgb(0x99,0xD9,0xEA));
            m_PumpBackgroundColor.Add(Color.FromRgb(0x70,0x92,0xBE));
            m_PumpBackgroundColor.Add(Color.FromRgb(0xC8,0xBF,0xE7));
            m_PumpBackgroundColor.Add(Color.FromRgb(0x2E,0x66,0xBA));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = string.Format("配置第{0}号货架", m_DockNo);
            System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (!(int.TryParse(config.AppSettings.Settings["DockCount"].Value, out m_DockCount)))
                m_DockCount = 12;
            int pumpCount = DockInfoManager.Instance().Get(m_DockNo);
            if (pumpCount == 30)
                this.Height = 600;
            InitPumpType();
            LoadDockList();
            LoadPumpList();
            InitParameter();
            InitSelectedPumps();
        }

        /// <summary>
        /// 初始化泵的类型（自定义的类型）
        /// </summary>
        private void InitPumpType()
        {
            cmPumpType.ItemsSource = ProductIDConvertor.GetAllCustomProductIDName();
        }

        /// <summary>
        /// 重新初始化压力等级，C9有个等级，其他泵则只有4个
        /// </summary>
        /// <param name="cid"></param>
        private void InitPressureLevel(CustomProductID cid)
        {
            if (cid == CustomProductID.GrasebyC9)
            {
                cmOcclusionLevel.ItemsSource = Enum.GetNames(typeof(C9OcclusionLevel));
            }
            else
            {
                cmOcclusionLevel.ItemsSource = Enum.GetNames(typeof(OcclusionLevel));
            }
        }

        /// <summary>
        /// 当已经配置完成后，重新再进入配置界面时，要将之前的参数赋值
        /// </summary>
        private void InitParameter()
        {
            if(DockWindow.m_DockParameter.ContainsKey(m_DockNo))
            {
                AgingParameter para = DockWindow.m_DockParameter[m_DockNo] as AgingParameter;
                int index = -1;
                for (int i = 0; i < cmPumpType.Items.Count; i++)
                {
                    if (string.Compare(cmPumpType.Items[i].ToString(), para.PumpType, true) == 0)
                    {
                        index = i;
                        break;
                    }
                }
                if (index >= 0)
                {
                    cmPumpType.SelectedIndex = index;
                    CustomProductID cid = ProductIDConvertor.Name2CustomProductID(para.PumpType);
                    for (int i = 0; i < cmOcclusionLevel.Items.Count; i++)
                    {
                        if (cid == CustomProductID.GrasebyC9)
                        {
                            if (string.Compare(cmOcclusionLevel.Items[i].ToString(), para.C9OclusionLevel.ToString(), true) == 0)
                            {
                                index = i;
                                break;
                            }
                        }
                        else
                        {
                            if (string.Compare(cmOcclusionLevel.Items[i].ToString(), para.OclusionLevel.ToString(), true) == 0)
                            {
                                index = i;
                                break;
                            }
                        }
                    }
                    if (index >= 0)
                    {
                        cmOcclusionLevel.SelectedIndex = index;
                    }
                }
                tbRate.Text = para.Rate.ToString();
                tbVolume.Text = para.Volume.ToString();
                tbCharge.Text = para.ChargeTime.ToString();
                tbRecharge.Text = para.RechargeTime.ToString();
            }
        }

        /// <summary>
        /// 哪些泵已经选中
        /// </summary>
        private void InitSelectedPumps()
        {
            //进入此界面时需要将m_CheckedPumpLocationList清空
            m_CheckedPumpLocationList.Clear();
            if (DockWindow.m_DockPumpList.ContainsKey(m_DockNo))
            {
                List<Tuple<int, int, int, string>> pumpLocation = DockWindow.m_DockPumpList[m_DockNo] as List<Tuple<int, int, int, string>>;
               
                SinglePump pump = null;
                for (int i = 0; i < pumplistGrid.Children.Count; i++)
                {
                    if (pumplistGrid.Children[i] is SinglePump)
                    {
                        pump = pumplistGrid.Children[i] as SinglePump;
                        if (pump.Tag != null)
                        {
                            Tuple<int, int, int, string> location = pumpLocation.Find((x) => { return x.Item1 == (int)pump.Tag;});
                            if (location != null)
                            {
                                pump.chNo.IsChecked = true;
                                pump.SerialNo = location.Item4;
                                if (!m_CheckedPumpLocationList.Contains(pump.PumpLocation))
                                    m_CheckedPumpLocationList.Add(pump.PumpLocation);
                            }
                        }

                    }
                }
             
            }
        }

        private void LoadDockList()
        {
            if (m_DockCount <= 0)
            {
                Logger.Instance().Info("机架数量小于等于0，请重新设置。");
                return;
            }
            //add 20180902 老化系统不稳定，采用一个机架一个应用窗口模式
            if (m_DockCount == 1)
            {
                otherDockCheckBoxGrid.Visibility = Visibility.Collapsed;
                Logger.Instance().Info("LoadDockList()机架数量等于1，不需要显示其他机架");
                return;
            }
            int rowCount = m_DockCount / 5;
            if (m_DockCount % 5 > 0)
                rowCount += 1;

            for (int i = 0; i < rowCount; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = GridLength.Auto;
                otherDockCheckBoxGrid.RowDefinitions.Add(row);
            }
            for (int i = 0; i < 5; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(this.Width / 5-5);//GridLength.Auto;
                otherDockCheckBoxGrid.ColumnDefinitions.Add(col);
            }

            for (int i = 0; i < m_DockCount; i++)
            {
                CheckBox box = new  CheckBox();
                box.Name = "box" + (i + 1).ToString();
                box.Tag = i + 1;
                box.Margin = new Thickness(3, 3, 3, 3);
                box.Padding = new Thickness(5,0,0,0);
                box.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                box.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                box.FontSize = 12;
                box.Content = (i + 1).ToString() + "号货架";
                if(m_DockNo==i + 1)
                {
                    box.IsChecked = true;
                    box.IsEnabled = false;
                }
                otherDockCheckBoxGrid.Children.Add(box);
                Grid.SetRow(box, i / 5);
                Grid.SetColumn(box, i % 5);
            }
        }

        private void LoadPumpList()
        {
            int pumpCount =  DockInfoManager.Instance().Get(m_DockNo);
            int pumpCountPerRow = pumpCount / 5;    //每行有几个机位
            if (pumpCount <= 0)
            {
                Logger.Instance().Info("泵数量小于等于0，请重新设置。");
                return;
            }
            int rowCount = pumpCount / 2;
            if (pumpCount % 2 > 0)
                rowCount += 1;
            for (int i = 0; i < rowCount; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = GridLength.Auto;
                pumplistGrid.RowDefinitions.Add(row);
            }
            int iRow = 0,iCol = 0;
            for (int i = 0, rowIndex = 1; i < pumpCount; i++)
            {
                //AgingSystem:SinglePump  Grid.Row="1"  Grid.Column="1" Margin="1" Cursor="Hand" Background="Blue"/>
                iRow = i/pumpCountPerRow+1;
                iCol = (i+1)%pumpCountPerRow;
                if(iCol==0)
                    iCol = pumpCountPerRow;
                SinglePump pump = new SinglePump(m_DockNo, i+1, iRow, iCol);
                pump.Name = "pump" + (i + 1).ToString();
                pump.Tag = i + 1;
                pump.Margin = new Thickness(1, 1, 1, 1);
                pump.Cursor = Cursors.Hand;
                pump.SetPump(i + 1, "", "");
                pump.OnClickCheckBox += OnSinglePumpClickCheckBox;
                pump.OnSerialNoTypeIn += OnSerialNoInputComplete;
                pumplistGrid.Children.Add(pump);
                Grid.SetRow(pump, rowIndex);
                if (i % 2 == 1)
                    ++rowIndex;
                Grid.SetColumn(pump, i % 2);
                pump.Background = new SolidColorBrush(m_PumpBackgroundColor[i / pumpCountPerRow]);
            }
        }

        /// <summary>
        /// 双道F6 WZ50F6都不用点击第二道，当点击第一道时，第二道自动勾选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSinglePumpClickCheckBox(object sender, SinglePumpArgs e)
        {
            SinglePump self = sender as SinglePump;
            SinglePump pump = null;
            if (e != null)
            {
                //如果是双道泵
                if (e.EnableCheckBoxClick)
                {
                    //点击奇数位泵偶数位也要相应点击,反之亦然
                    if (e.PumpLocation % 2 == 0 && e.PumpLocation > 0)
                    {
                        pump = FindPumpByLocation(e.PumpLocation - 1);
                        if (pump != null)
                        {
                            pump.chNo.IsChecked = self.chNo.IsChecked;
                        }
                    }
                    else if (e.PumpLocation % 2 == 1)
                    {
                        pump = FindPumpByLocation(e.PumpLocation + 1);
                        if (pump != null)
                        {
                            pump.chNo.IsChecked = self.chNo.IsChecked;
                        }
                    }

                    if (pump != null && pump.chNo.IsChecked == true)
                    {
                        if (!m_CheckedPumpLocationList.Contains(pump.PumpLocation))
                            m_CheckedPumpLocationList.Add(pump.PumpLocation);
                    }
                    else
                    {
                        if (m_CheckedPumpLocationList.Contains(pump.PumpLocation))
                            m_CheckedPumpLocationList.Remove(pump.PumpLocation);
                    }
                }

                if (self.chNo.IsChecked == true)
                {
                    if (!m_CheckedPumpLocationList.Contains(e.PumpLocation))
                        m_CheckedPumpLocationList.Add(e.PumpLocation);
                }
                else
                {
                    if (m_CheckedPumpLocationList.Contains(e.PumpLocation))
                        m_CheckedPumpLocationList.Remove(e.PumpLocation);
                }
                m_CheckedPumpLocationList.Sort();//默认是从小到大排序
            }
        }

        /// <summary>
        /// 当条码枪完成一次扫码，光标自动跳到下一条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSerialNoInputComplete(object sender, SerialNoInputArgs e)
        {
            if (m_CheckedPumpLocationList.Count <= 0)
                return;
            if (cmPumpType.SelectedIndex < 0)
                return;
            string strPumpType = cmPumpType.Items[cmPumpType.SelectedIndex].ToString();
            CustomProductID cid = ProductIDConvertor.Name2CustomProductID(strPumpType);
            int index = m_CheckedPumpLocationList.FindIndex((x) => { return e.PumpLocation == x; });
            if (index >= 0 && index + 1 < m_CheckedPumpLocationList.Count)
            {
                if (cid == CustomProductID.GrasebyF6_Double || cid == CustomProductID.WZS50F6_Double)
                {
                    #region
                    SinglePump pump = null;
                    //第一道泵序列号与第二道 一样
                    if (index % 2 == 0)
                    {
                        if (m_CheckedPumpLocationList.Count > index + 1)
                        {
                            pump = FindPumpByLocation(m_CheckedPumpLocationList[index + 1]);
                            if (pump != null)
                            {
                                pump.SetSerialNo(e.SerialNo);
                            }
                        }
                        if (m_CheckedPumpLocationList.Count > index + 2)
                        {
                            //光标跳到下一个泵
                            pump = FindPumpByLocation(m_CheckedPumpLocationList[index + 2]);
                            if (pump != null)
                            {
                                pump.SetCursor();
                            }
                        }
                    }
                    else
                    {
                        if (m_CheckedPumpLocationList.Count > index - 1)
                        {
                            pump = FindPumpByLocation(m_CheckedPumpLocationList[index - 1]);
                            if (pump != null)
                            {
                                pump.SetSerialNo(e.SerialNo);
                            }
                        }
                        //光标跳到下一个泵
                        if (m_CheckedPumpLocationList.Count > index + 1)
                        {
                            pump = FindPumpByLocation(m_CheckedPumpLocationList[index + 1]);
                            if (pump != null)
                            {
                                pump.SetCursor();
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    SinglePump pump = FindPumpByLocation(m_CheckedPumpLocationList[index + 1]);
                    if (pump != null)
                    {
                        pump.SetCursor();
                    }
                }
            }
        }

        /// <summary>
        /// 每个泵都有一个编号，从1开始,按照位置一定能找到
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private SinglePump FindPumpByLocation(int location)
        {
            SinglePump pump = null;
            for (int i = 0; i < pumplistGrid.Children.Count; i++)
            {
                if (pumplistGrid.Children[i] is SinglePump)
                {
                    pump = pumplistGrid.Children[i] as SinglePump;
                    if (pump.PumpLocation == location)
                        break;
                    else
                        continue;
                }
            }
            return pump;
        }

        private void OnSelect(object sender, RoutedEventArgs e)
        {
            ParameterList parameterList = new ParameterList();
            bool? bRet = parameterList.ShowDialog();
            if(bRet.HasValue)
            {
                if(bRet==true)
                {
                    int index = -1;
                    for(int i = 0;i<cmPumpType.Items.Count;i++)
                    {
                        if (string.Compare(cmPumpType.Items[i].ToString(), parameterList.Args.m_PumpType, true) == 0)
                        {
                            index = i;
                            break;
                        }
                    }
                    if(index>=0)
                    {
                        cmPumpType.SelectedIndex = index;
                    }
                    for(int i = 0;i<cmOcclusionLevel.Items.Count;i++)
                    {
                        if( string.Compare(cmOcclusionLevel.Items[i].ToString(),parameterList.Args.m_OccLevel, true)==0)
                        {
                            index = i;
                            break;
                        }
                    }
                    if(index>=0)
                    {
                        cmOcclusionLevel.SelectedIndex = index;
                    }
                    tbRate.Text = parameterList.Args.m_Rate;
                    tbVolume.Text = parameterList.Args.m_Volume;
                    tbCharge.Text = parameterList.Args.m_ChargeTime;
                    tbRecharge.Text = parameterList.Args.m_RechargeTime;
                }
                else
                {

                }
            }
        }

        private void OnSave(object sender, RoutedEventArgs e)
        { 
            if( string.IsNullOrEmpty(tbRate.Text.Trim())
                ||string.IsNullOrEmpty(tbVolume.Text.Trim())
                ||string.IsNullOrEmpty(tbCharge.Text.Trim())
                ||string.IsNullOrEmpty(tbRecharge.Text.Trim())
                ||cmPumpType.SelectedIndex<0
                ||cmOcclusionLevel.SelectedIndex<0
                )
            {
                MessageBox.Show("请将参数填写完整");
                return;
            }
            string szCustomPid = cmPumpType.Items[cmPumpType.SelectedIndex].ToString();
            string szPressureLevel = cmOcclusionLevel.Items[cmOcclusionLevel.SelectedIndex].ToString();
            decimal rate           = Convert.ToDecimal(float.Parse(tbRate.Text).ToString("F1"));
            decimal volume         = Convert.ToDecimal(float.Parse(tbVolume.Text).ToString("F1"));
            decimal charge         = Convert.ToDecimal(tbCharge.Text);

            CustomProductID cid = ProductIDConvertor.Name2CustomProductID(szCustomPid);
            OcclusionLevel level = OcclusionLevel.H;
            C9OcclusionLevel c9Level = C9OcclusionLevel.Level3;

            if (cid == CustomProductID.GrasebyC9)
            {
                if (Enum.IsDefined(typeof(C9OcclusionLevel), szPressureLevel))
                    c9Level = (C9OcclusionLevel)Enum.Parse(typeof(C9OcclusionLevel), szPressureLevel);
                else
                    Logger.Instance().ErrorFormat("Configuration::OnSave()->压力转换出错,C9OcclusionLevel={0}", szPressureLevel);
            }
            else
            {
                if (Enum.IsDefined(typeof(OcclusionLevel), szPressureLevel))
                    level = (OcclusionLevel)Enum.Parse(typeof(OcclusionLevel), szPressureLevel);
                else
                    Logger.Instance().ErrorFormat("Configuration::OnSave()->压力转换出错,OcclusionLevel={0}", szPressureLevel);
            }
            
            decimal discharge = 0;
            decimal recharge  = Convert.ToDecimal(tbRecharge.Text);
            Hashtable dockParameter = new Hashtable();
            AgingParameter para = new AgingParameter(szCustomPid,
                                                     rate,
                                                     volume,
                                                     charge,
                                                     discharge,
                                                     recharge,
                                                     level,
                                                     c9Level);


            if (m_DockCount == 1)
            {
                dockParameter.Add(m_DockNo, new AgingParameter(para));
            }
            else
            {
                for (int i = 0; i < otherDockCheckBoxGrid.Children.Count; i++)
                {
                    if (otherDockCheckBoxGrid.Children[i] is CheckBox)
                    {
                        CheckBox box = otherDockCheckBoxGrid.Children[i] as CheckBox;
                        if (box.IsChecked.HasValue)
                        {
                            if (box.IsChecked == true)
                            {
                                dockParameter.Add((int)box.Tag, new AgingParameter(para));    //这里需要拷贝一份新的参数
                            }
                        }
                    }
                }
            }
            if(OnSaveConfigration!=null)
                OnSaveConfigration(this, new ConfigrationArgs(dockParameter));
            this.DialogResult = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SinglePump pump = null;
            List<Tuple<int, int, int, string>> pumpLocationList = new List<Tuple<int, int, int, string>>();
            for (int i = 0; i < pumplistGrid.Children.Count; i++)
            {
                if (pumplistGrid.Children[i] is SinglePump)
                {
                    pump = pumplistGrid.Children[i] as SinglePump;
                    if (pump.chNo.IsChecked.HasValue && pump.chNo.IsChecked==true)
                        pumpLocationList.Add(new Tuple<int, int, int, string>((int)pump.Tag, pump.RowNo, pump.ColNo, pump.SerialNo));
                }
            }
            Hashtable hashPumps = new Hashtable();
            hashPumps.Add(m_DockNo, pumpLocationList);
            if(OnSelectedPumps!=null)
            {
                OnSelectedPumps(this, new SelectedPumpsArgs(hashPumps));
            }
            GC.Collect();
        }

        /// <summary>
        /// 单击选择全部或反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectAllPump(object sender, RoutedEventArgs e)
        {
            SinglePump pump = null;
            for(int i=0;i<pumplistGrid.Children.Count; i++)
            {
                if (pumplistGrid.Children[i] is SinglePump)
                {
                    pump = pumplistGrid.Children[i] as SinglePump;
                    pump.chNo.IsChecked = this.chNo.IsChecked;
                    if (this.chNo.IsChecked == true)
                    {
                        if (!m_CheckedPumpLocationList.Contains(pump.PumpLocation))
                            m_CheckedPumpLocationList.Add(pump.PumpLocation);
                    }
                    else
                    {
                        if (m_CheckedPumpLocationList.Contains(pump.PumpLocation))
                            m_CheckedPumpLocationList.Remove(pump.PumpLocation);
                    }
                }
            }
        }

        /// <summary>
        /// 选择泵类型,下拉列表框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SinglePump pump = null;
            string strPumpType = string.Empty;
            int index = 0;
            CustomProductID cid = CustomProductID.Unknow;
            for (int i = 0; i < pumplistGrid.Children.Count; i++)
            {
                if (pumplistGrid.Children[i] is SinglePump)
                {
                    pump = pumplistGrid.Children[i] as SinglePump;
                    strPumpType = cmPumpType.Items[cmPumpType.SelectedIndex].ToString();
                    cid = ProductIDConvertor.Name2CustomProductID(strPumpType);
                    InitPressureLevel(cid);
                    if (cid == CustomProductID.GrasebyF6_Double || cid == CustomProductID.WZS50F6_Double)
                    {
                        if (pump.Tag != null && !string.IsNullOrEmpty(pump.Tag.ToString()))
                        {
                            if (int.TryParse(pump.Tag.ToString(), out index))
                            {
                                if (index % 2 == 1)
                                    pump.lbPumpType.Content = strPumpType + " 1道";
                                else
                                    pump.lbPumpType.Content = strPumpType + " 2道";
                            }
                        }
                        pump.EnableCheckBoxClick = true;
                    }
                    else
                    {
                        pump.EnableCheckBoxClick = false;
                        pump.lbPumpType.Content = strPumpType;
                    }
                }
            }
        }

        private void OnOcclusionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
       
        private void TextBoxChanged(object sender, TextChangedEventArgs e, float max)
        {
            TextBox obj = sender as TextBox;
            InputValidation.ValidateTextBox(obj, max, 1);
            try
            {
                if (Convert.ToSingle(obj.Text.Trim('.')) > max)
                {
                    obj.Text = string.Empty;
                }
            }
            catch
            {
                obj.Text = string.Empty;
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Key key = Key.D0;
            while (key <= Key.D9)
            {
                if (e.KeyboardDevice.IsKeyDown(key))
                {
                    e.Handled = false;
                    return;
                }
                key += 1;
            }
            key = Key.NumPad0;
            while (key <= Key.NumPad9)
            {
                if (e.KeyboardDevice.IsKeyDown(key))
                {
                    e.Handled = false;
                    return;
                }
                key += 1;
            }
            if (e.KeyboardDevice.IsKeyDown(Key.OemPeriod) || e.KeyboardDevice.IsKeyDown(Key.Decimal))
            {
                e.Handled = false;
                return;
            }
            e.Handled = true;
        }

        private void tbRate_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(sender, e);
        }

        private void tbVolume_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(sender, e);
        }

        private void tbVolume_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxChanged(sender, e, 9999f);
        }

        private void tbCharge_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxChanged(sender, e, 24);
        }

        private void tbCharge_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(sender, e);
        }

        private void tbRecharge_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(sender, e);
        }

        private void tbRecharge_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxChanged(sender, e, 20);
        }

        private void tbRate_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxChanged(sender, e, 1200.0f);
        }
        
      
    }
}
