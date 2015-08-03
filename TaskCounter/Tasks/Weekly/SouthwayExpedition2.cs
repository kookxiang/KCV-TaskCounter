using Grabacr07.KanColleWrapper.Models.Raw;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Weekly {
    class SouthwayExpedition2 : Task {

        public override void Initialize() {
            MaxCount[0] = 7;

            TaskID = 411;
            Name = "南方への鼠輸送を継続実施せよ！";
            Description = "一周内完成七次远征「東京急行」或「東京急行(弐)」";
            TaskCycle = Cycle.Week;

            Hooks.OnExpeditionSuccessRaw += new Hooks.OnExpeditionSuccessRawHandler(OnExpeditionSuccessRaw);
        }

        public void OnExpeditionSuccessRaw(kcsapi_mission_result Data) {
            if (Data.api_quest_name.Contains("東京"))
                Increase();
        }
    }
}
