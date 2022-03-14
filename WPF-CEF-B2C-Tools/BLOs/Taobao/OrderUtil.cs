
using System;
using System.Collections;

namespace WPF_CEF_B2C_Tools.BLOs.Taobao {
    public class OrderUtil  {
        // 获取淘宝买家的订单，
        // 返回一个处理流程对象
        public static Process FetchOrderData {
            get {
                Process process = new Process();

                BrowserTask task = new BrowserTask("跳转到订单界面", TaskType.Redirect);
                task.redirectUrl = "https://trade.taobao.com/trade/itemlist/list_sold_items.htm";
                process.addTask(task);

                task = new BrowserTask("点击按钮【显示用户信息】", TaskType.Script);
                task.script = JsSnippet.clickSwitchBtn;
                process.addTask(task);

                task = new BrowserTask("获取销售订单数据", TaskType.Script);
                task.script = "";
                process.addTask(task);

                return process;
            }
        }
        public static Process Login {
            get {
                Process process = new Process();

                BrowserTask task = new BrowserTask("打开登陆页面", TaskType.Redirect);
                task.redirectUrl = "https://login.taobao.com/member/login.jhtml";
                process.addTask(task);

                task = new BrowserTask("输入用户名密码，执行登陆", TaskType.Script);
                task.script = JsSnippet.Login;
                process.addTask(task);

                return process;
            }
        }
    }
}
