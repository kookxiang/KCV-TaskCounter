using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Daily {
    public class Sortie3 : Task {
        public override void Initialize() {
            MaxCount[0] = 10;

            TaskID = 210;
            Name = "敵艦隊を10回邀撃せよ！";
            Description = "每日出击 10 次胜利";
            TaskCycle = Cycle.Day;

            Hooks.OnBattleFinish += new Hooks.OnBattleFinishHandler(onBattleFinish);
        }

        public void onBattleFinish(int MapAera, int MapID, bool isBoss, string Rank) {
            if (Rank != "C" && Rank != "D")
                Increase();
        }
    }
}
