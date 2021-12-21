using CefSharp;
using CefSharp.Wpf;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using WPF_CEF_B2C_Tools.Components;

namespace WPF_CEF_B2C_Tools {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            this.Width = 1336;
            this.Height = 768;
        }

        private void Window_Initialized(object sender, EventArgs e) {
            TabItem tab = new TabItem();
            tab.Header = "淘宝订单采集";
            XingWebBrowser browser = new XingWebBrowser();
            tab.Content = browser;
            this.mainTab.Items.Add(tab);
            browser.RunProcess(BLOs.Taobao.OrderUtil.GetOrder);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e) {

        }

        private void menuTaobaoOrder_Click(object sender, RoutedEventArgs e) {

        }
    }
}
