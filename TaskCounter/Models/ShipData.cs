using Grabacr07.KanColleWrapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskCounter.Models {
    public class ShipData {

        private readonly ShipInfo EnemyInfo;

        private readonly Ship ShipSource;

        public int Id {
            get {
                return EnemyInfo.Id;
            }
        }

        public string Name {
            get {
                return ShipSource != null
                    ? ShipSource.Info.Name
                    : EnemyInfo != null
                        ? EnemyInfo.Name
                        : "Unknown";
            }
        }

        public string AdditionalName {
            get {
                if (EnemyInfo == null)
                    return "";
                var isEnemyID = 500 < EnemyInfo.Id && EnemyInfo.Id < 901;
                return isEnemyID ? KCVPlugin.RawStart2.api_mst_ship.Single(x => x.api_id == EnemyInfo.Id).api_yomi : "";
            }
        }

        public string TypeName {
            get {
                return ShipSource != null
                    ? ShipSource.Info.ShipType.Name
                    : EnemyInfo != null
                        ? EnemyInfo.ShipType.Name
                        : "Unknown";
            }
        }

        public ShipSituation Situation {
            get {
                return ShipSource != null ? ShipSource.Situation : ShipSituation.None;
            }
        }

        public int SourceMaxHP {
            get {
                return ShipSource != null
                    ? ShipSource.HP.Maximum
                    : EnemyInfo != null
                        ? EnemyInfo.HP
                        : 0;
            }
        }

        public int SourceNowHP {
            get {
                return ShipSource != null
                    ? ShipSource.HP.Current
                    : EnemyInfo != null
                        ? EnemyInfo.HP
                        : 0;
            }
        }

        public int MaxHP;
        public int NowHP;
        public LimitedValue HP {
            get {
                return new LimitedValue(NowHP, MaxHP, 0);
            }
        }

        public ShipData() {
        }

        public ShipData(ShipInfo info) {
            EnemyInfo = info;
        }

        public ShipData(Ship ship) {
            ShipSource = ship;
        }
    }

    public static class ShipDataExtensions {
        /// <summary>
        /// Actionを使用して値を設定
        /// Zipするので要素数が少ない方に合わせられる
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="values"></param>
        /// <param name="setter"></param>
        public static void SetValues<TSource, TValue>(
            this TSource[] source,
            IEnumerable<TValue> values,
            Action<TSource, TValue> setter) {
            source.Zip(values, (s, v) => new {
                s,
                v
            })
                .ToList()
                .ForEach(x => setter(x.s, x.v));
        }

        /// <summary>
        /// ダメージ適用
        /// </summary>
        /// <param name="ships">艦隊</param>
        /// <param name="damages">適用ダメージリスト</param>
        public static void CalcDamages(this ShipData[] ships, params FleetDamages[] damages) {
            foreach (var damage in damages) {
                ships.SetValues(damage.ToArray(), (s, d) => s.NowHP -= d);
            }
        }

        /// <summary>
        /// ダメージ適用
        /// </summary>
        /// <param name="ships">艦隊</param>
        /// <param name="damages">適用ダメージリスト</param>
        public static void CalcEnemyDamages(this ShipData[] ships, params FleetDamages[] damages) {
            foreach (var damage in damages) {
                ships.SetValues(damage.ToArray(), (s, d) => {
                    if (s.NowHP > 0 && s.NowHP <= d)
                        Hooks.OnEnemyShipSink(s);
                    s.NowHP -= d;
                });
            }
        }
    }
}
