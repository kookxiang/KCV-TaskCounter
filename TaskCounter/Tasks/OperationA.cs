using Grabacr07.KanColleWrapper.Models.Raw;
using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks {
    public class OperationA : Task {
        const int SORTIE = 0;
        const int BOSS = 1;
        const int BOSS_WIN = 2;
        const int S_WIN = 3;

        // Boss 点名称
        private readonly string[] Bosses =
        {
            "敵主力艦隊", "敵主力部隊", "敵機動部隊", "敵通商破壊主力艦隊",
            "敵通商破壊艦隊", "敵主力打撃群", "敵侵攻中核艦隊",
            "敵北方侵攻艦隊", "敵キス島包囲艦隊", "深海棲艦泊地艦隊", "深海棲艦北方艦隊中枢", "北方増援部隊主力",
            "東方派遣艦隊", "東方主力艦隊", "敵東方中枢艦隊",
            "敵前線司令艦隊", "敵機動部隊本隊", "敵サーモン方面主力艦隊", "敵補給部隊本体", "敵任務部隊本隊",
            "敵回航中空母", "敵攻略部隊本体"
        };

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

            // 挂钩子：战斗结束检查
            Hooks.OnBattleFinish += new Hooks.OnBattleFinishHandler(onBattleFinish);
        }

        public void onBattleFinish(kcsapi_battleresult RawBattleResultData) {
            Increase(SORTIE);
            if (RawBattleResultData.api_win_rank == "S")
                Increase(S_WIN);
            if (Bosses.Contains(RawBattleResultData.api_enemy_info.api_deck_name)) {
                Increase(BOSS);
                if (RawBattleResultData.api_win_rank != "C" & RawBattleResultData.api_win_rank != "D")
                    Increase(BOSS_WIN);
            }
        }
    }
}
