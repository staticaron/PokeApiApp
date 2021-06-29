using UnityEngine;

[CreateAssetMenu(fileName = "DisplayDataChannelSO", menuName = "SO/DisplayDataChannelSO", order = 0)]
public class DisplayDataChannelSO : ScriptableObject
{
    public delegate void DisplayData(PokemonData data);
    public event DisplayData displayDataEvent;

    public void RaiseEvent(PokemonData data)
    {
        if (displayDataEvent != null)
        {
            displayDataEvent(data);
        }
        else
        {
            Debug.Log("DisplayDataEvent was called but no one was listening to it");
        }
    }
}