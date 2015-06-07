using Grabacr07.KanColleWrapper.Models.Raw;
using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Daily {
    public class NorthWest : Task {
        private int curMapArea = 0;
        private bool isBossCell = false;

        public override void Initialize() {
            MaxCount[0] = 5;

            TaskID = 226;
            Name = "南西諸島海域の制海権を握れ！";
            Description = "击破 [2]南西諸島海域 BOSS 5次";
            TaskCycle = Cycle.Day;

            Hooks.OnBattleFinish += new Hooks.OnBattleFinishHandler(onBattleFinish);
            Hooks.OnEnterMap += new Hooks.OnEnterMapHandler(OnEnterMap);
        }

        public void onBattleFinish(kcsapi_battleresult RawBattleResultData) {
            if (curMapArea == 2)
                if (isBossCell)
                    if (RawBattleResultData.api_win_rank != "C" & RawBattleResultData.api_win_rank != "D")
                        Increase();
        }

        public void OnEnterMap(int MapAera, int MapId, bool isBoss) {
            curMapArea = MapAera;
            isBossCell = isBoss;
        }
    }
}
