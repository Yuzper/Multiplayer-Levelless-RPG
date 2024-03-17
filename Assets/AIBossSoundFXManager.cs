using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * In the tutorials he actually makes a AIDurkSoundFXManager (that meaning a combat manager that is very specific to the boss)
 * Hence for each boss there would be a new class. However I just called this class "AIBossSoundFXManager".
 * It would be nice if we could make a base boss class that we can just tune in the editor or if we want a very specific boss
 * then we can just inherit from this class
 */
public class AIBossSoundFXManager : CharacterSoundFXManager
{
    [Header("Attack whooshes")]
    public AudioClip[] attackWhooshes;

    [Header("Attack Impacts")]
    public AudioClip[] attackImpacts;

    [Header("Stomp Impacks")]
    public AudioClip[] stompImpacts;

    public virtual void PlayWhoosh()
    {
        if (attackGrunts.Length > 0)
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(attackWhooshes));
    }
    public virtual void PlayAttackImpact()
    {
        if (attackImpacts.Length > 0)
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(attackImpacts));
    }
    public virtual void PlayStompImpact()
    {
        if (stompImpacts.Length > 0)
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(stompImpacts));
    }

}
