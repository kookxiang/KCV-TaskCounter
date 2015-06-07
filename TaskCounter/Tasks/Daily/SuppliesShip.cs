using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Daily {
    public class SuppliesShip : Task {
        public override void Initialize() {
            MaxCount[0] = 3;

            TaskID = 218;
            Name = "敵補給艦を3隻撃沈せよ！";
            Description = "击沉敌方 3 艘补给舰";
            TaskCycle = Cycle.Day;

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
