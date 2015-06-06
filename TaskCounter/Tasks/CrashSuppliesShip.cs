using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks {
    public class CrashSuppliesShip : Task {
        // 补给船 ID
        private readonly int[] Supplies = new int[] { 513, 526, 558 };

        public override void Initialize() {
            MaxCount[0] = 20;

            TaskID = 213;
            Name = "海上通商破壊作戦";
            Description = "一周内击沉 20 艘补给船";
            TaskCycle = Cycle.Week;

            // 挂钩子：战斗结束检查
            Hooks.OnEnemyShipSink += new Hooks.OnEnemyShipSinkHandler(onEnemyShipSink);
        }

        public void onEnemyShipSink(ShipData ship) {
            if (!Supplies.Contains(ship.Id))
                return;
            Increase();
        }
    }
}
