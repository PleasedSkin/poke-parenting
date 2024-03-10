using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { get; private set;}
    private static readonly string MAIN_SCENE_NAME = "MainScene";
    
    // Save data labels
    private static readonly string LEVEL_SAVE_LABEL = "Level";
    private static readonly string STARS_AMOUNT_SAVE_LABEL = "StarsAmount";
    private static readonly string DROPS_AMOUNT_SAVE_LABEL = "DropsAmount";
    private static readonly string POKEMON_NUMBER_SAVE_LABEL = "PokemonNumber";
    private static readonly string IS_SHINY_SAVE_LABEL = "IsShiny";

    private int level = 0;
    private int starsAmount = 0;
    private int dropsAmount = 0;
    private int currentPokemonNumber = 0;
    private bool isShiny = false;

    private Dictionary<int, int> evolutionDictionary = new Dictionary<int, int>();

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
    }

    void OnEnable() {
        EventManager.SelectDetailedCommandMenuItem += SelectDetailedCommandMenuItem;
        EventManager.BroadcastEvolutionDictionary += UpdateEvolutionDictionary;
        EventManager.BroadcastPokemonNumber += UpdatePokemonNumber;
        EventManager.BroadcastShinyInfo += UpdateShinyState;
        SceneManager.sceneLoaded += InitializePokemonIfNeeded;
    }

    void OnDisable() {
        EventManager.SelectDetailedCommandMenuItem -= SelectDetailedCommandMenuItem;
        EventManager.BroadcastEvolutionDictionary -= UpdateEvolutionDictionary;
        EventManager.BroadcastPokemonNumber -= UpdatePokemonNumber;
        EventManager.BroadcastShinyInfo -= UpdateShinyState;
        SceneManager.sceneLoaded -= InitializePokemonIfNeeded;
    }

  
    void SelectDetailedCommandMenuItem(int levelsAmount) {
        SetLevel(levelsAmount);
    }

    private void UpdateEvolutionDictionary(Dictionary<int, int> dico) {
        evolutionDictionary = dico;
    }

    private void UpdatePokemonNumber(int pokemonNumber) {
        currentPokemonNumber = pokemonNumber;
        PlayerPrefs.SetInt(POKEMON_NUMBER_SAVE_LABEL, pokemonNumber);
        SaveData();
    }

    private void UpdateShinyState(bool shinyState) {
        isShiny = shinyState;
        PlayerPrefs.SetInt(IS_SHINY_SAVE_LABEL, shinyState ? 1 : 0);
        SaveData();
    }


    private void SetLevel(int levelsAmount) {
        var oldLevel = level;
        level += levelsAmount;
        level = Mathf.Clamp(level, -100, 100);
        EventManager.TriggerBroadcastLevel(level);
        PlayerPrefs.SetInt(LEVEL_SAVE_LABEL, level);

        ReactToLevelChange(oldLevel, level);
        SaveData();
    }


 
    private void ReactToLevelChange(int oldLevel, int newLevel) {
        if (oldLevel <= 0 && newLevel > 0) {
            EventManager.TriggerPokemonRise();
            EventManager.TriggerGenerateNewPokemon();
        } else if (level == 0) {
            UpdatePokemonNumber(0);
            isShiny = false;
            EventManager.TriggerResetPokemon();
        } else if (oldLevel == 100 && newLevel == 100) {
            HandlePokemonAchievment();
        } else if (oldLevel >= 0 && newLevel < 0) {
            HandlePokemonDecline();
        } else if (oldLevel == -100 && newLevel == -100) {
            HandlePokemonFailure();
        } else {
            HandlePossibleEvolution();
        }	
    }

    private void HandlePokemonDecline() {
        EventManager.TriggerResetPokemon();
        EventManager.TriggerPokemonDecline();
    }

    private void HandlePokemonAchievment() {
        EventManager.TriggerResetPokemon();
        level = 0;
        EventManager.TriggerBroadcastLevel(level);
        starsAmount += 1;
        PlayerPrefs.SetInt(STARS_AMOUNT_SAVE_LABEL, starsAmount);
        SaveData();
        EventManager.TriggerBroadcastStarsAmount(starsAmount);
    }

    private void HandlePokemonFailure() {
        level = 0;
        EventManager.TriggerBroadcastLevel(level);
        dropsAmount += 1;
        PlayerPrefs.SetInt(DROPS_AMOUNT_SAVE_LABEL, dropsAmount);
        SaveData();
        EventManager.TriggerBroadcastDropsAmount(dropsAmount);
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

    private void SaveData() {
        PlayerPrefs.Save();
    }

    private void LoadData() {
        level = PlayerPrefs.GetInt(LEVEL_SAVE_LABEL);
        starsAmount = PlayerPrefs.GetInt(STARS_AMOUNT_SAVE_LABEL);
        dropsAmount = PlayerPrefs.GetInt(DROPS_AMOUNT_SAVE_LABEL);
        currentPokemonNumber = PlayerPrefs.GetInt(POKEMON_NUMBER_SAVE_LABEL);
        isShiny = PlayerPrefs.GetInt(IS_SHINY_SAVE_LABEL) == 1;
    }

    public void LoadMainScene() {
        SceneManager.LoadScene(MAIN_SCENE_NAME);
    }

    public void LoadGame() {
        LoadData();
        LoadMainScene();
    }

    private void InitializePokemonIfNeeded(Scene scene, LoadSceneMode loadSceneMode) {
        if (scene.name == MAIN_SCENE_NAME) {
            if (currentPokemonNumber >= 0) {
                if (currentPokemonNumber > 0) {
                    EventManager.TriggerGenerateTargetPokemonFromSaveData(currentPokemonNumber);
                }
                EventManager.TriggerBroadcastLevel(level, true);
                if (level < 0) {
                    EventManager.TriggerPokemonDecline();
                }

                EventManager.TriggerBroadcastStarsAmount(starsAmount, true);
                EventManager.TriggerBroadcastDropsAmount(dropsAmount, true);
                EventManager.TriggerBroadcastShinyInfo(isShiny);
            } else {
                PlayerPrefs.DeleteAll();
            }
        }
    }

}
