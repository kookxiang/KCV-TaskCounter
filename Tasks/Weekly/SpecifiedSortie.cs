using Grabacr07.KanColleWrapper.Models.Raw;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Weekly {
    class SpecifiedSortie : Task {

        public override void Initialize() {
            MaxCount[0] = 3;

            TaskID = 261;
            Name = "海上輸送路の安全確保に努めよ！";
            Description = "与 1-5 BOSS 交战取得 3 次 A Rank 及以上";
            TaskCycle = Cycle.Week;

            Hooks.OnBattleFinish += new Hooks.OnBattleFinishHandler(onBattleFinish);
        }

        public void onBattleFinish(int MapAera, int MapID, bool isBoss, string Rank) {
            if (MapAera == 1)
                if (MapID == 5)
                    if (isBoss)
                        if (Rank == "S" || Rank == "A")
                            Increase();
        }
    }
}
