using System.Collections.Generic;
using System.Linq;

public sealed class BehaviorStateUtils {

    public static Dictionary<BehaviorStateEnum, string> DICO_CORRESPONDANCE_BEHAVIOR_LABEL = new Dictionary<BehaviorStateEnum, string>();
    public static Dictionary<string, BehaviorStateEnum> DICO_CORRESPONDANCE_LABEL_BEHAVIOR = new Dictionary<string, BehaviorStateEnum>();


    static BehaviorStateUtils() {
        DICO_CORRESPONDANCE_BEHAVIOR_LABEL.Add(BehaviorStateEnum.BONUS, "Bonus");
        DICO_CORRESPONDANCE_BEHAVIOR_LABEL.Add(BehaviorStateEnum.MALUS, "Malus");

        DICO_CORRESPONDANCE_LABEL_BEHAVIOR = DICO_CORRESPONDANCE_BEHAVIOR_LABEL.ToDictionary(x => x.Value, x => x.Key);
    }
}