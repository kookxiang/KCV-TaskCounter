using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Daily {
    public class ShipBuild : Task {

        public override void Initialize() {
            MaxCount[0] = 1;

            TaskID = 606;
            Name = "新造艦「建造」指令";
            Description = "建造一艘新船";
            TaskCycle = Cycle.Day;

            Hooks.OnBuildShip += new Hooks.OnBuildShipHandler(OnBuildShip);
        }

        public void OnBuildShip() {
            Increase();
        }
    }
}
