using System;
using System.Collections.Generic;
using System.Text;

namespace WPF_CEF_B2C_Tools {
    public class XingUtils {
    }

    public class Process {
        public Task currentTask;
        public List<Task> tasks;
    }

    public class Task {
        public string taskType;
        public string title;
    }
    public class BrowserTask :Task{
        public BrowserTask(string title) {
            this.taskType = "BrowserTask";
            this.title = title;
        }
        public string url { get; set; }
        public string script { get; set; }
    }
}
