using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Weekly {
    public class OperationI : Task {
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
            if (!ShipConst.AirCarriers.Contains(ship.Id))
                return;
            Increase();
        }
    }
}
