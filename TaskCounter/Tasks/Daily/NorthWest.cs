using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Daily {
    public class NorthWest : Task {

        public override void Initialize() {
            MaxCount[0] = 5;

            TaskID = 226;
            Name = "南西諸島海域の制海権を握れ！";
            Description = "击破 [2]南西諸島海域 BOSS 5次";
            TaskCycle = Cycle.Day;

            Hooks.OnBattleFinish += new Hooks.OnBattleFinishHandler(onBattleFinish);
        }

        public void onBattleFinish(int MapAera, int MapID, bool isBoss, string Rank) {
            if (MapAera == 2)
                if (isBoss)
                    if (Rank != "C" & Rank != "D")
                        Increase();
        }
    }
}
