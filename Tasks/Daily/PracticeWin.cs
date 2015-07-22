using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Daily {
    public class PraacticeWin : Task {
        public override void Initialize() {
            MaxCount[0] = 5;

            TaskID = 304;
            Name = "「演習」で他提督を圧倒せよ！";
            Description = "一天内参与演习 5 次并取得胜利";
            TaskCycle = Cycle.Day;
            
            Hooks.OnPractice += new Hooks.OnPracticeHandler(OnPractice);
        }

        public void OnPractice(string Rank) {
            if(Rank != "C" && Rank != "D")
                Increase();
        }
    }
}
