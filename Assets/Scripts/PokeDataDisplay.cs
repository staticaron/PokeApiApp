using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public void DisplayData(PokemonData pokemonData)
    {
        pokemonIdText.text = pokemonData.id.ToString();
        pokemonNameText.text = pokemonData.name;
        pokemonFrontImage.texture = pokemonData.sprites;
        pokemonHeightText.text = pokemonData.height;
        pokemonWeightText.text = pokemonData.weight;

        foreach (string typeName in pokemonData.types)
        {
            Debug.Log(typeName);
        }

        foreach (string typeName in pokemonData.types)
        {
            GameObject typeGO = typeDictionary[typeName];
            GameObject typeIns = Instantiate<GameObject>(typeGO);
            typeIns.transform.SetParent(typeHolder, false);
            Debug.Log($"{typeIns.name}", typeIns);
        }
    }
}
