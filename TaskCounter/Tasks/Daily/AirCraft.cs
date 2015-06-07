using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Daily {
    public class AirCraft : Task {
        // 航母 ID
        private readonly int[] Carriers = new int[] { 510, 512, 525, 528, 565, 579, 523, 560 };

        public override void Initialize() {
            MaxCount[0] = 3;

            TaskID = 211;
            Name = "敵空母を３隻撃沈せよ！";
            Description = "击沉敌方 3 艘航母";
            TaskCycle = Cycle.Day;

            // 挂钩子：战斗结束检查
            Hooks.OnEnemyShipSink += new Hooks.OnEnemyShipSinkHandler(onEnemyShipSink);
        }

        public void onEnemyShipSink(ShipData ship) {
            if (!Carriers.Contains(ship.Id))
                return;
            Increase();
        }
    }
}
