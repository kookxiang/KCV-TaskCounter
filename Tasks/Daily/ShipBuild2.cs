using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Daily {
    public class ShipBuild2 : Task {

        public override void Initialize() {
            MaxCount[0] = 3;

            TaskID = 608;
            Name = "艦娘「建造」艦隊強化！";
            Description = "建造 3 艘新船";
            TaskCycle = Cycle.Day;

            Hooks.OnBuildShip += new Hooks.OnBuildShipHandler(OnBuildShip);
        }

        public void OnBuildShip() {
            Increase();
        }
    }
}
