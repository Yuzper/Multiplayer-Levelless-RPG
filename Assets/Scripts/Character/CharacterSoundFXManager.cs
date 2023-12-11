using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    private AudioSource audioSource;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        //PlayBackgroundMusic(); // Plays background music
    }

    public void PlayBackgroundMusic()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.BackgroundMusic);
    }

    public void PlayRollSoundFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX);
    }


}
