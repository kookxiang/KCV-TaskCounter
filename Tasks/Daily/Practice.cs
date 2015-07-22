using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Daily {
    public class Praactice : Task {
        public override void Initialize() {
            MaxCount[0] = 3;

            TaskID = 303;
            Name = "「演習」で練度向上！";
            Description = "一天内参与演习 3 次";
            TaskCycle = Cycle.Day;
            
            Hooks.OnPractice += new Hooks.OnPracticeHandler(OnPractice);
        }

        public void OnPractice(string Rank) {
            Increase();
        }
    }
}
