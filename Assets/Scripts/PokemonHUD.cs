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
    private Animator starAnimator;

    [SerializeField]
    private Material whiteMaterial;

    [SerializeField]
    private Material starMaterial;

    [SerializeField]
    private Material evolutionMaterial;

    [SerializeField]
    private ParticleSystem customParticleSystem;

    private bool isPokemonShiny;

    private int currentLevel = -1; 
    private int currentStarsAmount = -1; 


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
        StartCoroutine(GiveJuiceToLevelChange(level));
    }

    private IEnumerator GiveJuiceToLevelChange(int newLevel) {
        const float duration = 0.3f;
        const float initialFontSize = 30;
        var initialColor = Color.white;
        if (currentLevel != -1) {
            if (newLevel > currentLevel) {
                levelLabelComponent.fontSize = 35;
                levelLabelComponent.color = new Color(0.2080649f, 0.4716f, 0.1935f, 1);
                yield return new WaitForSeconds(duration);
                levelLabelComponent.fontSize = initialFontSize;
                levelLabelComponent.color = initialColor;
            } else if (newLevel < currentLevel) {
                levelLabelComponent.fontSize = 25;
                levelLabelComponent.color = new Color(0.7169812f, 0.1907295f, 0.1589f, 1);
                yield return new WaitForSeconds(duration);
                levelLabelComponent.fontSize = initialFontSize;
                levelLabelComponent.color = initialColor;
            }
        }
        currentLevel = newLevel;
        yield return null;
    }

    private void UpdateNameLabel(string name) {
        nameLabelComponent.SetText(name);
    }

    private void UpdateStarsLabel(int starsAmount) {
        starsLabelComponent.SetText($" : {starsAmount}");
        StartCoroutine(GiveJuiceToStarsAmountChange(starsAmount));
    }

    private IEnumerator GiveJuiceToStarsAmountChange(int newStarsAmount) {
        const float duration = 0.3f;
        const float initialFontSize = 24;
        if (currentStarsAmount != -1) {
            starAnimator.Play("StarGain");
            starsLabelComponent.fontSize = 30;
            yield return new WaitForSeconds(duration);
            starsLabelComponent.fontSize = initialFontSize;
            yield return new WaitForSeconds(0.7f);
            starAnimator.Play("Idle");
        }
        currentStarsAmount = newStarsAmount;
        yield return null;
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
            PlayShinyPokemonParticlesIfRequired();
        }
    }

    private void PlayShinyPokemonParticlesIfRequired() {
        if (isPokemonShiny) {
            ChangeParticleSystemRendererMaterial(starMaterial);
            customParticleSystem.Play();
        } else {
            customParticleSystem.Stop();
        }
    }

    private void ChangeParticleSystemRendererMaterial(Material targetMaterial) {
        customParticleSystem.GetComponent<ParticleSystemRenderer>().material = targetMaterial;
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
        StartCoroutine(EvolutionFinishedCoroutine());
    }

    private IEnumerator EvolutionFinishedCoroutine()
    {
        ChangeParticleSystemRendererMaterial(evolutionMaterial);
        customParticleSystem.Play();
        yield return new WaitForSeconds(2f);
        PlayShinyPokemonParticlesIfRequired();

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
