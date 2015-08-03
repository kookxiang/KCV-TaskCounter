using Grabacr07.KanColleWrapper.Models.Raw;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Weekly {
    class SouthwayExpedition : Task {

        public override void Initialize() {
            MaxCount[0] = 1;

            TaskID = 410;
            Name = "南方への輸送作戦を成功させよ！";
            Description = "完成一次远征「東京急行」或「東京急行(弐)」";
            TaskCycle = Cycle.Week;

            Hooks.OnExpeditionSuccessRaw += new Hooks.OnExpeditionSuccessRawHandler(OnExpeditionSuccessRaw);
        }

        public void OnExpeditionSuccessRaw(kcsapi_mission_result Data) {
            if (Data.api_quest_name.Contains("東京"))
                Increase();
        }
    }
}
