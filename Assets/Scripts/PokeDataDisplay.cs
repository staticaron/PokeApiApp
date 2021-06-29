using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokeDataDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text pokemonIdText;
    [SerializeField] TMP_Text pokemonNameText;
    [SerializeField] RawImage pokemonFrontImage;
    [SerializeField] GameObject typeHolder;
    [SerializeField] TMP_Text pokemonHeightText;
    [SerializeField] TMP_Text pokemonWeightText;

    [Space]

    [SerializeField] DisplayDataChannelSO displayDataChannelSO;

    private void Awake()
    {
        displayDataChannelSO.displayDataEvent += DisplayData;
    }

    private void OnDisable()
    {
        displayDataChannelSO.displayDataEvent -= DisplayData;
    }

    public void DisplayData(PokemonData pokemonData)
    {
        pokemonIdText.text = pokemonData.id.ToString();
        pokemonNameText.text = pokemonData.name;
        pokemonFrontImage.texture = pokemonData.sprites;
        pokemonHeightText.text = pokemonData.height;
        pokemonWeightText.text = pokemonData.weight;
    }
}
