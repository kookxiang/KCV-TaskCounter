using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks {
    public class WeeklyDestory : Task {

        public override void Initialize() {
            MaxCount[0] = 24;

            TaskID = 613;
            Name = "資源の再利用";
            Description = "一周内废弃装备 24 次";
            TaskCycle = Cycle.Week;
            
            Hooks.OnDestroyItem += new Hooks.OnDestroyItemHandler(OnDestroyItem);
        }

        public void OnDestroyItem() {
            Increase();
        }
    }
}
