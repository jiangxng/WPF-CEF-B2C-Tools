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

        private XingWebBrowser appendTabItem(string tabTitle, bool isBackpage = true) {
            TabItem tab = new TabItem();
            tab.Header = tabTitle;
            XingWebBrowser browser = new XingWebBrowser();
            tab.Content = browser;
            this.mainTab.Items.Add(tab);
            if (!isBackpage) this.mainTab.SelectedItem = tab;
            return browser;
        }

        private XingWebBrowser appendTabItem(string tabTitle) {
            return this.appendTabItem(tabTitle, false);
        }

        private void Window_Initialized(object sender, EventArgs e) {
            //XingWebBrowser browser = this.appendTabItem("淘宝订单采集");
            //ProcessEngine.RunProcess(BLOs.Taobao.OrderUtil.GetOrder, browser);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e) {

        }

        private void menuTaobaoOrder_Click(object sender, RoutedEventArgs e) {
            MenuItem menuItem = (MenuItem)sender;
            MenuItem menuItemParent = (MenuItem)menuItem.Parent;
        }

        private void menuPDDBK_Click(object sender, RoutedEventArgs e) {
            XingWebBrowser browser = this.appendTabItem("拼多多订单管理");

        }

        private void menuOpenTestPage_onClick(object sender, RoutedEventArgs e) {
            XingWebBrowser browser = this.appendTabItem("脚本测试");
            browser.Open("D:/Documents/WPF-CEF-B2C-Tools/WPF-CEF-B2C-Tools/blank.html");
        }
    }
}
