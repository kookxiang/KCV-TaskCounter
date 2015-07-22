using Grabacr07.KanColleWrapper.Models.Raw;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Weekly {
    class NorthwaySortie : Task {

        public override void Initialize() {
            MaxCount[0] = 5;

            TaskID = 241;
            Name = "敵北方艦隊主力を撃滅せよ！";
            Description = "一周内击破 3-3, 3-4 or 3-5 BOSS 5 次";
            TaskCycle = Cycle.Week;

            Hooks.OnBattleFinish += new Hooks.OnBattleFinishHandler(onBattleFinish);
        }

        public void onBattleFinish(int MapAera, int MapID, bool isBoss, string Rank) {
            if (MapAera == 3)
                if (MapID == 3 || MapID == 4 || MapID == 5)
                    if (isBoss)
                        if (Rank == "S" || Rank == "A" || Rank == "B")
                            Increase();
        }
    }
}
