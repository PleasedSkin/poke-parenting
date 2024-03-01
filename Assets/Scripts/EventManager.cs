
using System;
using UnityEngine;

public class EventManager
{

    public static event Action<string> ChangeTopLevelCommandMenuRequired;
    public static event Action<BehaviorStateEnum, string> ChangeDetailedCommandMenuRequired;
    public static event Action<int> SelectDetailedCommandMenuItem;
    public static event Action<int> BroadcastLevel;
    public static event Action<string> BroadcastName;
    public static event Action<Sprite> BroadcastPokemonSprite;
    public static event Action<int> BroadcastStarsAmount;
    public static event Action ReturnRequired;
    public static event Action ResetPokemon;
    public static event Action GenerateNewPokemon;
    public static event Action DisplayLoadingSprite;


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

    public static void TriggerBroadcastName(string name) {
        BroadcastName?.Invoke(name);
    }

    public static void TriggerReturn() {
        ReturnRequired?.Invoke();
    }

    public static void TriggerResetPokemon() {
        ResetPokemon?.Invoke();
    }

    public static void TriggerGenerateNewPokemon() {
        GenerateNewPokemon?.Invoke();
    }

    public static void TriggerBroadcastPokemonSprite(Sprite sprite) {
        BroadcastPokemonSprite?.Invoke(sprite);
    }

    public static void TriggerBroadcastStarsAmount(int starsAmount) {
        BroadcastStarsAmount?.Invoke(starsAmount);
    }

    public static void TriggerDisplayLoadingSprite() {
        DisplayLoadingSprite?.Invoke();
    }
}
