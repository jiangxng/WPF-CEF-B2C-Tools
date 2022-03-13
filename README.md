作者WX:x76784911 
# WPF Cefsharp B2C Tools 电商助手
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
第四天：最近工作比较多，隔了好长时间没有更新
    今天拼多多的订单突然多了起来，拼多多官方提供的后台很难用，如果要看用户的地址详情，每个单需要点两次，即使只有10单，也需要点击20次，已经无法通过人工维护，
    所以决定需要继续完善这个订单工具，实现拼多多的订单管理，实现批量点击查看用户地址详情的功能
    流程，
    0.登陆
    1.采购
    查看订单地址详情
    自动采购
    备注订单状态
    完成
    

    方式1. ExecuteScriptAsync 方法使用方式与 js 的 eval方法一样，异步执行，无返回值。
// xxx为js的方法名称
wb.ExecuteScriptAsync("xxx()"); 
// 为 js 的 变量jsVar赋值 'abc'
wb.ExecuteScriptAsync("jsVar='abc'"); 

方式2. EvaluateScriptAsync 方法使用方式与 js 的 eval方法一样，异步执行，有返回值。

https://blog.csdn.net/gong_hui2000/article/details/48155547