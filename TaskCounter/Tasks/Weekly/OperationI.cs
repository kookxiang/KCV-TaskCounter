using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Weekly {
    public class OperationI : Task {
        // 航母 ID
        private readonly int[] Carriers = new int[] { 510, 512, 525, 528, 565, 579, 523, 560 };

        public override void Initialize() {
            MaxCount[0] = 20;

            TaskID = 220;
            Name = "い号作戦";
            Description = "一周内击沉 20 艘敌方航母";
            TaskCycle = Cycle.Week;

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
