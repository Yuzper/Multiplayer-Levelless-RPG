using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVFXManager : MonoBehaviour
{
    CharacterManager character;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    // Landing dust
    public void PlayLandingDustVFXInAnimation()
    {
        WorldVFXManager.instance.PlayLandingDustVFX(character.transform.position, Quaternion.Euler(90f, 0f, 0f));
    }


}
