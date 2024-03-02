using System.Collections;
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

    [SerializeField]
    private Material whiteMaterial;

    [SerializeField]
    private ParticleSystem shinyParticleSystem;

    private bool isPokemonShiny;


    void OnEnable() {
        EventManager.BroadcastLevel += UpdateLevelLabel;
        EventManager.BroadcastName += UpdateNameLabel;
        EventManager.BroadcastPokemonSprite += UpdatePokemonSprite;
        EventManager.BroadcastStarsAmount += UpdateStarsLabel;
        EventManager.ResetPokemon += ResetPokemon;
        EventManager.DisplayLoadingSprite += DisplayLoadingSprite;
        EventManager.BroadcastShinyInfo += DisplayShinyParticles;
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
        UpdatePokemonSprite(defaultPokemonSprite, false);
    }

    private void UpdatePokemonSprite(Sprite sprite, bool isEvolving) {
        if (isEvolving) {
            targetPokemonImage.material = whiteMaterial;
            StartCoroutine(EvolutionCoroutine(sprite));
        } else {
            loadingImage.gameObject.SetActive(false);
            targetPokemonImage.gameObject.SetActive(true);
            targetPokemonImage.sprite = sprite;
            if (isPokemonShiny) {
                shinyParticleSystem.Play();
            } else {
                shinyParticleSystem.Stop();
            }
        }
    }

    private void DisplayLoadingSprite() {
        targetPokemonImage.gameObject.SetActive(false);
        loadingImage.gameObject.SetActive(true);
    }

    private IEnumerator EvolutionCoroutine(Sprite evolutionSprite)
    {
        yield return new WaitForSeconds(1f);
        targetPokemonImage.material = null;
        targetPokemonImage.sprite = evolutionSprite;
    }

    private void DisplayShinyParticles(bool isShiny) {
        isPokemonShiny = isShiny;
    }


    void OnDisable() {
        EventManager.BroadcastLevel -= UpdateLevelLabel;
        EventManager.BroadcastName -= UpdateNameLabel;
        EventManager.BroadcastPokemonSprite -= UpdatePokemonSprite;
        EventManager.BroadcastStarsAmount -= UpdateStarsLabel;
        EventManager.ResetPokemon -= ResetPokemon;
        EventManager.DisplayLoadingSprite -= DisplayLoadingSprite;
        EventManager.BroadcastShinyInfo -= DisplayShinyParticles;
    }

}
