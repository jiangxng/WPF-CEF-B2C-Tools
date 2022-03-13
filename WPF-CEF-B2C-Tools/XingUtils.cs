using System;
using System.Collections.Generic;
using System.Text;
using WPF_CEF_B2C_Tools.Components;

namespace WPF_CEF_B2C_Tools {
    public class XingUtils {
    }
    static class ProcessEngine {
        // 开始运行一个流程
        public static void RunProcess(Process process, XingWebBrowser browser) {
            ProcessInstance inc = instantiateProc(process, browser);
            BrowserTask task = (BrowserTask)inc.getNextTask();
            browser.RunTask(task);
        }
        // 继续运行一个流程实例
        public static void RunProcess(ProcessInstance instance) {
            BrowserTask task = (BrowserTask)instance.getNextTask();
            instance.webBrowser.RunTask(task);
        }
        // 实例化一个流程
        private static ProcessInstance instantiateProc(Process process, XingWebBrowser browser) {
            ProcessInstance inc = (ProcessInstance)process;
            inc.id = Guid.NewGuid().ToString();
            inc.webBrowser = browser;
            inc.instantiateTasks();
            return inc;
        }
    }
    // 自动化脚本流程
    public class Process {
        private int currentTaskIndex = -1;
        protected List<Task> tasks;
        public string id;

        public Process() {
            this.tasks = new List<Task>();
        }

        public void addTask(Task task) {
            this.tasks.Add(task);
        }

        public Task getNextTask() {
            this.currentTaskIndex += 1;
            if (currentTaskIndex < this.tasks.Count) {
                return this.tasks[currentTaskIndex];
            } else {
                return null;
            }
        }
        public Task current() {
            if (currentTaskIndex < this.tasks.Count) {
                return this.tasks[currentTaskIndex];
            } else {
                return null;
            }
        }
    }

    public class ProcessInstance : Process {
        public XingWebBrowser webBrowser;
        public void instantiateTasks() {
            this.tasks.ForEach(it => {
                it.OwnerProcess = this;
            });
        }
    }

    public class Task {
        public TaskType Type;
        public string Title;
        private TaskStatus status;
        public TaskStatus Status {
            get { return status; }
            set {
                this.status = value;
                if (value == TaskStatus.Closed) {
                    ProcessEngine.RunProcess(OwnerProcess);
                }
            }
        }
        public ProcessInstance OwnerProcess;
    }

    public class BrowserTask : Task {
        public BrowserTask(string title) {
            this.Title = title;
        }
        public BrowserTask(string title, TaskType type) {
            this.Title = title;
            this.Type = type;
        }
        public string redirectUrl { get; set; }
        public string script { get; set; }
        public string runOnLoad { get; set; }
        public string runOnResponse { get; set; }
    }

    public enum TaskType {
        Redirect,
        Script,
        Fork
    }

    public enum TaskStatus {
        Canceling,
        Closed,
        Compensating,
        Executing,
        Faulting
    }
}
