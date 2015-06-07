using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Daily {
    public class ItemBuild : Task {

        public override void Initialize() {
            MaxCount[0] = 1;

            TaskID = 605;
            Name = "新装備「開発」指令";
            Description = "进行一次装备开发 (失败了也没问题)";
            TaskCycle = Cycle.Day;

            Hooks.OnCreateItem += new Hooks.OnCreateItemHandler(OnCreateItem);
        }

        public void OnCreateItem() {
            Increase();
        }
    }
}
