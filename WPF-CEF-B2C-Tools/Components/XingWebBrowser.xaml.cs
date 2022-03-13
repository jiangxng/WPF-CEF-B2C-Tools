using System.Windows.Controls;
using CefSharp;
using CefSharp.Wpf;
using System.IO;
using System;
using System.Windows;

namespace WPF_CEF_B2C_Tools.Components {
    public class JsEvent {
        public string MessageText = string.Empty;


        public void ShowTest() {
            MessageBox.Show("this in C#.\n\r" + MessageText);
        }
    }

    /// <summary>
    /// XingWebBrows.xaml 的交互逻辑
    /// </summary>
    public partial class XingWebBrowser : UserControl {
        BrowserTask _task;
        string injection_jquery = @"
        function () {
            function getScript(url, success) {
                var script = document.createElement('script');
                script.src = url;
                var head = document.getElementsByTagName('head')[0],
                    done = false;
                script.onload = script.onreadystatechange = function () {
                    if (!done && (!this.readyState || this.readyState == 'loaded' || this.readyState == 'complete')) {
                        done = true;
                        success();
                        script.onload = script.onreadystatechange = null;
                        head.removeChild(script);
                    }
                };
                head.appendChild(script);
            }

            getScript('http://code.jquery.com/jquery-latest.min.js', function () {
                if (typeof jQuery == '') {
                    console.log('Sorry, but jQuery wasn\'t able to load');
                } else {
                    console.log('This page is now jQuerified with v' + $.fn.jquery);
                    $(document).ready(function () {
                        //here you can write your jquery code
                    });
                }
            });
        };";

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
            
            //cef.ExecuteScriptAsync(injection_jquery);
            //每次浏览器完成加载后执行【脚本流程】的【当前任务】
            //BrowserTask task = (BrowserTask)this._process.tasks[this._process.currentTask];
            //cef.ExecuteScriptAsync(task.runOnLoad);

        }
        /// <summary>
        /// 运行【任务流程】浏览器的入库
        /// </summary>
        /// <param name="process"></param>
        public void RunTask(BrowserTask task) {
            this._task = task;
            if (task.Type == TaskType.Redirect) {
                this.browser.Address = task.redirectUrl;
                this._
            } else if (task.Type == TaskType.Script) {
                ChromiumWebBrowser cef = (ChromiumWebBrowser)sender;
                // 注入 jQuery
                // 等待 jQuery 注入成功后再执行任务脚本【因为任务脚本依赖jQuery】
                var respons = await cef.EvaluateScriptAsync(injection_jquery);
                if (respons.Success && respons.Result is IJavascriptCallback) {
                    respons = await((IJavascriptCallback)respons.Result).ExecuteAsync();
                    if (respons.Success) {
                        this.RunProcess(this._process);
                    }
                }
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
