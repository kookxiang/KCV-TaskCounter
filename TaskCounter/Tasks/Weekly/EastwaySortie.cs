using Grabacr07.KanColleWrapper.Models.Raw;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Weekly {
    class EastwaySortie : Task {

        public override void Initialize() {
            MaxCount[0] = 12;

            TaskID = 229;
            Name = "敵東方艦隊を撃滅せよ！";
            Description = "一周内击破 4 图 BOSS 12 次";
            TaskCycle = Cycle.Week;

            Hooks.OnBattleFinish += new Hooks.OnBattleFinishHandler(onBattleFinish);
        }

        public void onBattleFinish(int MapAera, int MapID, bool isBoss, string Rank) {
            if (MapAera == 4)
                if (isBoss)
                    if (Rank == "S" || Rank == "A" || Rank == "B")
                        Increase();
        }
    }
}
