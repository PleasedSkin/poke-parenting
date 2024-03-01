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
    private Image loadingImage;

    [SerializeField]
    private Sprite defaultPokemonSprite;

    [SerializeField]
    private TextMeshProUGUI starsLabelComponent;


    void OnEnable() {
        EventManager.BroadcastLevel += UpdateLevelLabel;
        EventManager.BroadcastName += UpdateNameLabel;
        EventManager.BroadcastPokemonSprite += UpdatePokemonSprite;
        EventManager.BroadcastStarsAmount += UpdateStarsLabel;
        EventManager.ResetPokemon += ResetPokemon;
        EventManager.DisplayLoadingSprite += DisplayLoadingSprite;
    }


    private void UpdateLevelLabel(int level) {
        levelLabelComponent.SetText($"Niveau : {level}");
    }

    private void UpdateNameLabel(string name) {
        nameLabelComponent.SetText(name);
    }

    private void UpdateStarsLabel(int starsAmount) {
        starsLabelComponent.SetText($" : {starsAmount}");
    }

    private void ResetPokemon() {
        UpdateNameLabel(DEFAULT_POKEMON_NAME);
        UpdatePokemonSprite(defaultPokemonSprite);
    }

    private void UpdatePokemonSprite(Sprite sprite) {
        loadingImage.gameObject.SetActive(false);
        targetPokemonImage.gameObject.SetActive(true);
        targetPokemonImage.sprite = sprite;
    }

    private void DisplayLoadingSprite() {
        targetPokemonImage.gameObject.SetActive(false);
        loadingImage.gameObject.SetActive(true);
    }


    void OnDisable() {
        EventManager.BroadcastLevel -= UpdateLevelLabel;
        EventManager.BroadcastName -= UpdateNameLabel;
        EventManager.BroadcastPokemonSprite -= UpdatePokemonSprite;
        EventManager.BroadcastStarsAmount -= UpdateStarsLabel;
        EventManager.ResetPokemon -= ResetPokemon;
        EventManager.DisplayLoadingSprite -= DisplayLoadingSprite;
    }

}
