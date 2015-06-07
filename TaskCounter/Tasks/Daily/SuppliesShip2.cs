using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Daily {
    public class SuppliesShip2 : Task {
        public override void Initialize() {
            MaxCount[0] = 5;

            TaskID = 212;
            Name = "敵輸送船団を叩け！";
            Description = "击沉敌方 5 艘补给舰";
            TaskCycle = Cycle.Day;

            // 挂钩子：战斗结束检查
            Hooks.OnEnemyShipSink += new Hooks.OnEnemyShipSinkHandler(onEnemyShipSink);
        }

        public void onEnemyShipSink(ShipData ship) {
            if (!Ship.Supplies.Contains(ship.Id))
                return;
            Increase();
        }
    }
}
