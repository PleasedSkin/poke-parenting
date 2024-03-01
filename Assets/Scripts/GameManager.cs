using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int level = 0;
    private int starsAmount = 0;
    private int currentPokemonNumber;

    private Dictionary<int, int> evolutionDictionary = new Dictionary<int, int>();

    

    void OnEnable() {
        EventManager.SelectDetailedCommandMenuItem += SelectDetailedCommandMenuItem;
        EventManager.BroadcastEvolutionDictionary += UpdateEvolutionDictionary;
        EventManager.BroadcastPokemonNumber += UpdatePokemonNumber;
    }

    void OnDisable() {
        EventManager.SelectDetailedCommandMenuItem -= SelectDetailedCommandMenuItem;
        EventManager.BroadcastEvolutionDictionary -= UpdateEvolutionDictionary;
        EventManager.BroadcastPokemonNumber -= UpdatePokemonNumber;
    }

  
    void SelectDetailedCommandMenuItem(int levelsAmount) {
        SetLevel(levelsAmount);
    }

    private void UpdateEvolutionDictionary(Dictionary<int, int> dico) {
        evolutionDictionary = dico;
    }

    private void UpdatePokemonNumber(int pokemonNumber) {
        currentPokemonNumber = pokemonNumber;
    }


    private void SetLevel(int levelsAmount) {
        var oldLevel = level;
        level += levelsAmount;
        level = Mathf.Clamp(level, 0, 100);
        EventManager.TriggerBroadcastLevel(level);

        ReactToLevelChange(oldLevel, level);
    }


 
    private void ReactToLevelChange(int oldLevel, int newLevel) {
        if (oldLevel == 0 && newLevel > 0) {
            EventManager.TriggerGenerateNewPokemon();
        } else if (level == 0) {
            EventManager.TriggerResetPokemon();
        } else if (oldLevel == 100 && newLevel == 100) {
            HandlePokemonAchievment();
        } else {
            HandlePossibleEvolution();
        }	
    }

    private void HandlePokemonAchievment() {
        EventManager.TriggerResetPokemon();
        level = 0;
        EventManager.TriggerBroadcastLevel(level);
        starsAmount += 1;
        EventManager.TriggerBroadcastStarsAmount(starsAmount);
    }

    private void HandlePossibleEvolution() {
        var relevantLevel = 0;
        foreach (var evolutionLevel in evolutionDictionary.Keys)
        {
            if (level >= evolutionLevel) {
                relevantLevel = evolutionLevel;
                Debug.Log(relevantLevel);
            }
        }

        if (relevantLevel > 0) {
            var targetPokemonNumber = evolutionDictionary.GetValueOrDefault(relevantLevel);
            if (targetPokemonNumber != currentPokemonNumber && !IsHigherInEvolutionTree()) {
                currentPokemonNumber = targetPokemonNumber;
                EventManager.TriggerGeneratePokemonEvolution(currentPokemonNumber);
            }
        }
    }

    private bool IsHigherInEvolutionTree() {
        var result = false;

        foreach (var evolutionLevel in evolutionDictionary.Keys)
        {
            if (evolutionDictionary.GetValueOrDefault(evolutionLevel) == currentPokemonNumber && level < evolutionLevel) {
                return true;
            }
        }

        return result;
    }

}
