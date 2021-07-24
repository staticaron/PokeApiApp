using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokeDataDisplay : MonoBehaviour
{
    [SerializeField] const string LabelTag = "Label";

    [Header("Fields")]
    [SerializeField] TMP_Text pokemonIdText;
    [SerializeField] TMP_Text pokemonNameText;
    [SerializeField] RawImage pokemonImage;
    [SerializeField] RectTransform typeHolder;
    [SerializeField] TMP_Text pokemonHeightText;
    [SerializeField] TMP_Text pokemonWeightText;
    [SerializeField] TMP_Text pokedexInfo;

    [SerializeField] RectTransform moveHolder;
    [SerializeField] RectTransform abilityHolder;
    [SerializeField] GameObject imageSwitchButtonHolder;

    [SerializeField, Space] GameObject moveTemplate;
    [SerializeField, Space] GameObject abilityTemplate;

    private PokemonData fetchedData;
    private bool isFrontSpriteDisplayed = true;

    [SerializeField] List<GameObject> typesGO;
    [SerializeField] Dictionary<string, GameObject> typeDictionary = new Dictionary<string, GameObject>();

    [SerializeField, Space] DisplayDataChannelSO displayDataChannelSO;

    private void Awake()
    {
        displayDataChannelSO.displayDataEvent += DisplayData;

        //Prepare Dictionary out of list
        MakeTypeDictionary(typesGO);
    }

    private void Start()
    {
        fetchedData = new PokemonData();
    }

    private void OnDisable()
    {
        displayDataChannelSO.displayDataEvent -= DisplayData;
    }

    //Makes a dictionary out of the list for faster object fetching
    private void MakeTypeDictionary(List<GameObject> typesGO)
    {
        foreach (GameObject g in typesGO)
        {
            typeDictionary.Add(g.name.ToLower(), g);
        }
    }

    //Display Data according to the pokemon data passed. If empty data is passed then reset the ui
    private void DisplayData(PokemonData pokemonData)
    {
        //Reset older data by using empty PokemonData;
        ResetUiDisplay(new PokemonData());

        //Display the fetched Data
        ResetUiDisplay(pokemonData);
    }

    private void ResetUiDisplay(PokemonData data)
    {
        fetchedData = data;

        pokemonIdText.text = fetchedData.id;
        pokemonNameText.text = fetchedData.name;
        pokemonImage.texture = fetchedData.frontSprite;
        pokemonHeightText.text = fetchedData.height;
        pokemonWeightText.text = fetchedData.weight;
        pokedexInfo.text = fetchedData.pokedexInfo;

        //Images
        if (fetchedData.backSprite == null)
        {
            imageSwitchButtonHolder.SetActive(false);
            fetchedData.backSprite = fetchedData.frontSprite;
        }
        else
        {
            imageSwitchButtonHolder.SetActive(true);
        }

        //If the pokemon data doesn't has any type badge then reset the older type badges
        if (fetchedData.types.Length <= 0)
        {
            for (int i = 0; i < typeHolder.childCount; i++)
            {
                GameObject child = typeHolder.GetChild(i).gameObject;

                //Ignore the labels
                if (child.tag == LabelTag) continue;

                GameObject.Destroy(child);
                Debug.Log("Removed the previous type badge");
            }
        }
        else
        {
            //Display the fetched type badges
            foreach (string typeName in fetchedData.types)
            {
                GameObject typeGO = typeDictionary[typeName];
                GameObject typeIns = Instantiate<GameObject>(typeGO);
                typeIns.transform.SetParent(typeHolder, false);
            }
        }

        //If the pokemon data doesn't has any move badges then reset the older move badges
        if (fetchedData.moves.Count <= 0)
        {
            RemoveChild(moveHolder);
        }
        else
        {
            foreach (PokemonMove m in fetchedData.moves)
            {
                GameObject moveIns = Instantiate(moveTemplate, Vector3.zero, Quaternion.identity);
                RectTransform rectT = moveIns.GetComponent<RectTransform>();
                rectT.SetParent(moveHolder);
                rectT.localScale = Vector3.one;
                moveIns.GetComponent<Move>().InitializeMove(m);
            }
        }

        //Abilities
        if (fetchedData.abilities.Count <= 0)
        {
            RemoveChild(abilityHolder);
        }
        else
        {
            foreach (PokemonAbility ability in fetchedData.abilities)
            {
                GameObject abilityIns = Instantiate(abilityTemplate, Vector3.zero, Quaternion.identity);
                RectTransform rectT = abilityIns.GetComponent<RectTransform>();
                rectT.SetParent(abilityHolder);
                rectT.localScale = Vector3.one;
                abilityIns.GetComponent<Ability>().InitializeAbility(ability);
            }
        }
    }

    [ContextMenu("Switch Sprite")]
    public void SwitchSprite()
    {
        if (isFrontSpriteDisplayed == true)
        {
            pokemonImage.texture = fetchedData.backSprite;
            isFrontSpriteDisplayed = false;
        }
        else
        {
            pokemonImage.texture = fetchedData.frontSprite;
            isFrontSpriteDisplayed = true;
        }
    }

    private void RemoveChild(RectTransform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject child = parent.GetChild(i).gameObject;

            //Ignore the labels
            if (child.tag == LabelTag) continue;

            GameObject.Destroy(child);
        }
    }
}
