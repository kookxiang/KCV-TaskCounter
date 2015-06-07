using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Weekly {
    public class OperationRo : Task {
        public override void Initialize() {
            MaxCount[0] = 50;

            TaskID = 221;
            Name = "ろ号作戦";
            Description = "一周内击沉 50 艘补给船";
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
