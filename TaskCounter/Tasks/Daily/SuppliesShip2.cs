using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Daily {
    public class SuppliesShip2 : Task {
        public bool ConflictMode = false;

        public override void Initialize() {
            MaxCount[0] = 5;

            TaskID = 212;
            Name = "敵輸送船団を叩け！";
            Description = "击沉敌方 5 艘补给舰";
            TaskCycle = Cycle.Day;

            Hooks.OnEnemyShipSink += new Hooks.OnEnemyShipSinkHandler(ship => {
                if (!ShipConst.Supplies.Contains(ship.Id))
                    return;
                Increase(0, ConflictMode ? 2 : 1);
            });

            Hooks.OnTaskListChanged += new Hooks.OnTaskListChangedHandler((AcceptedMission, AvailableMission) => {
                ConflictMode = AcceptedMission.Contains(218);
            });
        }

        public void onEnemyShipSink(ShipData ship) {
            if (!ShipConst.Supplies.Contains(ship.Id))
                return;
            Increase();
        }
    }
}
