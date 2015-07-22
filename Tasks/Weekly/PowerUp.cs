using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Weekly {
    public class PowerUp : Task {

        public override void Initialize() {
            MaxCount[0] = 15;

            TaskID = 703;
            Name = "「近代化改修」を進め、戦備を整えよ！";
            Description = "一周内成功近代化改修 15 次";
            TaskCycle = Cycle.Week;

            Hooks.OnPowerUp += new Hooks.OnPowerUpHandler(OnPowerUp);
        }

        public void OnPowerUp() {
            Increase();
        }
    }
}
