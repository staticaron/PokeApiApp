using UnityEngine;

[CreateAssetMenu(fileName = "LoadDataChannelSO", menuName = "SO/LoadDataChannelSO", order = 0)]
public class LoadDataChannelSO : ScriptableObject
{
    public delegate void LoadData(string inputName);
    public event LoadData loadDataEvent;

    public void RaiseEvent(string inputName)
    {
        if (loadDataEvent != null)
        {
            loadDataEvent(inputName);
        }
        else
        {
            Debug.LogWarning("LoadDataEvent was raised but no was listening to it");
        }
    }
}