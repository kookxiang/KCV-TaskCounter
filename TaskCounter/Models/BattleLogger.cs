using Grabacr07.KanColleWrapper;
using Livet;
using System;
using System.Linq;
using System.Reactive.Linq;
using TaskCounter.Models.Raw;

namespace TaskCounter.Models {
    public class BattleLogger : NotificationObject {
        #region UpdatedTime変更通知プロパティ
        private DateTimeOffset _UpdatedTime;

        public DateTimeOffset UpdatedTime {
            get {
                return _UpdatedTime;
            }
            set {
                if (_UpdatedTime == value)
                    return;
                _UpdatedTime = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region BattleSituation変更通知プロパティ

        private BattleSituation _BattleSituation;

        public BattleSituation BattleSituation {
            get {
                return _BattleSituation;
            }
            set {
                if (_BattleSituation == value)
                    return;
                _BattleSituation = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Enemies変更通知プロパティ
        private ShipData[] _Enemies;

        public ShipData[] Enemies {
            get {
                return _Enemies;
            }
            set {
                if (_Enemies == value)
                    return;
                _Enemies = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        private int CurrentDeckId {
            get; set;
        }
        private readonly EnemyDataProvider provider = new EnemyDataProvider();

        public BattleLogger() {
            var kanColleProxy = KanColleClient.Current.Proxy;

            // 通常 - 昼战
            kanColleProxy.api_req_sortie_battle.TryParse<sortie_battle>().Subscribe(x => Update(x.Data));

            // 通常 - 夜战
            kanColleProxy.ApiSessionSource.Where(x => x.PathAndQuery == "/kcsapi/api_req_battle_midnight/battle")
                .TryParse<battle_midnight_battle>().Subscribe(x => Update(x.Data));

            // 通常 - 开幕夜战
            kanColleProxy.ApiSessionSource.Where(x => x.PathAndQuery == "/kcsapi/api_req_battle_midnight/sp_midnight")
                .TryParse<battle_midnight_sp_midnight>().Subscribe(x => Update(x.Data));

            // 出击 && 进击
            kanColleProxy.ApiSessionSource.Where(x => x.PathAndQuery == "/kcsapi/api_req_map/start")
                .TryParse<map_start_next>().Subscribe(x => UpdateFleetsByStartNext(x.Data, x.Request["api_deck_id"]));
            kanColleProxy.ApiSessionSource.Where(x => x.PathAndQuery == "/kcsapi/api_req_map/next")
                .TryParse<map_start_next>().Subscribe(x => UpdateFleetsByStartNext(x.Data));
        }

        private void UpdateFleetsByStartNext(map_start_next startNext, string api_deck_id = null) {
            Hooks.OnEnterMap(startNext.api_maparea_id, startNext.api_mapinfo_no, startNext.api_bosscell_no == startNext.api_no);

            UpdatedTime = DateTimeOffset.Now;

            BattleSituation = BattleSituation.未开战;
            Enemies = provider.GetNextEnemies(startNext);
            Enemies.UpdateHPBySource();

            if (api_deck_id != null)
                CurrentDeckId = int.Parse(api_deck_id);
            if (CurrentDeckId < 1)
                return;
        }

        private void Update(sortie_battle data) {
            UpdateFleets(data.api_dock_id, data.api_ship_ke, data.api_formation, null, false);

            UpdateMaxHP(data.api_maxhps);
            UpdateNowHP(data.api_nowhps);

            Enemies.CalcEnemyDamages(
                data.api_support_info.GetEnemyDamages(),
                data.api_kouku.GetEnemyDamages(),
                data.api_opening_atack.GetEnemyDamages(),
                data.api_hougeki1.GetEnemyDamages(),
                data.api_hougeki2.GetEnemyDamages(),
                data.api_raigeki.GetEnemyDamages()
            );
        }

        private void Update(battle_midnight_battle data) {
            UpdateFleets(data.api_deck_id, data.api_ship_ke);

            UpdateMaxHP(data.api_maxhps);
            UpdateNowHP(data.api_nowhps);

            Enemies.CalcEnemyDamages(
                data.api_hougeki.GetEnemyDamages()
            );
        }

        private void Update(battle_midnight_sp_midnight data) {
            UpdateFleets(data.api_deck_id, data.api_ship_ke, data.api_formation, data.api_eSlot);

            UpdateMaxHP(data.api_maxhps);
            UpdateNowHP(data.api_nowhps);

            Enemies.CalcEnemyDamages(
                data.api_hougeki.GetEnemyDamages()
            );
        }

        private void UpdateFleets(
            int api_deck_id,
            int[] api_ship_ke,
            int[] api_formation = null,
            int[][] api_eSlot = null,
            bool isUpdateEnemyData = true) {
            UpdatedTime = DateTimeOffset.Now;

            if (api_formation != null) {
                BattleSituation = (BattleSituation)api_formation[2];
                if (isUpdateEnemyData)
                    provider.UpdateEnemyData(api_ship_ke, api_formation, api_eSlot);
            }

            var master = KanColleClient.Current.Master.Ships;
            Enemies = api_ship_ke.Where(x => x != -1).Select(x => new ShipData(master[x])).ToArray();

            CurrentDeckId = api_deck_id;
        }

        private void UpdateMaxHP(int[] api_maxhps, int[] api_maxhps_combined = null) {
            Enemies.SetValues(api_maxhps.GetEnemyData(), (s, v) => s.MaxHP = v);
        }

        private void UpdateNowHP(int[] api_nowhps, int[] api_nowhps_combined = null) {
            Enemies.SetValues(api_nowhps.GetEnemyData(), (s, v) => s.NowHP = v);
        }
    }

    static class BattleDataExtensions {
        public static void UpdateHPBySource(this ShipData[] target) {
            target.SetValues(target.Select(x => x.SourceMaxHP), (s, v) => s.MaxHP = v);
            target.SetValues(target.Select(x => x.SourceNowHP), (s, v) => s.NowHP = v);
        }
    }
}
