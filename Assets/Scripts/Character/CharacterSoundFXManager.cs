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

    public void PlayDeathSFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.deathSFX);
    }

    public void PlayDrawSwordSFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.DrawSwordSFX);
    }

    public void PlaySheathSwordSFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.SheathSwordSFX);
    }

    public void PlaySwordSwipeSFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.SwordSwipeSFX);
    }
    
}
