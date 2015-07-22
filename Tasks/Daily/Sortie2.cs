using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Daily {
    public class Sortie2 : Task {
        public override void Initialize() {
            MaxCount[0] = 1;

            TaskID = 216;
            Name = "敵艦隊主力を撃滅せよ！";
            Description = "击败敌军的旗舰船";
            TaskCycle = Cycle.Day;

            Hooks.OnBattleFinish += new Hooks.OnBattleFinishHandler(onBattleFinish);
        }

        public void onBattleFinish(int MapAera, int MapID, bool isBoss, string Rank) {
            if (Rank != "C" && Rank != "D")
                Increase();
        }
    }
}
