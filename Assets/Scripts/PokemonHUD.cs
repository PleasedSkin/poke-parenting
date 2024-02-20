using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonHUD : MonoBehaviour
{

    private static readonly string DEFAULT_POKEMON_NAME = "Oeuf";

    [SerializeField]
    private TextMeshProUGUI levelLabelComponent;

    [SerializeField]
    private TextMeshProUGUI nameLabelComponent;

    [SerializeField]
    private Image targetPokemonImage;

    [SerializeField]
    private Sprite defaultPokemonSprite;


    void OnEnable() {
        EventManager.BroadcastLevel += UpdateLevelLabel;
        EventManager.BroadcastName += UpdateNameLabel;
        EventManager.BroadcastPokemonSprite += UpdatePokemonSprite;
        EventManager.ResetPokemon += ResetPokemon;
    }


    private void UpdateLevelLabel(int level) {
        levelLabelComponent.SetText($"Niveau : {level}");
    }

    private void UpdateNameLabel(string name) {
        nameLabelComponent.SetText(name);
    }

    private void ResetPokemon() {
        UpdateNameLabel(DEFAULT_POKEMON_NAME);
        UpdatePokemonSprite(defaultPokemonSprite);
    }

    private void UpdatePokemonSprite(Sprite sprite) {
        targetPokemonImage.sprite = sprite;
    }


    void OnDisable() {
        EventManager.BroadcastLevel -= UpdateLevelLabel;
        EventManager.BroadcastName -= UpdateNameLabel;
        EventManager.BroadcastPokemonSprite -= UpdatePokemonSprite;
        EventManager.ResetPokemon -= ResetPokemon;
    }

}
