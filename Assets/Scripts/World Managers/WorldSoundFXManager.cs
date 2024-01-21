using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager instance;

    [Header("Damage Sounds")]
    public AudioClip[] physicalDamageSFX;

    [Header("Action Sounds")]
    public AudioClip rollSFX;
    public AudioClip deathSFX;

    [Header("Weapon Sounds")]
    public AudioClip[] DrawSwordSFX;
    public AudioClip[] SheathSwordSFX;
    public AudioClip[] SwordSwipeSFX;

    [Header("Background Sounds")]
    public AudioClip BackgroundMusic;

    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
    {
        int index = Random.Range(0, array.Length);
        return array[index];
    }
}
