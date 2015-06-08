using Grabacr07.KanColleWrapper.Models.Raw;
using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Weekly {
    public class OperationA : Task {
        const int SORTIE = 0;
        const int BOSS = 1;
        const int BOSS_WIN = 2;
        const int S_WIN = 3;

        // 当前点是否为 Boss 点
        bool isBoss = false;

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

            // 进入战斗前记录当前是否为 Boss 点
            Hooks.OnEnterMap += new Hooks.OnEnterMapHandler((MapAera, MapId, isBoss) => this.isBoss = isBoss);
        }

        public void onBattleFinish(kcsapi_battleresult RawBattleResultData) {
            Increase(SORTIE);
            if (RawBattleResultData.api_win_rank == "S")
                Increase(S_WIN);
            if (isBoss) {
                Increase(BOSS);
                if (RawBattleResultData.api_win_rank != "C" & RawBattleResultData.api_win_rank != "D")
                    Increase(BOSS_WIN);
            }
        }
    }
}
