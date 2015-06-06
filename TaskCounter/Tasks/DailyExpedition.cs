using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks {
    public class DailyExpedition : Task {

        public override void Initialize() {
            MaxCount[0] = 3;

            TaskID = 402;
            Name = "「遠征」を３回成功させよう！";
            Description = "一天内远征成功 3 次";
            TaskCycle = Cycle.Day;
            
            Hooks.OnExpeditionSuccess += new Hooks.OnExpeditionSuccessHandler(OnExpeditionSuccess);
        }

        public void OnExpeditionSuccess() {
            Increase();
        }
    }
}
