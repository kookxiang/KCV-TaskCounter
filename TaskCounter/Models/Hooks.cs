namespace TaskCounter.Models {
    public class Hooks {
        public delegate void OnEnemyShipSinkHandler(ShipData ship);
        public static OnEnemyShipSinkHandler OnEnemyShipSink;

        public delegate void OnSupplyHandler();
        public static OnSupplyHandler OnSupply;

        public delegate void OnExpeditionSuccessHandler();
        public static OnExpeditionSuccessHandler OnExpeditionSuccess;

        public delegate void OnDestroyItemHandler();
        public static OnDestroyItemHandler OnDestroyItem;

        public delegate void OnPowerUpHandler();
        public static OnPowerUpHandler OnPowerUp;

        public delegate void OnRepairHandler();
        public static OnRepairHandler OnRepair;

        public delegate void OnBuildShipHandler();
        public static OnBuildShipHandler OnBuildShip;

        public delegate void OnCreateItemHandler();
        public static OnCreateItemHandler OnCreateItem;

        public delegate void OnDestoryShipHandler();
        public static OnDestoryShipHandler OnDestoryShip;

        public delegate void OnEnterMapHandler(int MapAera, int MapID, bool isBoss);
        public static OnEnterMapHandler OnEnterMap;
        
        public delegate void OnBattleFinishHandler(int MapAera, int MapID, bool isBoss, string Rank);
        public static OnBattleFinishHandler OnBattleFinish;

        public delegate void OnPracticeHandler(string api_win_rank);
        public static OnPracticeHandler OnPractice;

        public delegate void OnTaskListChangedHandler(int[] AcceptedMission, int[] AvailableMission);
        public static OnTaskListChangedHandler OnTaskListChanged;
    }
}
