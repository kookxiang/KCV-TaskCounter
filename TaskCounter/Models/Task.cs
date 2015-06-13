using Grabacr07.KanColleViewer.Composition;
using Grabacr07.KanColleWrapper;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using TaskCounter.ViewModels;

namespace TaskCounter.Models {
    
    public abstract class Task {
        public TaskViewModel BindedViewModel;

        public Task() {
            Counter = new int[1];
            MaxCount = new int[1];

            Initialize();

            try {
                Load();
            } catch { }

            BindedViewModel = new TaskViewModel(this);
            BindedViewModel.Name = Name;
            BindedViewModel.Description = Description;
            BindedViewModel.Precentage = Precentage;

            Save();

            FinishNotice();
        }

        public abstract void Initialize();

        /// <summary>
        /// 任务编号
        /// </summary>
        public int TaskID {
            get; protected set;
        }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name {
            get; protected set;
        }

        /// <summary>
        /// 任务详细介绍
        /// </summary>
        public string Description {
            get; protected set;
        }

        /// <summary>
        /// 任务周期
        /// </summary>
        public Cycle TaskCycle {
            get; protected set;
        }

        /// <summary>
        /// 任务需要用到的计数器
        /// </summary>
        public int[] Counter {
            get; protected set;
        }

        /// <summary>
        /// 计数器的最大值
        /// </summary>
        public int[] MaxCount {
            get; protected set;
        }

        /// <summary>
        /// 是否启用计数器
        /// </summary>
        public bool isAvailable {
            get; set;
        }

        /// <summary>
        /// 任务进度百分比
        /// </summary>
        public int Precentage {
            get {
                return (int)Math.Round((double)(Counter.Sum() * 100 / MaxCount.Sum()));
            }
        }

        /// <summary>
        /// 任务开始时间
        /// </summary>
        public DateTime StartTime {
            get; protected set;
        }

        /// <summary>
        /// 默认计数器自增 1
        /// </summary>
        public void Increase() {
            Increase(0);
        }

        /// <summary>
        /// 指定计数器自增 1
        /// </summary>
        /// <param name="CounterOffset">计数器编号</param>
        public void Increase(int CounterOffset) {
            Increase(CounterOffset, 1);
        }

        /// <summary>
        /// 指定计数器自增 n
        /// </summary>
        /// <param name="CounterOffset">计数器编号</param>
        /// <param name="Count">自增量</param>
        public void Increase(int CounterOffset, int Count) {
            if (!isAvailable)
                return;
            
            if (Counter[CounterOffset] >= MaxCount[CounterOffset])
                return;
            Counter[CounterOffset] += Count;
            Save();

            BindedViewModel.Precentage = Precentage;

            FinishNotice();
        }

        private string SavePath = "";

        /// <summary>
        /// 初始化文件保存路径
        /// </summary>
        private void InitializeSavePath() {
            if (SavePath != "")
                return;
            string SaveDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "grabacr.net",
                "KanColleViewer");
            SavePath = SaveDir + @"\TaskCounter_" + TaskID + ".xml";
            if (!Directory.Exists(SaveDir))
                Directory.CreateDirectory(SaveDir);
        }

        private XmlSerializer serializer = new XmlSerializer(typeof(TaskData));

        /// <summary>
        /// 保存计数器的值到硬盘
        /// </summary>
        public void Save() {
            InitializeSavePath();

            using (FileStream fileStream = new FileStream(SavePath, FileMode.Create)) {
                TaskData savedObject = new TaskData();
                savedObject.StartTime = StartTime;
                savedObject.counterValue = Counter.ToString(",");
                serializer.Serialize(fileStream, savedObject);
            }
        }

        /// <summary>
        /// 从硬盘读取计数器的值
        /// </summary>
        public void Load() {
            InitializeSavePath();

            if (!File.Exists(SavePath))
                return;

            using (FileStream fileStream = new FileStream(SavePath, FileMode.Open)) {
                TaskData savedObject = (TaskData)serializer.Deserialize(fileStream);
                string[] savedCounter = savedObject.counterValue.Split(',');
                Counter = new int[savedCounter.Length];
                for (int i=0; i<savedCounter.Length; i++) {
                    Counter[i] = int.Parse(savedCounter[i]);
                }
                StartTime = savedObject.StartTime;
            }

            CheckTime();
            TaskUpdateChecker.Abort();
            TaskUpdateChecker = new DelayedTask(CheckTime, TaskUpdateTime);
        }

        /// <summary>
        /// 任务周期
        /// </summary>
        public enum Cycle {
            Once = 0,
            Day = 1,
            Week = 2,
            Month = 3,
        }

        public DateTime TaskUpdateTime {
            get; protected set;
        }

        private DelayedTask TaskUpdateChecker = new DelayedTask();

        /// <summary>
        /// 检查任务刷新时间
        /// </summary>
        public void CheckTime() {
            // TODO: 非北京时间的处理
            TaskUpdateTime = DateTime.Now;
            if (TaskCycle == Cycle.Once)
                return;
            if (TaskCycle == Cycle.Day) {
                TaskUpdateTime = new DateTime(StartTime.Year, StartTime.Month, StartTime.Day, 4, 0, 0);
                if (StartTime.Hour >= 4)
                    TaskUpdateTime = TaskUpdateTime.AddDays(1);
            }
            if (TaskCycle == Cycle.Week) {
                TaskUpdateTime = new DateTime(StartTime.Year, StartTime.Month, StartTime.Day, 4, 0, 0);
                TaskUpdateTime = TaskUpdateTime.AddDays(7 - (int)(TaskUpdateTime.DayOfWeek) + 1);
            }
            if (TaskCycle == Cycle.Month) {
                TaskUpdateTime = new DateTime(StartTime.Year, StartTime.Month, 1, 4, 0, 0);
                TaskUpdateTime.AddMonths(1);
            }
            if (TaskUpdateTime.CompareTo(DateTime.Now) <= 0) {
                ResetCounter();
                StartTime = DateTime.Now;
                isAvailable = false;            // 任务过期，需要重新领取
                Save();
            }
            TaskUpdateChecker = new DelayedTask(CheckTime, TaskUpdateTime);
        }

        public void ResetCounter() {
            for (int i = 0; i < Counter.Length; i++) {
                Counter[i] = 0;
                StartTime = DateTime.Now;
                Save();
            }
        }

        public void checkAvailable(int[] misson) {
            isAvailable = misson.Contains(TaskID);
        }

        protected void FinishNotice() {
            if (!isAvailable || Counter.Sum() != MaxCount.Sum())
                return;

            PluginHost.Instance.GetNotifier().Show(
                NotifyType.Other,
                "任务完成 - " + Name,
                Description,
                () => Grabacr07.KanColleViewer.App.ViewModelRoot.Activate());
        }

        [Serializable()]
        public class TaskData {
            public DateTime StartTime;
            public string counterValue;
        }
    }
}
