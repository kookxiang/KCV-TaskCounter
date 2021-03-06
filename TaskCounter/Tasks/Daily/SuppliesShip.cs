﻿using System.Linq;
using TaskCounter.Models;

namespace TaskCounter.Tasks.Daily {
    public class SuppliesShip : Task {
        public bool ConflictMode = false;

        public override void Initialize() {
            MaxCount[0] = 3;

            TaskID = 218;
            Name = "敵補給艦を3隻撃沈せよ！";
            Description = "击沉敌方 3 艘补给舰";
            TaskCycle = Cycle.Day;
            
            Hooks.OnEnemyShipSink += new Hooks.OnEnemyShipSinkHandler(ship => {
                if (!ShipConst.Supplies.Contains(ship.Id))
                    return;
                Increase(0, ConflictMode ? 2 : 1);
            });

            Hooks.OnTaskListChanged += new Hooks.OnTaskListChangedHandler((AcceptedMission, AvailableMission) => {
                ConflictMode = AcceptedMission.Contains(212);
            });
        }
    }
}
