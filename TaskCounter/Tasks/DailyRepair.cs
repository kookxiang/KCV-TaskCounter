using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks {
    public class DailyRepair : Task {

        public override void Initialize() {
            MaxCount[0] = 5;

            TaskID = 503;
            Name = "艦隊大整備！";
            Description = "入渠修复 5 条船只";
            TaskCycle = Cycle.Day;

            Hooks.OnRepair += new Hooks.OnRepairHandler(OnRepair);
        }

        public void OnRepair() {
            Increase();
        }
    }
}
