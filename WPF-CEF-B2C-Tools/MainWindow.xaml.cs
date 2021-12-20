using CefSharp;
using CefSharp.Wpf;
using System;
using System.Windows;

namespace WPF_CEF_B2C_Tools {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e) {
            CefSettings settings = new CefSettings();
            settings.CachePath = System.AppDomain.CurrentDomain.BaseDirectory + "/cef_cache";
            
            if (!Cef.IsInitialized)
            {
                Cef.Initialize(settings);
            }

        }



        private void MenuItem_Click_1(object sender, RoutedEventArgs e) {

        }


        private void menuTaobaoOrder_Click(object sender, RoutedEventArgs e) {

        }
    }
}
