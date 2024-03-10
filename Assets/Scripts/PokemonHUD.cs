using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonHUD : MonoBehaviour
{

    private static readonly string DEFAULT_POKEMON_NAME = "Oeuf";
    private static readonly Color DEFAULT_COLOR = new Color(0.5843138f, 0.2392157f, 0.2392157f, 1);
    private static readonly Color DECLINE_COLOR = new Color(0.2f, 0.2f, 0.2f, 1);

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
    private TextMeshProUGUI dropsLabelComponent;

    [SerializeField]
    private Animator dropAnimator;

    [SerializeField]
    private Material whiteMaterial;

    [SerializeField]
    private Material starMaterial;

    [SerializeField]
    private Material evolutionMaterial;

    [SerializeField]
    private ParticleSystem customParticleSystem;

    [SerializeField]
    private Image canvasImage;


    private bool isPokemonShiny;

    private int currentLevel; 


    void OnEnable() {
        EventManager.BroadcastLevel += UpdateLevelLabel;
        EventManager.BroadcastName += UpdateNameLabel;
        EventManager.BroadcastPokemonSprite += UpdatePokemonSprite;
        EventManager.BroadcastStarsAmount += UpdateStarsLabel;
        EventManager.BroadcastDropsAmount += UpdateDropsLabel;
        EventManager.ResetPokemon += ResetPokemon;
        EventManager.DisplayLoadingSprite += DisplayLoadingSprite;
        EventManager.BroadcastShinyInfo += DisplayShinyParticles;
        EventManager.PokemonDecline += ReactToPokemonDecline;
        EventManager.PokemonRise += ReactToPokemonRise;
    }

    private void UpdateLevelLabel(int level, bool isLoadingDataContext) {
        levelLabelComponent.SetText($"Niveau : {level}");
        StartCoroutine(GiveJuiceToLevelChange(level, isLoadingDataContext));
    }

    private IEnumerator GiveJuiceToLevelChange(int newLevel, bool isLoadingDataContext) {
        const float duration = 0.3f;
        const float initialFontSize = 30;
        var initialColor = Color.white;
        if (!isLoadingDataContext) {
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

    private void UpdateStarsLabel(int starsAmount, bool isLoadingDataContext) {
        UpdateStarsOrDropsLabel(starsAmount, isLoadingDataContext, true);
    }

    private void UpdateDropsLabel(int dropsAmount, bool isLoadingDataContext) {
        UpdateStarsOrDropsLabel(dropsAmount, isLoadingDataContext, false);
    }

    private void UpdateStarsOrDropsLabel(int amount, bool isLoadingDataContext, bool isStarsRelated) {
        var text = $" : {amount}";
        var relevantComponent = isStarsRelated ? starsLabelComponent : dropsLabelComponent;
        relevantComponent.SetText(text);
        StartCoroutine(GiveJuiceToStarsOrDropsAmountChange(isLoadingDataContext, isStarsRelated));
    }

    private IEnumerator GiveJuiceToStarsOrDropsAmountChange(bool isLoadingDataContext, bool isStarsRelated) {
        const float duration = 0.3f;
        const float initialFontSize = 24;
        var relevantComponent = isStarsRelated ? starsLabelComponent : dropsLabelComponent;
        var relevantAnimator = isStarsRelated ? starAnimator : dropAnimator;
        var gainAnimation = isStarsRelated ? "StarGain" : "DropGain";
        if (!isLoadingDataContext) {
            relevantAnimator.Play(gainAnimation);
            relevantComponent.fontSize = 30;
            yield return new WaitForSeconds(duration);
            relevantComponent.fontSize = initialFontSize;
            yield return new WaitForSeconds(0.7f);
            relevantAnimator.Play("Idle");
        }
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

    private void ReactToPokemonDecline() {
        canvasImage.color = DECLINE_COLOR;
    }

    private void ReactToPokemonRise() {
        canvasImage.color = DEFAULT_COLOR;
    }


    void OnDisable() {
        EventManager.BroadcastLevel -= UpdateLevelLabel;
        EventManager.BroadcastName -= UpdateNameLabel;
        EventManager.BroadcastPokemonSprite -= UpdatePokemonSprite;
        EventManager.BroadcastStarsAmount -= UpdateStarsLabel;
        EventManager.BroadcastDropsAmount -= UpdateDropsLabel;
        EventManager.ResetPokemon -= ResetPokemon;
        EventManager.DisplayLoadingSprite -= DisplayLoadingSprite;
        EventManager.BroadcastShinyInfo -= DisplayShinyParticles;
        EventManager.PokemonDecline -= ReactToPokemonDecline;
        EventManager.PokemonRise -= ReactToPokemonRise;
    }

}
