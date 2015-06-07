using Grabacr07.KanColleWrapper.Models.Raw;
using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Daily {
    public class Sortie : Task {
        public override void Initialize() {
            MaxCount[0] = 10;

            TaskID = 210;
            Name = "敵艦隊を10回邀撃せよ！";
            Description = "每日出击 10 次胜利";
            TaskCycle = Cycle.Day;

            Hooks.OnBattleFinish += new Hooks.OnBattleFinishHandler(onBattleFinish);
        }

        public void onBattleFinish(kcsapi_battleresult RawBattleResultData) {
            if (RawBattleResultData.api_win_rank != "C" & RawBattleResultData.api_win_rank != "D")
                Increase();
        }
    }
}
