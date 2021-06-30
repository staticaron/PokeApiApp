using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using SimpleJSON;

public class PokemonData
{
    public string id { get; set; }
    public string name { get; set; }
    public string[] types { get; set; }
    public Texture2D sprites { get; set; }
    public string height { get; set; }
    public string weight { get; set; }

    public PokemonData(string id = "0", string name = "", string[] types = null, Texture2D sprite = null, string height = "", string weight = "")
    {
        this.id = id;
        this.name = name;
        this.types = types == null ? new string[0] : types;
        this.sprites = sprite;
        this.height = height;
        this.weight = weight;
    }
}

public class PokedexLoader : MonoBehaviour
{
    private const string pokeApiBaseUrl = "https://pokeapi.co/api/v2/";

    [SerializeField] DisplayDataChannelSO displayDataChannelSO;
    [SerializeField] LoadDataChannelSO loadDataChannelSO;
    [SerializeField] WarningWindowChannelSO warningWindowChannelSO;

    private void Awake()
    {
        loadDataChannelSO.loadDataEvent += GetData;
    }

    private void OnDisable()
    {
        loadDataChannelSO.loadDataEvent -= GetData;
    }

    //Get Data from the server
    public void GetData(string pokemonName)
    {
        StartCoroutine(GetPokemonData(pokemonName));
    }

    //Get the pokemon data by index
    protected IEnumerator<YieldInstruction> GetPokemonData(int pokedexIndex)
    {
        string url = pokeApiBaseUrl + "pokemon/" + pokedexIndex.ToString();

        //Create A request to PokeApi based on the index
        UnityWebRequest request = UnityWebRequest.Get(url);

        //Check for any errors
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.LogWarning("Failed to get the Pokemon Data from the server \n Data failed to load");
            yield break;
        }

        //Get result from the request
        JSONNode jsonData = JSON.Parse(request.downloadHandler.text);

        //Store the data into the the PokemonData class
        PokemonData pokemonData = new PokemonData();

        //-------General
        pokemonData.id = jsonData["id"];
        pokemonData.name = jsonData["name"];

        //-------Sprite
        string frontSpriteUrl = jsonData["sprites"]["front_default"];

        UnityWebRequest spriteRequest = UnityWebRequestTexture.GetTexture(frontSpriteUrl);

        yield return spriteRequest.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.LogWarning("Failed to get the Pokemon Data from the server \n Image failed to load");
            yield break;
        }

        pokemonData.sprites = DownloadHandlerTexture.GetContent(spriteRequest);

        pokemonData.sprites.filterMode = FilterMode.Point;

        //-------Type
        JSONNode types = jsonData["types"];
        string[] pokeTypesName = new string[types.Count];

        for (int i = pokeTypesName.Length - 1; i >= 0; i--)
        {
            pokeTypesName[types.Count - i - 1] = types[i]["type"]["name"];
        }

        pokemonData.types = pokeTypesName;

        PrintData(pokemonData);

        //Raise Event
        displayDataChannelSO.RaiseEvent(pokemonData);
    }

    //Get pokemon data by name
    protected IEnumerator<YieldInstruction> GetPokemonData(string pokemonName)
    {
        string url = pokeApiBaseUrl + "pokemon/" + pokemonName.ToString();

        //Create A request to PokeApi based on the index
        UnityWebRequest request = UnityWebRequest.Get(url);

        //Check for any errors
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogWarning("Failed to get the Pokemon Data from the server \n Connection Error");
            warningWindowChannelSO.RaiseEvent(WarningType.CONNECTION);
            yield break;
        }
        else if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.LogWarning("Failed to get the Pokemon Data from the server \n Pokemon not found");
            warningWindowChannelSO.RaiseEvent(WarningType.NOTFOUND);
            yield break;
        }

        //Get result from the request
        JSONNode jsonData = JSON.Parse(request.downloadHandler.text);

        //Store the data into the the PokemonData class
        PokemonData pokemonData = new PokemonData();

        //-------General
        pokemonData.id = jsonData["id"];
        pokemonData.name = jsonData["name"];

        //-------Sprite
        string frontSpriteUrl = jsonData["sprites"]["front_default"];

        UnityWebRequest spriteRequest = UnityWebRequestTexture.GetTexture(frontSpriteUrl);

        yield return spriteRequest.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.LogWarning("Failed to get the Pokemon Data from the server \n Image failed to load");
            warningWindowChannelSO.RaiseEvent(WarningType.CONNECTION);
            yield break;
        }

        pokemonData.sprites = DownloadHandlerTexture.GetContent(spriteRequest);

        pokemonData.sprites.filterMode = FilterMode.Point;

        //-------Type
        JSONNode types = jsonData["types"];
        string[] pokeTypesName = new string[types.Count];

        for (int i = pokeTypesName.Length - 1; i >= 0; i--)
        {
            pokeTypesName[types.Count - i - 1] = types[i]["type"]["name"];
        }

        pokemonData.types = pokeTypesName;

        //Height and Weight
        pokemonData.height = jsonData["height"];
        pokemonData.weight = jsonData["weight"];

        PrintData(pokemonData);

        //Raise Event
        displayDataChannelSO.RaiseEvent(pokemonData);
    }

    //Logs the data fetched from the server
    protected void PrintData(PokemonData pokemonData)
    {
        Debug.Log(pokemonData.id);
        Debug.Log(pokemonData.name);
        for (int i = 0; i < pokemonData.types.Length; i++)
        {
            Debug.Log(pokemonData.types[i]);
        }
    }
}
