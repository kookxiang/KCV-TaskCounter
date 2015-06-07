using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Weekly {
    public class PraacticeWin : Task {
        public override void Initialize() {
            MaxCount[0] = 20;

            TaskID = 302;
            Name = "大規模演習";
            Description = "一周内参与演习 20 次并取得胜利";
            TaskCycle = Cycle.Week;
            
            Hooks.OnPractice += new Hooks.OnPracticeHandler(OnPractice);
        }

        public void OnPractice(string Rank) {
            if(Rank != "C" && Rank != "D")
                Increase();
        }
    }
}
