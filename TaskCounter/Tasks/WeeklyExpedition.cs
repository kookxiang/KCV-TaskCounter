using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks {
    public class WeeklyExpedition : Task {

        public override void Initialize() {
            MaxCount[0] = 30;

            TaskID = 404;
            Name = "大規模遠征作戦、発令！";
            Description = "一周内远征成功 30 次";
            TaskCycle = Cycle.Week;
            
            Hooks.OnExpeditionSuccess += new Hooks.OnExpeditionSuccessHandler(OnExpeditionSuccess);
        }

        public void OnExpeditionSuccess() {
            Increase();
        }
    }
}
