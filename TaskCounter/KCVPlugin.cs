using Grabacr07.KanColleViewer.Composition;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models.Raw;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using TaskCounter.Models;
using TaskCounter.Models.Raw;
using TaskCounter.ViewModels;
using TaskCounter.Views;

namespace TaskCounter {
    [Export(typeof(IToolPlugin))]
    [ExportMetadata("Title", "KCV 任务计数器")]
    [ExportMetadata("Description", "计算任务完成进度")]
    [ExportMetadata("Version", "1.0.0")]
    [ExportMetadata("Author", "kookxiang")]
    public class KCVPlugin : IToolPlugin {
        internal static kcsapi_start2 RawStart2 {
            get; private set;
        }
        public BattleLogger battleLogger = new BattleLogger();
        public static List<Task> SupportedTasks = new List<Task>();
        public static PluginPanelViewModel viewModel = new PluginPanelViewModel();
        private int currentMapAera = 0;
        private int currentMapID = 0;
        private bool currentIsBoss = false;

        public KCVPlugin() {
            #region 挂Fiddler钩子
            KanColleClient.Current.Proxy.api_start2.TryParse<kcsapi_start2>().Subscribe(x => RawStart2 = x.Data);
            
            // 刷新任务列表
            KanColleClient.Current.Proxy.ApiSessionSource.Where(x => x.PathAndQuery == "/kcsapi/api_get_member/questlist").Subscribe(x => {
                new Thread(checkTaskAvailable).Start();
            });

            // 接任务
            //KanColleClient.Current.Proxy.ApiSessionSource.Where(x => x.PathAndQuery == "/kcsapi/api_req_quest/start").TryParse().Subscribe(x => onAcceptNewTask(int.Parse(x.Request.Get("id"))));

            // 补给
            KanColleClient.Current.Proxy.api_req_hokyu_charge.TryParse().Where(x => x.IsSuccess).Subscribe(x => {
                if (Hooks.OnSupply != null)
                    Hooks.OnSupply();
            });

            // 远征
            KanColleClient.Current.Proxy.api_req_mission_result.TryParse<kcsapi_mission_result>().Where(x => x.Data.api_clear_result > 0).Subscribe(x => {
                if (Hooks.OnExpeditionSuccess != null)
                    Hooks.OnExpeditionSuccess();
            });

            // 废弃装备
            KanColleClient.Current.Proxy.api_req_kousyou_destroyitem2.TryParse().Where(x => x.IsSuccess).Subscribe(x => {
                if (Hooks.OnDestroyItem != null)
                    Hooks.OnDestroyItem();
            });

            // 近代化改修
            KanColleClient.Current.Proxy.api_req_kaisou_powerup.TryParse<kaisou_powerup>().Where(x => x.Data.api_powerup_flag == 1).Subscribe(x => {
                if (Hooks.OnPowerUp != null)
                    Hooks.OnPowerUp();
            });

            // 入渠
            KanColleClient.Current.Proxy.api_req_nyukyo_start.TryParse().Where(x => x.IsSuccess).Subscribe(x => {
                if (Hooks.OnRepair != null)
                    Hooks.OnRepair();
            });

            // 地图切换
            Hooks.OnEnterMap += new Hooks.OnEnterMapHandler((mapArea, mapId, isBoss) => {
                currentMapAera = mapArea;
                currentMapID = mapId;
                currentIsBoss = isBoss;
            });

            // 战斗结算
            KanColleClient.Current.Proxy.api_req_sortie_battleresult.TryParse<kcsapi_battleresult>().Subscribe(x => {
                if (Hooks.OnBattleFinish != null)
                    Hooks.OnBattleFinish(currentMapAera, currentMapID, currentIsBoss, x.Data.api_win_rank);
            });

            // 船只建造
            KanColleClient.Current.Proxy.api_req_kousyou_createship.TryParse().Subscribe(x => {
                if (Hooks.OnBuildShip != null)
                    Hooks.OnBuildShip();
            });

            // 装备开发
            KanColleClient.Current.Proxy.api_req_kousyou_createitem.TryParse().Subscribe(x => {
                if (Hooks.OnCreateItem != null)
                    Hooks.OnCreateItem();
            });

            // 拆船
            KanColleClient.Current.Proxy.api_req_kousyou_destroyship.TryParse().Subscribe(x => {
                if (Hooks.OnDestoryShip != null)
                    Hooks.OnDestoryShip();
            });

            // 演习
            KanColleClient.Current.Proxy.ApiSessionSource.Where(x => x.PathAndQuery == "/kcsapi/api_req_practice/battle_result").TryParse<practice_result>().Subscribe(x => {
                if (Hooks.OnPractice != null)
                    Hooks.OnPractice(x.Data.api_win_rank);
            });
            #endregion

            // 每日任务
            List<Type> dailyTask = Assembly.GetExecutingAssembly().GetTypes().ToList().Where(t => t.Namespace == "TaskCounter.Tasks.Daily").ToList();
            foreach (Type taskName in dailyTask) {
                SupportedTasks.Add((Task)Activator.CreateInstance(taskName));
            }

            // 每周任务
            List<Type> weeklyTask = Assembly.GetExecutingAssembly().GetTypes().ToList().Where(t => t.Namespace == "TaskCounter.Tasks.Weekly").ToList();
            foreach (Type taskName in weeklyTask) {
                SupportedTasks.Add((Task)Activator.CreateInstance(taskName));
            }
        }

        private static void onAcceptNewTask(int newTaskID) {
            Thread.Sleep(100);
            SupportedTasks.Where(x => x.TaskID == newTaskID).ToList().ForEach(task => {
                task.isAvailable = true;
                task.CheckTime();
            });
        }

        private static void checkTaskAvailable() {
            Thread.Sleep(100);
            int[] AcceptedMission = null, AvailableMission = null;
            try {
                AcceptedMission = KanColleClient.Current.Homeport.Quests.Current.Where(i => i != null).Select(i => i.Id).ToArray();
                AvailableMission = KanColleClient.Current.Homeport.Quests.All.Where(i => i != null).Select(i => i.Id).Where(x => !AcceptedMission.Contains(x)).ToArray();
            } catch { }
            if (AvailableMission == null || AvailableMission.Length == 0)
                return;
            SupportedTasks.ForEach(task => {
                if (task != null)
                    task.checkAvailable(AcceptedMission);
                if(task.isAvailable)
                    task.CheckTime();
            });
            viewModel.AcceptedList = SupportedTasks.Where(x => x.isAvailable).Select(x => x.BindedViewModel).ToList();
            viewModel.AvailableList = new List<TaskViewModel>();
            AvailableMission.ToList().ForEach(taskId => {
                if (SupportedTasks.Where(x => !x.isAvailable && x.TaskID == taskId).Select(x => x.BindedViewModel).Count() == 0)
                    return;
                viewModel.AvailableList.Add(SupportedTasks.Where(x => !x.isAvailable && x.TaskID == taskId).Select(x => x.BindedViewModel).First());
            });
        }

        public string ToolName {
            get {
                return "任务计数";
            }
        }

        public object GetSettingsView() {
            return null;
        }

        public object GetToolView() {
            return new PluginPanel() { DataContext = viewModel };
        }
    }
}
