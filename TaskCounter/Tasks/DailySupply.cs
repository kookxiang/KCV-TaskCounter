using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks {
    public class DailySupply : Task {

        public override void Initialize() {
            MaxCount[0] = 15;

            TaskID = 504;
            Name = "艦隊酒保祭り！";
            Description = "补给 15 次. (为一艘船只单独补给算一次，为舰队全体补给也只算一次)";
            TaskCycle = Cycle.Day;

            Hooks.OnSupply += new Hooks.OnSupplyHandler(OnSupply);
        }

        public void OnSupply() {
            Increase();
        }
    }
}
