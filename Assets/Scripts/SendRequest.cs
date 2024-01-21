using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SendRequest : MonoBehaviour
{

    [SerializeField]
    private string apiUrl;

    [SerializeField]
    private Image targetImage;

    [SerializeField]
    private TextMeshProUGUI targetNameTextArea;


    // Start is called before the first frame update
    void Start()
    {
        TriggerPokemonSearch();
    }


    public void TriggerPokemonSearch()
    {
        StartCoroutine(GetPokemon());
    }


    private IEnumerator GetPokemon() 
    {
        var rnd = new System.Random();
        int randomPokemonNumber  = rnd.Next(1, 1026);
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl + "pokemon/" + randomPokemonNumber.ToString()))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                Pokemon pokemon = JsonUtility.FromJson<Pokemon>(request.downloadHandler.text);
                // Debug.Log($"Mon pokémon est {pokemon?.name}, il mesure {pokemon?.height} et pèse {pokemon?.weight}");
                StartCoroutine(GetPokemonFrenchName(randomPokemonNumber));
                StartCoroutine(LoadImage(pokemon.sprites.front_default));
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
                this.targetNameTextArea.text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(correspondingName.name);
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
                targetImage.sprite = newSprite;
            } else 
            {
                Debug.LogError("Une erreur est survenue");
            }
        }
    }


}

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
}



[System.Serializable]
public class PokemonSpecies
{
    public Name[] names;
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