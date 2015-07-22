using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Daily {
    public class ShipDestory : Task {

        public override void Initialize() {
            MaxCount[0] = 2;

            TaskID = 609;
            Name = "軍縮条約対応！";
            Description = "解体 2 艘舰船";
            TaskCycle = Cycle.Day;

            Hooks.OnDestoryShip += new Hooks.OnDestoryShipHandler(OnDestoryShip);
        }

        public void OnDestoryShip() {
            Increase();
        }
    }
}
