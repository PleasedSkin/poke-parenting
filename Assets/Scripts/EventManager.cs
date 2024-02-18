
using System;

public class EventManager
{

    public static event Action<string> ChangeTopLevelCommandMenuRequired;
    public static event Action<string, string> ChangeDetailedCommandMenuRequired;


    public static void TriggerChangeTopLevelCommandMenu(string menuType) // Si mot-clé event (si pas présent => multi cast)
    {
        ChangeTopLevelCommandMenuRequired?.Invoke(menuType);
    }


    public static void TriggerChangeDetailedCommandMenu(string menuType, string detailedMenuType)
    {
        ChangeDetailedCommandMenuRequired?.Invoke(menuType, detailedMenuType);
    }
}
