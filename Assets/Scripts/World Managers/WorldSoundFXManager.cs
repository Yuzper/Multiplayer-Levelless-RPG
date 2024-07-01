using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager instance;

    [Header("Boss Track")]
    [SerializeField] AudioSource bossIntroPlayer;
    [SerializeField] AudioSource bossLoopPlayer;

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

    public void PlayBossTrack(AudioClip introTrack, AudioClip loopTrack)
    {
        bossIntroPlayer.volume = 0.5f;
        bossIntroPlayer.clip = introTrack;
        bossIntroPlayer.loop = false;
        bossIntroPlayer.Play();

        bossLoopPlayer.volume = 0.5f;
        bossLoopPlayer.clip = loopTrack;
        bossLoopPlayer.loop = true;
        bossLoopPlayer.PlayDelayed(bossIntroPlayer.clip.length);
    }


    public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
    {
        if (array.Length == 0) return null;
        int index = Random.Range(0, array.Length);
        return array[index];
    }

    public AudioClip ChooseRandomFootstepSoundBasedOnGround(GameObject steppedOnObject, CharacterManager manager)
    {
        switch (steppedOnObject.tag)
        {
            case "Dirt":
                return ChooseRandomSFXFromArray(manager.characterSoundFXManager.footstepsDirt);
            case "Stone":
                return ChooseRandomSFXFromArray(manager.characterSoundFXManager.footstepsStone);
            default:
                return ChooseRandomSFXFromArray(manager.characterSoundFXManager.footstepsDefault);
        }
    }



    public void StopBossMusic()
    {
        StartCoroutine(FadeOutBossMusicThenStop());
    }

    private IEnumerator FadeOutBossMusicThenStop()
    {
        while(bossLoopPlayer.volume > 0)
        {
            bossLoopPlayer.volume -= Time.deltaTime;
            bossIntroPlayer.volume -= Time.deltaTime;
            yield return null;
        }


        bossIntroPlayer.Stop();
        bossLoopPlayer.Stop();
    }
}
