using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int level = 0;
    private int starsAmount = 0;

    

    void OnEnable() {
        EventManager.SelectDetailedCommandMenuItem += SelectDetailedCommandMenuItem;
    }

    void OnDisable() {
        EventManager.SelectDetailedCommandMenuItem -= SelectDetailedCommandMenuItem;
    }

  
    void SelectDetailedCommandMenuItem(int levelsAmount) {
        SetLevel(levelsAmount);
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
        // TODO : TriggerBroadcastStarsAmount
    }

    private void HandlePossibleEvolution() {
        // TODO

        // 	var niveaux_evol = evolution_infos_dict.keys()
	// 	var niveau_pertinent = 0
	// 	for niv in niveaux_evol:
	// 		if niv == null:
	// 			niveau_pertinent = 0
	// 		elif level >= niv:
	// 			niveau_pertinent = niv
	// 	print_debug(niveau_pertinent)

	// 	if (niveau_pertinent > 0):
	// 		evolution_pokemon_number = evolution_infos_dict.get(niveau_pertinent)
	// 		if evolution_pokemon_number != pokemon_number && _is_not_in_upper_evolution_tree():
	// 			pokemon_number = evolution_pokemon_number
	// 			is_evolving = true
	// 			_generate_targeted_pokemon(evolution_pokemon_number)
    }

}
