
namespace WPF_CEF_B2C_Tools.BLOs.Pinduoduo {
    public static class OrderUtil {
        public static Process GetOrder {
            get {
                Process value = new Process();

                BrowserTask task1 = new BrowserTask("登陆");
                task1.redirectUrl = "https://mms.pinduoduo.com/login/";
                value.addTask(task1);

                BrowserTask task2 = new BrowserTask("跳转");
                task2.redirectUrl = "https://mms.pinduoduo.com/orders/list?type=0";
                value.addTask(task2);

                BrowserTask task3 = new BrowserTask("点击按钮【显示用户信息】");
                task3.script = JsSnippet.clickSwitchBtn;
                value.addTask(task3);

                BrowserTask task4 = new BrowserTask("获取销售订单数据");
                task4.script = "";
                value.addTask(task4);

                return value;
            }
        }
    }

}
