using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WPF_CEF_B2C_Tools.BLOs.Test {
    public class Class1 {
        public static Process OpenBaidu {
            get {
                Process process = new Process();

                BrowserTask task = new BrowserTask("打开百度", TaskType.Redirect);
                task.redirectUrl = "http://www,baidu.com";
                process.addTask(task);


                return process;
            }
        }

        public static Process OpenTestPage {
            get {
                Process process = new Process();

                BrowserTask task = new BrowserTask("打开测试页", TaskType.Redirect);
                task.redirectUrl = "D:/Documents/WPF-CEF-B2C-Tools/WPF-CEF-B2C-Tools/blank.html";
                process.addTask(task);

                return process;
            }
        }
        
        public static Process execInjoinScript {
            get {
                Process process = new Process();

                BrowserTask task = new BrowserTask("执行注入后的脚本", TaskType.Script);
                task.script = File.ReadAllText(Path.GetFullPath("BLOs/Test/test.js"));
                process.addTask(task);

                return process;
            }
        }
    }
}
