# WPF-CEF-B2C-Tools
基于 WPF .net Core Cefsharp 开发的电商助手工具 
关键字 RPA WPF .net Core CefSharp 开源 
# 主要模块
### 订单（可筛选，可编辑，扩展信息，忽略），
    物流
### 商品，
    刊登
### 报表，
### 供应商，
### 电商平台，
### 账号

第一天：产品技术选项 WPF .net Core Cefsharp
第二天：计划优先级 -- taobao 订单采集
    问题：没办法在同一个进程里面不同的cef浏览器用不同的CachePath
    Cef.Initialize并且Cef.Shutdown每个进程（应用程序）只能调用一次Initialize，因此仅使用完后CefSharp后才调用Shutdown。
    Cef.Initialize并且Cef.Shutdown必须在同一线程上调用。
    Cef.Initialize如果您创建新ChromiumWebBrowser实例并且尚未调用，则会为您隐式调用Cef.Initialize。
    从磁盘/数据库/嵌入式资源/流中加载HTML / CSS / JavaScript / etc
    CefSharp.WebBrowserExtensions类中提供了一些扩展方法，以方便使用。
第三天：开始编写requesthandler,
      结果发现一个前置任务需要先实现自动登陆。So，开始写登陆功能。
      通过 EvaluateScriptAsync 函数实现了 Cefsharp 注入Js脚本实现了登陆功能。为了方便调试，顺便实现了F12弹出调试窗口
      验证了登陆功能后，开始进行代整理，分为一下部分
      MainWindow，程序的Ui主框架用于呈现内容
      WebBrowserContrl,分装的浏览器负荷组件，用来执行脚本流程与任务
      Process,组织任务，维护任务执行状态
      Task,任务的描述
      Factory,定义流程
      JsLibrary，维护任务的脚本素材