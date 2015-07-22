using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Daily {
    public class Sortie : Task {
        public override void Initialize() {
            MaxCount[0] = 1;

            TaskID = 201;
            Name = "敵艦隊を撃破せよ！";
            Description = "出击胜利一次";
            TaskCycle = Cycle.Day;

            Hooks.OnBattleFinish += new Hooks.OnBattleFinishHandler(onBattleFinish);
        }

        public void onBattleFinish(int MapAera, int MapID, bool isBoss, string Rank) {
            if (Rank != "C" && Rank != "D")
                Increase();
        }
    }
}
