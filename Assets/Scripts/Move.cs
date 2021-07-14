using UnityEngine;
using TMPro;

public class Move : MonoBehaviour
{
    [SerializeField] TMP_Text moveNameField;
    [SerializeField] TMP_Text levelField;
    [SerializeField] TMP_Text methodField;

    public void UpdateUI(PokemonMove moveData)
    {
        moveNameField.text = moveData.moveName;
        levelField.text = moveData.levelLearnedAt.ToString();
        methodField.text = moveData.learnMethod;
    }
}