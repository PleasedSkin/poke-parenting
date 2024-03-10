using UnityEngine;

public class MainAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip pokemonDeclineSong;
    [SerializeField]
    private AudioClip defaultPokemonSong;

    void OnEnable() {
        EventManager.PokemonDecline += PlayPokemonDeclineSong;
        EventManager.PokemonRise += PlayDefaultPokemonSong;
    }

    void OnDisable() {
        EventManager.PokemonDecline -= PlayPokemonDeclineSong;
        EventManager.PokemonRise -= PlayDefaultPokemonSong;
    }

    private void PlayPokemonDeclineSong() {
        PlayClip(pokemonDeclineSong);
    }

    private void PlayDefaultPokemonSong() {
        PlayClip(defaultPokemonSong);
    }

    private void PlayClip(AudioClip clip) {
        audioSource.clip = clip;
        audioSource.Play();
    }

}
