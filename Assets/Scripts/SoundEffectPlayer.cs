using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectPlayer : MonoBehaviour
{
    private AudioSource audioPlayer;

    [SerializeField] SoundChannelSO soundChannelSO;

    private void Awake()
    {
        //References
        audioPlayer = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {

        //Subscribe to events
        soundChannelSO.EPlaySound += PlayEffect;
    }

    private void OnDisable()
    {
        //Unsubscribe from events
        soundChannelSO.EPlaySound -= PlayEffect;
    }

    //Play the specified audio clip
    private void PlayEffect(AudioClip clip)
    {
        audioPlayer.PlayOneShot(clip);
    }
}