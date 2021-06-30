using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PokeDataDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text pokemonIdText;
    [SerializeField] TMP_Text pokemonNameText;
    [SerializeField] RawImage pokemonFrontImage;
    [SerializeField] RectTransform typeHolder;
    [SerializeField] TMP_Text pokemonHeightText;
    [SerializeField] TMP_Text pokemonWeightText;

    [Space]

    [SerializeField] List<GameObject> typesGO;
    [SerializeField] Dictionary<string, GameObject> typeDictionary = new Dictionary<string, GameObject>();

    [SerializeField] DisplayDataChannelSO displayDataChannelSO;

    private void Awake()
    {
        displayDataChannelSO.displayDataEvent += DisplayData;

        //Prepare Dictionary out of list
        MakeTypeDictionary(typesGO);
    }

    private void OnDisable()
    {
        displayDataChannelSO.displayDataEvent -= DisplayData;
    }

    private void MakeTypeDictionary(List<GameObject> typesGO)
    {
        foreach (GameObject g in typesGO)
        {
            typeDictionary.Add(g.name.ToLower(), g);
        }
    }

    [ContextMenu("Display Data")]
    private void DisplayData(PokemonData pokemonData)
    {
        //Reset older data by using empty PokemonData;
        ResetUiDisplay(new PokemonData());

        //Display the fetched Data
        ResetUiDisplay(pokemonData);
    }

    private void ResetUiDisplay(PokemonData data)
    {
        pokemonIdText.text = data.id;
        pokemonNameText.text = data.name;
        pokemonFrontImage.texture = data.sprites;
        pokemonHeightText.text = data.height;
        pokemonWeightText.text = data.weight;

        //If the data is empty then it doesn't run
        Debug.Log(data.types.Length);
        if (data.types.Length <= 0)
        {
            int n = 0;
            //Remove older data
            for (int i = 0; i < typeHolder.childCount - 1; i++)
            {
                n += 1;
            }
            Debug.Log($"there are {n} childs");
        }
        else
        {
            //Display the fetched type badges
            foreach (string typeName in data.types)
            {
                GameObject typeGO = typeDictionary[typeName];
                GameObject typeIns = Instantiate<GameObject>(typeGO);
                typeIns.transform.SetParent(typeHolder, false);
            }
        }
    }
}
