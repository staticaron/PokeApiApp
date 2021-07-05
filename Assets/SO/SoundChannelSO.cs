using UnityEngine;

[CreateAssetMenu(fileName = "SoundChannelSO", menuName = "SO/SoundChannelSO", order = 0)]
public class SoundChannelSO : ScriptableObject
{
    [SerializeField] AudioClip buttonClickEffect;
    [SerializeField] AudioClip searchCompleteEffect;
    [SerializeField] AudioClip errorEffect;

    public delegate void PlaySound(AudioClip clip);
    public event PlaySound EPlaySound;

    public void RaiseEvent(AudioClip clipToPlay)
    {
        if (EPlaySound != null)
        {
            EPlaySound(clipToPlay);
            Debug.Log($"Effect Played named {clipToPlay.name}");
        }
        else
        {
            Debug.LogWarning("EPlaySound was raised but no one was listening to it");
        }
    }

    public void RaiseEvent(SoundEffectType type)
    {
        if (EPlaySound != null)
        {
            EPlaySound(GetClipFromType(type));
            Debug.Log($"Effect Played named {type}");
        }
        else
        {
            Debug.LogWarning("EPlaySound was raised but no one was listening to it");
        }
    }

    private AudioClip GetClipFromType(SoundEffectType type)
    {
        switch (type)
        {
            case SoundEffectType.BUTTON_CLICK:
                return buttonClickEffect;
            case SoundEffectType.SEARCH_COMPLETE:
                return searchCompleteEffect;
            case SoundEffectType.ERROR:
                return errorEffect;
            default:
                Debug.LogWarning("Error the audio clip requested doesnot exist");
                return null;
        }
    }

}