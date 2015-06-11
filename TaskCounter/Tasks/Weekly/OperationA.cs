using Grabacr07.KanColleWrapper.Models.Raw;
using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Weekly {
    public class OperationA : Task {
        const int SORTIE = 0;
        const int BOSS = 1;
        const int BOSS_WIN = 2;
        const int S_WIN = 3;

        public override void Initialize() {
            Counter = new int[4];

            TaskID = 214;
            Name = "あ号作戦";
            Description = "一周内出击 36 次、进入 BOSS 点 24 次，BOSS 战胜利 12 次，S 胜 6 次";
            TaskCycle = Cycle.Week;

            MaxCount = new int[4];
            MaxCount[SORTIE] = 36;
            MaxCount[BOSS] = 24;
            MaxCount[BOSS_WIN] = 12;
            MaxCount[S_WIN] = 6;

            // 战斗结束检查战斗结果
            Hooks.OnBattleFinish += new Hooks.OnBattleFinishHandler(onBattleFinish);
        }

        public void onBattleFinish(int MapAera, int MapID, bool isBoss, string Rank) {
            Increase(SORTIE);
            if (Rank == "S")
                Increase(S_WIN);
            if (isBoss) {
                Increase(BOSS);
                if (Rank != "C" && Rank != "D")
                    Increase(BOSS_WIN);
            }
        }
    }
}
