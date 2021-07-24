using UnityEngine;

public enum SoundEffectType
{
    BUTTON_CLICK,
    SEARCH_COMPLETE,
    ERROR
}

public class UISoundManager : MonoBehaviour
{
    [SerializeField] AudioClip buttonClickEffect;
    [SerializeField] AudioClip searchResultShownEffect;

    [Space]

    [SerializeField] SoundChannelSO soundChannelSO;

    public void PlayEffect(AudioClip clip)
    {
        soundChannelSO.RaiseEvent(clip);
    }
}