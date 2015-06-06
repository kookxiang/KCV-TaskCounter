using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks {
    public class DailySuppliesShip2 : Task {
        // 补给船 ID
        private readonly int[] Supplies = new int[] { 513, 526, 558 };

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
            if (!Supplies.Contains(ship.Id))
                return;
            Increase();
        }
    }
}
