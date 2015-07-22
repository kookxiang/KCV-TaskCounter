using System.Runtime.Serialization;

namespace TaskCounter.Models {
    [DataContract]
    public enum Formation {
        [EnumMember]
        不明 = -1,
        [EnumMember]
        无 = 0,
        [EnumMember]
        单纵阵 = 1,
        [EnumMember]
        复纵阵 = 2,
        [EnumMember]
        轮形阵 = 3,
        [EnumMember]
        梯形阵 = 4,
        [EnumMember]
        单横阵 = 5,
        [EnumMember]
        対潜阵型 = 11,
        [EnumMember]
        前方阵型 = 12,
        [EnumMember]
        轮型阵型 = 13,
        [EnumMember]
        战斗阵型 = 14,
    }
}
