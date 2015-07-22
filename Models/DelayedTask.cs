using System;
using System.Threading;

namespace TaskCounter.Models {
    public class DelayedTask {
        private DateTime RunTime;
        private ThreadStart work;
        private Thread RunThread;

        public DelayedTask() {
        }

        public DelayedTask(ThreadStart work, DateTime RunTime) {
            if (RunTime.CompareTo(DateTime.Now) <= 0) {
                work();
                return;
            }
            this.RunTime = RunTime;
            this.work = work;
            RunThread = new Thread(Action);
            RunThread.IsBackground = true;
            RunThread.Start();
        }

        private void Action() {
            Thread.Sleep((RunTime - DateTime.Now));
            work();
        }

        public void Abort() {
            if (RunThread != null)
                RunThread.Abort();
        }
    }
}
