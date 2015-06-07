using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Daily {
    public class Submarine : Task {
        // 潜艇 ID
        private readonly int[] Submarines = new int[] { 530, 531, 532, 533, 534, 535, 570, 571, 572 };

        public override void Initialize() {
            MaxCount[0] = 6;

            TaskID = 230;
            Name = "敵潜水艦を制圧せよ！";
            Description = "击沉敌方 6 艘潜艇";
            TaskCycle = Cycle.Day;

            // 挂钩子：战斗结束检查
            Hooks.OnEnemyShipSink += new Hooks.OnEnemyShipSinkHandler(onEnemyShipSink);
        }

        public void onEnemyShipSink(ShipData ship) {
            if (!Submarines.Contains(ship.Id))
                return;
            Increase();
        }
    }
}
