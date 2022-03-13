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

        private XingWebBrowser appendTabItem(string tabTitle) {
            TabItem tab = new TabItem();
            tab.Header = tabTitle;
            XingWebBrowser browser = new XingWebBrowser();
            tab.Content = browser;
            this.mainTab.Items.Add(tab);
            return browser;
        }

        private void Window_Initialized(object sender, EventArgs e) {
            XingWebBrowser browser = this.appendTabItem("淘宝订单采集");
            ProcessEngine.RunProcess(BLOs.Taobao.OrderUtil.GetOrder, browser);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e) {

        }

        private void menuTaobaoOrder_Click(object sender, RoutedEventArgs e) {

        }

        private void menuPDDBK_Click(object sender, RoutedEventArgs e) {
            XingWebBrowser browser = this.appendTabItem("拼多多订单管理");

        }
    }
}
