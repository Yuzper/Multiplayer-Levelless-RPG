using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Damage Grunts")]
    [SerializeField] protected AudioClip[] damageGrunts;

    [Header("Damage Grunts")]
    [SerializeField] protected AudioClip[] attackGrunts;

    [Header("Footsteps")]
    [SerializeField] public AudioClip[] footstepsDefault;
    [SerializeField] public AudioClip[] footstepsDirt;
    [SerializeField] public AudioClip[] footstepsStone;


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
        PlaySoundFX(WorldSoundFXManager.instance.rollSFX);
    }

    public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f)
    {
        if (soundFX == null) return;
        audioSource.PlayOneShot(soundFX, volume);
        // RESETS PITCH
        audioSource.pitch = 1;

        if (randomizePitch)
        {
            audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
        }
    }




    public void PlayDeathSFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.deathSFX);
    }

    public void PlayDrawSwordSFX()
    {
        AudioClip drawSwordSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.DrawSwordSFX);
        PlaySoundFX(drawSwordSFX);
    }

    public void PlaySheathSwordSFX()
    {
        AudioClip sheathSwordSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.SheathSwordSFX);
        PlaySoundFX(sheathSwordSFX);
    }

    public void PlaySwordSwipeSFX()
    {
        AudioClip swordSwipeSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.SwordSwipeSFX);
        PlaySoundFX(swordSwipeSFX);
    }
    
    public virtual void PlayDamageGrunt()
    {
        if(damageGrunts.Length > 0)
        {
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(damageGrunts));
        }
        
    }

    public virtual void PlayAttackGrunt()
    {
        if (attackGrunts.Length > 0)
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(attackGrunts));
    }

    public virtual void PlayFootstep()
    {
        if (footstepsDefault.Length > 0)
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(footstepsDefault));
    }
}
