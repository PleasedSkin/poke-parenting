using UnityEngine;

public class MainAudio : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip pokemonDeclineSong;

    void OnEnable() {
        EventManager.PokemonDecline += PlayPokemonDeclineSong;
    }

    void OnDisable() {
        EventManager.PokemonDecline -= PlayPokemonDeclineSong;
    }

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }


    private void PlayPokemonDeclineSong() {
        audioSource.clip = pokemonDeclineSong;
        audioSource.Play();
    }
}
