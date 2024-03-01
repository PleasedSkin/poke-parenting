using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;

public class SendRequest : MonoBehaviour
{

    [SerializeField]
    private string apiUrl;

    private int currentPokemonNumber;

    private bool isShiny = false;

    private Dictionary<int, int> evolutionDictionary = new Dictionary<int, int>();


    void OnEnable() {
        EventManager.GenerateNewPokemon += TriggerPokemonSearch;
        EventManager.GeneratePokemonEvolution += TriggerPokemonEvolution;
    }

    void OnDisable() {
        EventManager.GenerateNewPokemon -= TriggerPokemonSearch;
        EventManager.GeneratePokemonEvolution -= TriggerPokemonEvolution;
    }


    private void TriggerPokemonSearch()
    {
        StartCoroutine(GetRandomPokemon());
    }

    private void TriggerPokemonEvolution(int targetPokemonNumber)
    {
        StartCoroutine(GetSpecificPokemon(targetPokemonNumber, true));
    }


    private IEnumerator GetRandomPokemon() 
    {
        var rnd = new System.Random();
        currentPokemonNumber  = rnd.Next(1, 1026);
        EventManager.TriggerBroadcastPokemonNumber(currentPokemonNumber);
        return GetSpecificPokemon(currentPokemonNumber);
    }

    private void HandleLoading(bool isEvolving) {
        EventManager.TriggerBroadcastName(isEvolving ? "Il évolue !" :  "?");
        EventManager.TriggerDisplayLoadingSprite();
    }

    private IEnumerator GetSpecificPokemon(int pokemonId, bool isEvolving = false) 
    {

        evolutionDictionary = new Dictionary<int, int>();
        EventManager.TriggerBroadcastEvolutionDictionary(evolutionDictionary);

        if (!isEvolving) {
            var rnd = new System.Random();
            isShiny = rnd.Next(1, 101) % 100 == 0;
        }

        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl + "pokemon/" + pokemonId.ToString()))
        {
            HandleLoading(isEvolving);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                Pokemon pokemon = JsonUtility.FromJson<Pokemon>(request.downloadHandler.text);
                // Debug.Log($"Mon pokémon est {pokemon?.name}, il mesure {pokemon?.height} et pèse {pokemon?.weight}");
                StartCoroutine(GetPokemonFrenchName(pokemonId));
                StartCoroutine(LoadImage(isShiny ? pokemon.sprites.front_shiny : pokemon.sprites.front_default));
            } else 
            {
                Debug.LogError("Une erreur est survenue");
            }
        }
    }

    private IEnumerator GetPokemonFrenchName(int pokemonNumber) 
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl + "pokemon-species/" + pokemonNumber.ToString()))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                PokemonSpecies pokemonSpecies = JsonUtility.FromJson<PokemonSpecies>(request.downloadHandler.text);
                var correspondingName = pokemonSpecies.names.FirstOrDefault(name => name.language.name == "fr");
                EventManager.TriggerBroadcastName(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(correspondingName.name));

                var evolutionChainUrl = pokemonSpecies.evolution_chain.url;
                StartCoroutine(GetPokemonEvolutionInfos(evolutionChainUrl));

            } else 
            {
                Debug.LogError("Une erreur est survenue");
            }
        }
    }

    private IEnumerator LoadImage(string url) 
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D myTexture = ((DownloadHandlerTexture) request.downloadHandler).texture;
                Sprite newSprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
                newSprite.texture.filterMode = FilterMode.Point;
                EventManager.TriggerBroadcastPokemonSprite(newSprite);
            } else 
            {
                Debug.LogError("Une erreur est survenue");
            }
        }
    }


    private IEnumerator GetPokemonEvolutionInfos(string url) {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                EvolutionInfos evolutionInfos = JsonUtility.FromJson<EvolutionInfos>(request.downloadHandler.text);
                var evolvesTo = evolutionInfos.chain.evolves_to;

                if (evolvesTo.Length == 0) {
                    evolutionDictionary = new Dictionary<int, int>();
                } else {
                    while (evolvesTo.Length > 0) {
                        var relevantEvolutionDetails = GetRelevantEvolutionDetails(evolvesTo);
                        if (relevantEvolutionDetails.Length == 0) {
                            break;
                        }
                        var minLevelToEvolve = relevantEvolutionDetails[0].min_level;
                        int targetPokemonNumner = int.Parse(evolvesTo[0].species.url.Substring((apiUrl + "pokemon-species/").Length).Replace("/", ""));
                        evolutionDictionary.Add(minLevelToEvolve, targetPokemonNumner);
                        evolvesTo = evolvesTo[0].evolves_to;
                    }
                }

                EventManager.TriggerBroadcastEvolutionDictionary(evolutionDictionary);

            } else 
            {
                Debug.LogError("Une erreur est survenue lors de la requête de recherche des infos d'évolution");
            }
        }
    }


    
    private EvolutionDetails[] GetRelevantEvolutionDetails(EvolvesTo[] evolvesToArray) {
        return evolvesToArray[0].evolution_details.Where(ed => ed.trigger.name == "level-up").ToArray();
    }


}





// TODO : faire en sorte que ce soit + propre => déjà en mettant tout en private ou en isolant dans un autre fichier ou 1 class par fichier dans répertoire models

[System.Serializable]
public class Pokemon
{
    public string name;
    public string height;
    public string weight;
    public Sprites sprites;
}

[System.Serializable]
public class Sprites
{
    public string front_default;
    public string front_shiny;
}



[System.Serializable]
public class PokemonSpecies
{
    public Name[] names;
    public EvolutionChainMeta evolution_chain;
    public string url;
}

[System.Serializable]
public class Name
{
    public string name;
    public Language language; 
}

[System.Serializable]
public class Language
{
    public string name;
    public string url; 
}

[System.Serializable]
public class EvolutionChainMeta {
    public string url;
}

[System.Serializable]
public class EvolutionInfos {
    public EvolutionChain chain;
}

[System.Serializable]
public class EvolutionChain {
    public EvolvesTo[] evolves_to;
}

[System.Serializable]
[DataContract(IsReference=true)]
public class EvolvesTo {
    [DataMember]
    public EvolvesTo[] evolves_to;
    public EvolutionDetails[] evolution_details;
    public PokemonSpecies species;
}

[System.Serializable]
public class EvolutionDetails {
    public int min_level;
    public EvolutionTrigger trigger;
}

[System.Serializable]
public class EvolutionTrigger {
    public string name;
}