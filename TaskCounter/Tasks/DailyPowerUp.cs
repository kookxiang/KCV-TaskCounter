using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks {
    public class DailyPowerUp : Task {

        public override void Initialize() {
            MaxCount[0] = 2;

            TaskID = 702;
            Name = "艦の「近代化改修」を実施せよ！";
            Description = "一天内成功近代化改修 2 次";
            TaskCycle = Cycle.Day;

            Hooks.OnPowerUp += new Hooks.OnPowerUpHandler(OnPowerUp);
        }

        public void OnPowerUp() {
            Increase();
        }
    }
}
