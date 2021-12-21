using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace WPF_CEF_B2C_Tools.BLOs.Taobao {
    public static class JsSnippet {
        // 自行修改下面的代码
        public static string Login = String.Format(@"
            document.getElementById('fm-login-id').value='{0}'
            document.getElementById('fm-login-password').value= '{1}'
            document.getElementsByClassName('fm-button fm-submit password-login')[0].click()
        ", ConfigurationManager.AppSettings["uid"], ConfigurationManager.AppSettings["pwd"]);
    }
}
