using System;
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

namespace AgingSystem
{
    /// <summary>
    /// Oprator.xaml 的交互逻辑
    /// </summary>
    public partial class Oprator : Window
    {
        public Oprator()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(DockWindow.m_OpratorNumber))
                tbNumber.Text = DockWindow.m_OpratorNumber;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (tbNumber.Text.Length >= 8)
            {
                e.Handled = true;
                return;
            }
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
            e.Handled = true;
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            if(tbNumber.Text.Length != 8)
            {
                MessageBox.Show("编号格式不正确");
                return;
            }
            DockWindow.m_OpratorNumber = tbNumber.Text;
            Close();
        }

        
    }
}
