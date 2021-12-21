
namespace WPF_CEF_B2C_Tools.BLOs.Taobao {
    public static class OrderUtil {
        public static Process GetOrder {
            get {
                Process value = new Process();
                BrowserTask task = new BrowserTask("登陆");
                task.url = "https://trade.taobao.com/trade/itemlist/list_sold_items.htm";
                task.script = JsSnippet.Login;
                value.tasks.Add(task);
                return value;
            }
        }
    }

}
