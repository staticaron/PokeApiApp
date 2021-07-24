using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using SimpleJSON;

public class PokedexLoader : MonoBehaviour
{
    private const string pokeApiBaseUrl = "https://pokeapi.co/api/v2/";

    private const int enLangIndex = 0;

    [SerializeField] DisplayDataChannelSO displayDataChannelSO;
    [SerializeField] LoadDataChannelSO loadDataChannelSO;
    [SerializeField] WarningWindowChannelSO warningWindowChannelSO;
    [SerializeField] SoundChannelSO soundChannelSO;

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
        string pokemonUrl = pokeApiBaseUrl + "pokemon/" + pokedexIndex.ToString();
        string pokemonSpeciesUrl = pokeApiBaseUrl + "pokemon-species/" + pokedexIndex.ToString();

        //Create A request to PokeApi based on the index
        UnityWebRequest pokemonRequest = UnityWebRequest.Get(pokemonUrl);
        UnityWebRequest speciesRequest = UnityWebRequest.Get(pokemonSpeciesUrl);

        //Check for any errors
        yield return pokemonRequest.SendWebRequest();

        if (pokemonRequest.result == UnityWebRequest.Result.ConnectionError || pokemonRequest.result == UnityWebRequest.Result.ProtocolError || pokemonRequest.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.LogWarning("Failed to get the Pokemon Data from the server \n Data failed to load");
            Debug.LogWarning($"URL tried was {pokemonUrl}");
            yield break;
        }

        //Get result from the request
        JSONNode jsonData = JSON.Parse(pokemonRequest.downloadHandler.text);

        //Store the data into the the PokemonData class
        PokemonData pokemonData = new PokemonData();

        //-------General
        pokemonData.id = jsonData["id"];
        pokemonData.name = jsonData["name"];

        //-------Sprite
        string frontSpriteUrl = jsonData["sprites"]["front_default"];

        UnityWebRequest spriteRequest = UnityWebRequestTexture.GetTexture(frontSpriteUrl);

        yield return spriteRequest.SendWebRequest();

        if (pokemonRequest.result == UnityWebRequest.Result.ConnectionError || pokemonRequest.result == UnityWebRequest.Result.ProtocolError || pokemonRequest.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.LogWarning("Failed to get the Pokemon Data from the server \n Image failed to load");
            yield break;
        }

        pokemonData.frontSprite = DownloadHandlerTexture.GetContent(spriteRequest);

        pokemonData.frontSprite.filterMode = FilterMode.Point;

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
        JSONNode jsonPokemonData = JSON.Parse(request.downloadHandler.text);

        PokemonData pokemonData = new PokemonData();

        //-------General
        pokemonData.id = jsonPokemonData["id"];
        pokemonData.name = jsonPokemonData["name"];

        //-------Sprite
        string frontSpriteUrl = jsonPokemonData["sprites"]["front_default"];
        string backSpriteUrl = jsonPokemonData["sprites"]["back_default"];

        if (frontSpriteUrl != null)
        {
            UnityWebRequest frontSpriteRequest = UnityWebRequestTexture.GetTexture(frontSpriteUrl);

            yield return frontSpriteRequest.SendWebRequest();

            if (frontSpriteRequest.result == UnityWebRequest.Result.ConnectionError || frontSpriteRequest.result == UnityWebRequest.Result.ProtocolError || frontSpriteRequest.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogWarning("Failed to get the Pokemon Data from the server \n Front Image failed to load");
                warningWindowChannelSO.RaiseEvent(WarningType.CONNECTION);
                yield break;
            }

            pokemonData.frontSprite = DownloadHandlerTexture.GetContent(frontSpriteRequest);
            pokemonData.frontSprite.filterMode = FilterMode.Point;
        }

        if (backSpriteUrl != null)
        {
            UnityWebRequest backSpriteRequest = UnityWebRequestTexture.GetTexture(backSpriteUrl);

            yield return backSpriteRequest.SendWebRequest();

            if (backSpriteRequest.result == UnityWebRequest.Result.ConnectionError || backSpriteRequest.result == UnityWebRequest.Result.ProtocolError || backSpriteRequest.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogWarning("Failed to get the Pokemon Data from the server \n Back Image failed to load");
                warningWindowChannelSO.RaiseEvent(WarningType.CONNECTION);
                yield break;
            }

            pokemonData.backSprite = DownloadHandlerTexture.GetContent(backSpriteRequest);
            pokemonData.backSprite.filterMode = FilterMode.Point;
        }

        //-------Type
        JSONNode types = jsonPokemonData["types"];
        string[] pokeTypesName = new string[types.Count];

        for (int i = pokeTypesName.Length - 1; i >= 0; i--)
        {
            pokeTypesName[types.Count - i - 1] = types[i]["type"]["name"];
        }

        pokemonData.types = pokeTypesName;

        //------Abilities
        JSONNode abilitiesInfo = jsonPokemonData["abilities"];

        for (int i = 0; i < abilitiesInfo.Count; i++)
        {
            PokemonAbility newAbility = new PokemonAbility();
            newAbility.abilityName = abilitiesInfo[i]["ability"]["name"];
            //TODO : Get the ability details.
            newAbility.isHidden = abilitiesInfo[i]["is_hidden"];

            pokemonData.abilities.Add(newAbility);
        }

        //------Height and Weight
        pokemonData.height = jsonPokemonData["height"];
        pokemonData.weight = jsonPokemonData["weight"];

        //------Moves
        JSONNode moves = jsonPokemonData["moves"];

        for (int i = 0; i < moves.Count; i++)
        {

            string moveName = moves[i]["move"]["name"];
            int levelLearnedAt = moves[i]["version_group_details"][0]["level_learned_at"];

            string method = moves[i]["version_group_details"][0]["move_learn_method"]["name"];

            PokemonMove move = new PokemonMove(moveName, levelLearnedAt, method);

            pokemonData.moves.Add(move);

        }

        //------PokemonInfo
        string pokemonSpeciesUrl = pokeApiBaseUrl + "pokemon-species/" + pokemonData.id.ToString();
        UnityWebRequest speciesRequest = UnityWebRequest.Get(pokemonSpeciesUrl);

        yield return speciesRequest.SendWebRequest();

        if (speciesRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogWarning("Failed to get the Pokemon Data from the server \n Connection Error");
            warningWindowChannelSO.RaiseEvent(WarningType.CONNECTION);
            yield break;
        }
        else if (speciesRequest.result == UnityWebRequest.Result.ProtocolError || speciesRequest.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.LogWarning("Failed to get the Pokemon Data from the server \n Pokemon not found");
            warningWindowChannelSO.RaiseEvent(WarningType.NOTFOUND);
            yield break;
        }

        JSONNode jsonSpeciesData = JSON.Parse(speciesRequest.downloadHandler.text);

        JSONNode info = jsonSpeciesData["flavor_text_entries"];
        for (int i = 0; i < info.Count; i++)
        {
            string lang = info[i]["language"]["name"];
            if (lang == "en")
            {
                pokemonData.pokedexInfo = info[i]["flavor_text"];
            }
        }

        PrintData(pokemonData);

        //Raise Event
        displayDataChannelSO.RaiseEvent(pokemonData);

        //Play Loaded Sound Effects
        soundChannelSO.RaiseEvent(SoundEffectType.SEARCH_COMPLETE);
    }

    //Logs the data fetched from the server
    protected void PrintData(PokemonData pokemonData)
    {
        Debug.Log($"Pokemon ID is {pokemonData.id}");
        Debug.Log($"Pokemon name is {pokemonData.name}");

        foreach (PokemonAbility ability in pokemonData.abilities)
        {
            string hiddenData = ability.isHidden == true ? "Hidden" : "Not Hidden";

            Debug.Log($"Ability Name is {ability.abilityName} and it is {hiddenData} by default");
        }
    }
}