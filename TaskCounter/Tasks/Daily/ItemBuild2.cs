using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Daily {
    public class ItemBuild2 : Task {

        public override void Initialize() {
            MaxCount[0] = 3;

            TaskID = 607;
            Name = "装備「開発」集中強化！";
            Description = "进行 3 次装备开发. (失败了也没问题)";
            TaskCycle = Cycle.Day;

            Hooks.OnCreateItem += new Hooks.OnCreateItemHandler(OnCreateItem);
        }

        public void OnCreateItem() {
            Increase();
        }
    }
}
