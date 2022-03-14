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
            BrowserTask task = (BrowserTask)inc.nextTask();
            if (task != null)
                browser.RunTask(task);
        }
        // 继续运行一个流程实例
        public static void RunProcessNext(ProcessInstance instance) {
            BrowserTask task = (BrowserTask)instance.nextTask();
            if (task != null)
                instance.webBrowser.RunTask(task);
        }
        // 实例化一个流程
        private static ProcessInstance instantiateProc(Process process, XingWebBrowser browser) {
            ProcessInstance inc = new ProcessInstance(process);
            inc.id = Guid.NewGuid().ToString();
            inc.webBrowser = browser;
            inc.instantiateTasks();
            return inc;
        }
    }
    // 自动化脚本流程
    public class Process {
        internal List<Task> tasks;
        public Process() {
            this.tasks = new List<Task>();
        }
        public void addTask(Task task) {
            this.tasks.Add(task);
        }
    }

    public class ProcessInstance : Process {
        protected int currentTaskIndex = -1;
        public string id;
        public XingWebBrowser webBrowser;
        public ProcessInstance(Process process) {
            this.tasks = process.tasks;
        }

        public void instantiateTasks() {
            this.tasks.ForEach(it => {
                it.OwnerProcess = this;
            });
        }
        /// <summary>
        /// 将指针指向下一个任务
        /// </summary>
        /// <returns></returns>
        public Task nextTask() {
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

    public class Task {
        public TaskType Type;
        public string Title;
        protected TaskStatus status;
        public TaskStatus Status {
            get { return status; }
            set {
                this.status = value;
                if (value == TaskStatus.Closed) {
                    ProcessEngine.RunProcessNext(OwnerProcess);
                }
            }
        }
        public ProcessInstance OwnerProcess;
        public Task nextSibling() {
            throw new NotImplementedException("待实现");
        }
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
        public string keepUrl { get; set; }
    }

    public enum TaskType {
        Redirect,
        Script,
        Fork,
        ScriptRedirect
    }

    public enum TaskStatus {
        Canceling,
        Closed,
        Compensating,
        Executing,
        Faulting
    }
}
