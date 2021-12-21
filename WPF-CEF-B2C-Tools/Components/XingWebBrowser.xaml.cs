using System.Windows.Controls;
using CefSharp;
using CefSharp.Wpf;
using System.IO;
using System;
using System.Windows;

namespace WPF_CEF_B2C_Tools.Components {
    /// <summary>
    /// XingWebBrows.xaml 的交互逻辑
    /// </summary>
    public partial class XingWebBrowser : UserControl {
        Process _process;
        public XingWebBrowser() {
            CefSettings settings = new CefSettings();
            settings.CachePath = Directory.GetCurrentDirectory() + @"\cef_cache";
            settings.MultiThreadedMessageLoop = true;
            settings.CefCommandLineArgs.Add("proxy-auto-detect", "0");
            settings.CefCommandLineArgs.Add("--disable-web-security", "");
            if (!Cef.IsInitialized) {
                Cef.Initialize(settings);
            }
            InitializeComponent();         
        }
        private void Cef_AddressChanged(object sender, DependencyPropertyChangedEventArgs e) {
            txtAddrees.Text = e.NewValue.ToString();
        }
        private async void Cef_FrameLoadEnd(object sender, FrameLoadEndEventArgs e) {
            ChromiumWebBrowser cef = (ChromiumWebBrowser)sender;
            BrowserTask task = (BrowserTask)this._process.tasks[this._process.currentTask];
            var respons = await cef.EvaluateScriptAsync("()=>{" + task.script + "}");
            if (respons.Success && respons.Result is IJavascriptCallback) {
                respons = await ((IJavascriptCallback)respons.Result).ExecuteAsync();
                if (respons.Success) {
                    this.RunProcess(this._process);
                }
            }
        }
        public void RunProcess(Process process) {
            this._process = process;
            int index = process.currentTask + 1;
            if (index < process.tasks.Count) {
                process.currentTask = index;
                BrowserTask task = (BrowserTask)process.tasks[index];
                browser.Address = task.url;
            }
        }

        private void UserControl_Initialized(object sender, EventArgs e) {
            browser.RequestHandler = new XingBasicRequestHandler();
            browser.FrameLoadEnd += Cef_FrameLoadEnd;
            browser.AddressChanged += Cef_AddressChanged;
            browser.KeyboardHandler = new CEFKeyBoardHander();
        }
    }

    class XingRequestHandler : CefSharp.Handler.ResourceRequestHandler {
        private readonly System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

        protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response) {
            return new CefSharp.ResponseFilter.StreamResponseFilter(memoryStream);
        }

        protected override void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength) {
            //You can now get the data from the stream
            var bytes = memoryStream.ToArray();

            if (response.Charset == "utf-8") {
                var str = System.Text.Encoding.UTF8.GetString(bytes);
                Console.WriteLine("In OnResourceLoadComplete : " + str.Substring(0, 10) + " <...>");
            } else {
                //Deal with different encoding here
            }
        }
    }
    public class XingBasicRequestHandler : CefSharp.Handler.RequestHandler {
        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling) {
            //Only intercept specific Url's
            //https://trade.taobao.com/trade/itemlist/list_sold_items.htm?userSwitch=1
            //https://trade.taobao.com/trade/itemlist/asyncSold.htm?event_submit_do_query=1&_input_charset=utf8&sifg=0
            if (request.Url.Contains("/asyncSold.htm")) {
                return new XingRequestHandler();
            }
            //Default behaviour, url            will be loaded normally.
            return null;
        }

    }

    public class CEFKeyBoardHander : IKeyboardHandler {
        public bool OnKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey) {
            if (type == KeyType.RawKeyDown) {
                switch (windowsKeyCode) {
                    //F12
                    case 123:
                        browser.ShowDevTools();
                        break;
                    //F5
                    case 116:
                        if (modifiers == CefEventFlags.ControlDown) {
                            //MessageBox.Show("ctrl+f5");
                            browser.Reload(true); //强制忽略缓存
                        } else {
                            //MessageBox.Show("f5");
                            browser.Reload();
                        }
                        break;
                }
            }
            return false;
        }

        public bool OnPreKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut) {
            return false;
        }
    }
}
