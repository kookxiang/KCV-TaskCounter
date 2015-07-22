using Grabacr07.KanColleWrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using TaskCounter.Models.Raw;

namespace TaskCounter.Models {
    [DataContract]
    public class EnemyDataProvider {
        private static DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(EnemyDataProvider));

        // EnemyId, EnemyMasterIDs
        [DataMember]
        private Dictionary<int, int[]> EnemyDictionary {
            get; set;
        }

        // EnemyId, Formation
        [DataMember]
        private Dictionary<int, Formation> EnemyFormation {
            get; set;
        }


        //以下はとりあえず保存だけする

        // EnemyId, api_eSlot
        [DataMember]
        private Dictionary<int, int[][]> EnemySlotItems {
            get; set;
        }

        // MapInfoID, CellNo, EnemyId
        [DataMember]
        private Dictionary<int, Dictionary<int, HashSet<int>>> MapEnemyData {
            get; set;
        }

        // MapInfoID, FromCellNo, ToCellNo
        [DataMember]
        private Dictionary<int, HashSet<KeyValuePair<int, int>>> MapRoute {
            get; set;
        }

        // MapInfoID, CellNo
        [DataMember]
        private Dictionary<int, int> MapBossCellNo {
            get; set;
        }

        [NonSerialized]
        private int currentEnemyID;

        [NonSerialized]
        private int previousCellNo;

        public EnemyDataProvider() {
            if (EnemyDictionary == null)
                EnemyDictionary = new Dictionary<int, int[]>();
            if (EnemyFormation == null)
                EnemyFormation = new Dictionary<int, Formation>();
            if (EnemySlotItems == null)
                EnemySlotItems = new Dictionary<int, int[][]>();
            if (MapEnemyData == null)
                MapEnemyData = new Dictionary<int, Dictionary<int, HashSet<int>>>();
            if (MapRoute == null)
                MapRoute = new Dictionary<int, HashSet<KeyValuePair<int, int>>>();
            if (MapBossCellNo == null)
                MapBossCellNo = new Dictionary<int, int>();
            previousCellNo = 0;
            Dump("GetNextEnemyFormation");
        }

        public Formation GetNextEnemyFormation(map_start_next startNext) {
            Dump("GetNextEnemyFormation");

            if (startNext.api_enemy == null)
                return Formation.无;
            currentEnemyID = startNext.api_enemy.api_enemy_id;

            return this.EnemyFormation.ContainsKey(startNext.api_enemy.api_enemy_id)
                ? EnemyFormation[startNext.api_enemy.api_enemy_id]
                : Formation.不明;
        }

        public ShipData[] GetNextEnemies(map_start_next startNext) {
            Dump("GetNextEnemies");

            if (startNext.api_enemy == null)
                return new ShipData[0];
            currentEnemyID = startNext.api_enemy.api_enemy_id;

            var master = KanColleClient.Current.Master.Ships;
            return EnemyDictionary.ContainsKey(startNext.api_enemy.api_enemy_id)
                ? EnemyDictionary[startNext.api_enemy.api_enemy_id].Select(x => new ShipData(master[x])).ToArray()
                : Enumerable.Repeat(new ShipData(), 6).ToArray();
        }

        public void UpdateMapData(map_start_next startNext) {
            UpdateMapEnemyData(startNext);
            UpdateMapRoute(startNext);
            UpdateMapBossCellNo(startNext);
            Dump("UpdateMapData");
        }

        private void UpdateMapEnemyData(map_start_next startNext) {
            if (startNext.api_enemy == null)
                return;

            var mapInfo = GetMapInfo(startNext);

            if (!MapEnemyData.ContainsKey(mapInfo))
                MapEnemyData.Add(mapInfo, new Dictionary<int, HashSet<int>>());
            if (!MapEnemyData[mapInfo].ContainsKey(startNext.api_no))
                MapEnemyData[mapInfo].Add(startNext.api_no, new HashSet<int>());

            MapEnemyData[mapInfo][startNext.api_no].Add(startNext.api_enemy.api_enemy_id);
        }

        private void UpdateMapRoute(map_start_next startNext) {
            var mapInfo = GetMapInfo(startNext);
            if (!MapRoute.ContainsKey(mapInfo))
                MapRoute.Add(mapInfo, new HashSet<KeyValuePair<int, int>>());

            MapRoute[mapInfo].Add(new KeyValuePair<int, int>(this.previousCellNo, startNext.api_no));

            previousCellNo = 0 < startNext.api_next ? startNext.api_no : 0;
        }

        private void UpdateMapBossCellNo(map_start_next startNext) {
            var mapInfo = GetMapInfo(startNext);
            if (!MapBossCellNo.ContainsKey(mapInfo))
                MapBossCellNo.Add(mapInfo, startNext.api_bosscell_no);
            else
                MapBossCellNo[mapInfo] = startNext.api_bosscell_no;
        }

        private static int GetMapInfo(map_start_next startNext) {
            return KanColleClient.Current.Master.MapInfos
                .Select(x => x.Value)
                .Where(m => m.MapAreaId == startNext.api_maparea_id)
                .Single(m => m.IdInEachMapArea == startNext.api_mapinfo_no)
                .Id;
        }

        public void UpdateEnemyData(int[] api_ship_ke, int[] api_formation, int[][] api_eSlot) {
            var enemies = api_ship_ke.Where(x => x != -1).ToArray();
            var formation = (Formation)api_formation[1];
            NewMethod(enemies);

            if (EnemyFormation.ContainsKey(currentEnemyID))
                EnemyFormation[currentEnemyID] = formation;
            else
                EnemyFormation.Add(currentEnemyID, formation);

            if (EnemySlotItems.ContainsKey(currentEnemyID))
                EnemySlotItems[currentEnemyID] = api_eSlot;
            else
                EnemySlotItems.Add(currentEnemyID, api_eSlot);
            
            Dump("UpdateEnemyData");
        }

        private void NewMethod(int[] enemies) {
            if (EnemyDictionary.ContainsKey(currentEnemyID))
                EnemyDictionary[currentEnemyID] = enemies;
            else
                EnemyDictionary.Add(currentEnemyID, enemies);
        }

        public void Dump(string title = "") {
            Debug.WriteLine(title);
            //this.EnemyDictionary.SelectMany(x => x.Value, (key, value) => new { key, value })
            //    .ToList().ForEach(x => Debug.WriteLine(x.key + " : " + x.value));
            //this.EnemyFormation
            //    .ToList().ForEach(x => Debug.WriteLine(x.Key + " : " + x.Value));
        }
    }
}
