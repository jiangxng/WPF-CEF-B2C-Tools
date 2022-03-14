using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using WPF_CEF_B2C_Tools.Components;

namespace WPF_CEF_B2C_Tools {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        Hashtable scripts;

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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            this.scripts = new Hashtable();
            this.scripts.Add("tbLogin", BLOs.Taobao.OrderUtil.Login);
            this.scripts.Add("tbFetchOrderData", BLOs.Taobao.OrderUtil.FetchOrderData);
            this.scripts.Add("openBaidu", BLOs.Test.Class1.OpenBaidu);
            this.scripts.Add("openTestPage", BLOs.Test.Class1.OpenTestPage);
            this.scripts.Add("execInjoinScript", BLOs.Test.Class1.execInjoinScript);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            MenuItem menuItem = (MenuItem)sender;
            string cmd = menuItem.DataContext.ToString();
            if (cmd.StartsWith('N')) {
                this.appendTabItem(menuItem.Header.ToString());
                cmd = cmd.TrimStart('N');
            }
            XingWebBrowser browser = (XingWebBrowser)((TabItem)this.mainTab.SelectedItem).Content;
            if (cmd.Length > 0) {
                ProcessEngine.RunProcess((Process)this.scripts[cmd], browser);
            }
        }
    }
}
