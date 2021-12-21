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
            tab.Header = "宝贝订单采集";
            tab.Content = new XingWebBrowser();
            this.mainTab.Items.Add(tab);

            cef_taobao.RequestHandler = new XingBasicRequestHandler();
            cef_taobao.FrameLoadEnd += Cef_taobao_FrameLoadEnd;
            cef_taobao.AddressChanged += Cef_taobao_AddressChanged;
            cef_taobao.KeyboardHandler = new CEFKeyBoardHander();
        }



        private void MenuItem_Click_1(object sender, RoutedEventArgs e) {

        }


        private void menuTaobaoOrder_Click(object sender, RoutedEventArgs e) {

        }
    }
}
