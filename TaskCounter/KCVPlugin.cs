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

        public KCVPlugin() {
            #region 挂Fiddler钩子
            KanColleClient.Current.Proxy.api_start2.TryParse<kcsapi_start2>().Subscribe(x => RawStart2 = x.Data);

            // 刷新任务列表
            KanColleClient.Current.Proxy.ApiSessionSource.Where(x => x.PathAndQuery == "/kcsapi/api_get_member/questlist").Subscribe(x => Hooks.OnQuestListChange());

            // 补给
            KanColleClient.Current.Proxy.api_req_hokyu_charge.TryParse().Where(x => x.IsSuccess).Subscribe(x => Hooks.OnSupply());

            // 远征
            KanColleClient.Current.Proxy.api_req_mission_result.TryParse<kcsapi_mission_result>()
                .Where(x => x.IsSuccess).Where(x => x.Data.api_clear_result == 1).Subscribe(x => Hooks.OnExpeditionSuccess());

            // 废弃装备
            KanColleClient.Current.Proxy.api_req_kousyou_destroyitem2.TryParse().Where(x => x.IsSuccess).Subscribe(x => Hooks.OnDestroyItem());

            // 近代化改修
            KanColleClient.Current.Proxy.api_req_kaisou_powerup.TryParse().Where(x => x.IsSuccess).Subscribe(x => Hooks.OnPowerUp());

            // 入渠
            KanColleClient.Current.Proxy.api_req_nyukyo_start.TryParse().Where(x => x.IsSuccess).Subscribe(x => Hooks.OnRepair());

            // 战斗结算
            KanColleClient.Current.Proxy.api_req_sortie_battleresult.TryParse<kcsapi_battleresult>().Subscribe(x => Hooks.OnBattleFinish(x.Data));

            // 船只建造
            KanColleClient.Current.Proxy.api_req_kousyou_createship.TryParse().Subscribe(x => Hooks.OnBuildShip());

            // 装备开发
            KanColleClient.Current.Proxy.api_req_kousyou_createitem.TryParse().Where(x => x.IsSuccess).Subscribe(x => Hooks.OnCreateItem());

            // 拆船
            KanColleClient.Current.Proxy.api_req_kousyou_destroyship.TryParse().Subscribe(x => Hooks.OnDestoryShip());

            // 检查任务可用状态
            Hooks.OnQuestListChange += new Hooks.OnQuestListChangeHandler(delayCheckAvailable);
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

        private static void delayCheckAvailable() {
            new Thread(checkAvailable).Start();
        }

        private static void checkAvailable() {
            Thread.Sleep(100);
            int[] misson = null;
            try {
                misson = KanColleClient.Current.Homeport.Quests.Current.Where(i => i != null).Select(i => i.Id).ToArray();
            } catch { }
            if (misson == null || misson.Length == 0)
                return;
            SupportedTasks.ForEach(task => {
                if (task != null)
                    task.checkAvailable(misson);
            });
            viewModel.List = SupportedTasks.Where(x => x.isAvailable).Select(x => x.BindedViewModel).ToList();
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
