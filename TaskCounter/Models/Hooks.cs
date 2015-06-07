using Grabacr07.KanColleWrapper.Models.Raw;

namespace TaskCounter.Models {
    public class Hooks {
        public delegate void OnEnemyShipSinkHandler(ShipData ship);
        public static OnEnemyShipSinkHandler OnEnemyShipSink;

        public delegate void OnQuestListChangeHandler();
        public static OnQuestListChangeHandler OnQuestListChange;

        public delegate void OnBattleFinishHandler(kcsapi_battleresult RawBattleResultData);
        public static OnBattleFinishHandler OnBattleFinish;

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
    }
}
