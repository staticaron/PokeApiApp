using UnityEngine;
using TMPro;

public class Ability : MonoBehaviour
{
    [SerializeField] TMP_Text abilityNameField;
    [SerializeField] PokemonAbility ability;

    public void InitializeAbility(PokemonAbility abilityDetails)
    {
        abilityNameField.text = abilityDetails.abilityName;
        ability = abilityDetails;
    }
}
