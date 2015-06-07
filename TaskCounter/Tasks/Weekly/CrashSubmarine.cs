using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Weekly {
    public class CrashSubmarine : Task {
        // 潜艇 ID
        private readonly int[] Submarines = new int[] { 530, 531, 532, 533, 534, 535, 570, 571, 572 };

        public override void Initialize() {
            MaxCount[0] = 15;

            TaskID = 228;
            Name = "海上护卫战";
            Description = "一周内击沉敌潜水艇 15 艘以上";
            TaskCycle = Cycle.Week;

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
