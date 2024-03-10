
using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{

    public static event Action<string> ChangeTopLevelCommandMenuRequired;
    public static event Action<BehaviorStateEnum, string> ChangeDetailedCommandMenuRequired;
    public static event Action<int> SelectDetailedCommandMenuItem;
    public static event Action<int, bool> BroadcastLevel;
    public static event Action<string> BroadcastName;
    public static event Action<int> BroadcastPokemonNumber;
    public static event Action<Sprite, bool> BroadcastPokemonSprite;
    public static event Action<int, bool> BroadcastStarsAmount;
    public static event Action<int, bool> BroadcastDropsAmount;
    public static event Action ReturnRequired;
    public static event Action ResetPokemon;
    public static event Action GenerateNewPokemon;
    public static event Action<int> GenerateTargetPokemonFromSaveData;
    public static event Action DisplayLoadingSprite;
    public static event Action<Dictionary<int, int>> BroadcastEvolutionDictionary;
    public static event Action<int> GeneratePokemonEvolution;
    public static event Action<bool> BroadcastShinyInfo;


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

    public static void TriggerBroadcastLevel(int level, bool isLoadingDataContext = false) {
        BroadcastLevel?.Invoke(level, isLoadingDataContext);
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

    public static void TriggerGenerateTargetPokemonFromSaveData(int pokemonNumber) {
        GenerateTargetPokemonFromSaveData?.Invoke(pokemonNumber);
    }

    public static void TriggerBroadcastPokemonSprite(Sprite sprite, bool isEvolving) {
        BroadcastPokemonSprite?.Invoke(sprite, isEvolving);
    }

    public static void TriggerBroadcastStarsAmount(int starsAmount, bool isLoadingDataContext = false) {
        BroadcastStarsAmount?.Invoke(starsAmount, isLoadingDataContext);
    }

    public static void TriggerBroadcastDropsAmount(int dropsAmount, bool isLoadingDataContext = false) {
        BroadcastDropsAmount?.Invoke(dropsAmount, isLoadingDataContext);
    }

    public static void TriggerDisplayLoadingSprite() {
        DisplayLoadingSprite?.Invoke();
    }

    public static void TriggerBroadcastEvolutionDictionary(Dictionary<int, int> dico) {
        BroadcastEvolutionDictionary?.Invoke(dico);
    }

    public static void TriggerBroadcastPokemonNumber(int pokemonNumber) {
        BroadcastPokemonNumber?.Invoke(pokemonNumber);
    }

    public static void TriggerGeneratePokemonEvolution(int targetPokemonNumber) {
        GeneratePokemonEvolution?.Invoke(targetPokemonNumber);
    }

    public static void TriggerBroadcastShinyInfo(bool isShiny) {
        BroadcastShinyInfo?.Invoke(isShiny);
    }

}
