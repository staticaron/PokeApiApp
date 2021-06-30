using UnityEngine;

public enum WarningType
{
    CONNECTION,
    NOTFOUND
}

[CreateAssetMenu(fileName = "WarningWindowChannelSO", menuName = "SO/WarningWindowChannelSO", order = 0)]
public class WarningWindowChannelSO : ScriptableObject
{
    public delegate void PopupWarningWindow(WarningType type);
    public event PopupWarningWindow PopupWarningWindowEvent;

    public void RaiseEvent(WarningType type)
    {
        if (PopupWarningWindowEvent != null)
        {
            PopupWarningWindowEvent(type);
        }
        else
        {
            Debug.LogWarning("PopupWarningWindowEvent was raised but no was listening to it");
        }
    }
}