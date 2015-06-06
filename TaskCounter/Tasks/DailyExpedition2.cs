using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks {
    public class DailyExpedition2 : Task {

        public override void Initialize() {
            MaxCount[0] = 10;

            TaskID = 403;
            Name = "「遠征」を１０回成功させよう！";
            Description = "一天内远征成功 10 次";
            TaskCycle = Cycle.Day;

            Hooks.OnExpeditionSuccess += new Hooks.OnExpeditionSuccessHandler(OnExpeditionSuccess);
        }

        public void OnExpeditionSuccess() {
            Increase();
        }
    }
}
