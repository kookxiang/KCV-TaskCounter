using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Weekly {
    public class CrashSuppliesShip : Task {
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
            if (!ShipConst.Supplies.Contains(ship.Id))
                return;
            Increase();
        }
    }
}
