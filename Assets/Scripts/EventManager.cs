
using System;

public class EventManager
{

    public static event Action<string> ChangeTopLevelCommandMenuRequired;
    public static event Action<BehaviorStateEnum, string> ChangeDetailedCommandMenuRequired;
    public static event Action<int> SelectDetailedCommandMenuItem;
    public static event Action<int> BroadcastLevel;


    public static void TriggerChangeTopLevelCommandMenu(string behaviorLabel) // Si mot-clé event (si pas présent => multi cast)
    {
        ChangeTopLevelCommandMenuRequired?.Invoke(behaviorLabel);
    }


    public static void TriggerChangeDetailedCommandMenu(BehaviorStateEnum behaviorStateEnum, string detailedMenuType)
    {
        ChangeDetailedCommandMenuRequired?.Invoke(behaviorStateEnum, detailedMenuType);
    }

    public static void TriggerSelectDetailedCommandMenuItem(int levelsAmount)
    {
        SelectDetailedCommandMenuItem?.Invoke(levelsAmount);
    }

    public static void TriggerBroadcastLevel(int level) {
        BroadcastLevel?.Invoke(level);
    }
}
