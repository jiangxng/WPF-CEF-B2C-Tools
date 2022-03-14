using System.Windows.Controls;
using CefSharp;
using CefSharp.Wpf;
using System.IO;
using System;
using System.Windows;
using System.Text.RegularExpressions;

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
        string nowUrl;
        string injection_jquery = @"
        (function () {
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

            getScript('https://code.jquery.com/jquery-latest.min.js', function () {
                if (typeof jQuery == '') {
                    console.log('Sorry, but jQuery wasn\'t able to load');
                } else {
                    console.log('This page is now jQuerified with v' + $.fn.jquery);
                    $(document).ready(function () {
                        //here you can write your jquery code
                    });
                }
            });
        });";

        public XingWebBrowser() {

            CefSettings settings = new CefSettings();
            settings.CachePath = Directory.GetCurrentDirectory() + @"\cef_cache";
            settings.MultiThreadedMessageLoop = true;
            // 开启跨域访问
            settings.CefCommandLineArgs.Add("proxy-auto-detect", "0");
            // 禁用安全检查
            settings.CefCommandLineArgs.Add("--disable-web-security", "");
            if (!Cef.IsInitialized) {
                Cef.Initialize(settings);
            }
            InitializeComponent();
        }

        private void Cef_AddressChanged(object sender, DependencyPropertyChangedEventArgs e) {
            this.nowUrl = txtAddrees.Text = e.NewValue.ToString();
        }
        private void Cef_FrameLoadEnd(object sender, FrameLoadEndEventArgs e) {
            //cef.ExecuteScriptAsync(injection_jquery);
            //每次浏览器完成加载后执行【脚本流程】的【当前任务】
            //BrowserTask task = (BrowserTask)this._process.tasks[this._process.currentTask];
            //cef.ExecuteScriptAsync(task.runOnLoad);
            if (this._task != null) {
                if (this._task.Type == TaskType.Fork) {
                    Regex reg = new Regex(this._task.keepUrl);
                    if (!reg.IsMatch(this.nowUrl)) {
                        this._task.Status = TaskStatus.Closed;
                    }
                } else {
                    this._task.Status = TaskStatus.Closed;
                }
            }
            this.log("=====================FrameLoadEnd=======================");
        }

        public void log(string msg) {
            this.logPanel.Dispatcher.Invoke(() => {
                this.logPanel.Text = DateTime.Now.ToLocalTime() + "\t" + msg + "\n" + this.logPanel.Text;
            });
        }

        /// <summary>
        /// 运行【任务流程】浏览器的入库
        /// </summary>
        /// <param name="process"></param>
        public async void RunTask(BrowserTask task) {
            this._task = task;
            this.log(this._task.Title);
            if (task.Type == TaskType.Redirect) {
                this.browser.Dispatcher.Invoke(() => {
                    this.browser.Address = task.redirectUrl;
                    this._task.Status = TaskStatus.Executing;
                });
            } else if (task.Type == TaskType.Script) {
                // 注入 jQuery
                // 等待 jQuery 注入成功后再执行任务脚本【因为任务脚本依赖jQuery】
                var respons = await this.browser.EvaluateScriptAsync(injection_jquery);
                if (respons.Success && respons.Result is IJavascriptCallback) {
                    respons = await ((IJavascriptCallback)respons.Result).ExecuteAsync();
                    if (respons.Success) {
                        this.browser.ExecuteScriptAsync(task.script);
                        this._task.Status = TaskStatus.Executing;
                    }
                }
            }
        }

        public void Open(string url) {
            this.browser.Address = url;
        }

        private void UserControl_Initialized(object sender, EventArgs e) {
            browser.RequestHandler = new XingBasicRequestHandler(this);
            browser.FrameLoadEnd += Cef_FrameLoadEnd;
            browser.AddressChanged += Cef_AddressChanged;
            browser.KeyboardHandler = new CEFKeyBoardHander();
            browser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            browser.JavascriptObjectRepository.Register("Xing", new JsExpend(this));
        }
    }

    public class JsExpend {
        XingWebBrowser webBrowser;
        public JsExpend(XingWebBrowser browser) {
            this.webBrowser = browser;
        }
        public void Log(string msg) {
            this.webBrowser.log(msg);
        }
    }

    class XingRequestHandler : CefSharp.Handler.ResourceRequestHandler {
        XingWebBrowser xingWebBrowser;
        public XingRequestHandler(XingWebBrowser xingWebBrowser) {
            this.xingWebBrowser = xingWebBrowser;
        }

        private readonly System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
        protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response) {
            return new CefSharp.ResponseFilter.StreamResponseFilter(memoryStream);
        }
        
        //拦截请求，自定义处理程序
        // Ajax 请求
        protected override void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength) {
            //You can now get the data from the stream
            var bytes = memoryStream.ToArray();

            if (response.Charset == "utf-8") {
                var str = System.Text.Encoding.UTF8.GetString(bytes);
                this.xingWebBrowser.log("In OnResourceLoadComplete : " + str.Substring(0, 10) + " <...>");
            } else if (response.Charset == "gbk") {
                var str = System.Text.Encoding.GetEncoding("GB2312").GetString(bytes);
                this.xingWebBrowser.log("In OnResourceLoadComplete : " + str.Substring(0, 10) + " <...>");
            } else {
                //Deal with different encoding here
            }
        }
    }
    public class XingBasicRequestHandler : CefSharp.Handler.RequestHandler {
        XingWebBrowser xingWebBrowser;
        public XingBasicRequestHandler(XingWebBrowser webBrowser) {
            this.xingWebBrowser = webBrowser;
        }

        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling) {
            //Only intercept specific Url's
            //https://trade.taobao.com/trade/itemlist/list_sold_items.htm?userSwitch=1
            //https://trade.taobao.com/trade/itemlist/asyncSold.htm?event_submit_do_query=1&_input_charset=utf8&sifg=0
            // 不同的请求地址，用不同的处理程序
            if (request.Url.Contains("/asyncSold.htm")) {
                return new XingRequestHandler(this.xingWebBrowser);
            }
            //Default behaviour, url            will be loaded normally.
            return null;
        }

    }

    //监听键盘事件
    class CEFKeyBoardHander : IKeyboardHandler {
        public bool OnKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey) {
            if (type == KeyType.RawKeyDown) {
                switch (windowsKeyCode) {
                    //F12
                    case 123:
                        //打开调试工具
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
