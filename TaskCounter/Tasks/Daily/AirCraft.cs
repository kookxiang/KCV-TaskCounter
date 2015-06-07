using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Daily {
    public class AirCraft : Task {
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
            if (!Ship.AirCarriers.Contains(ship.Id))
                return;
            Increase();
        }
    }
}
