using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class CharacterSpellManager : NetworkBehaviour
{
    public CharacterManager character;
    public Transform rightHand;
    public Transform leftHand;
    public GameObject rightHandVFX;
    public GameObject leftHandVFX;

    [SerializeField] protected BaseSpell equippedSpell;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void SpawnHandVFX()
    {
        equippedSpell.SpawnHandVFX(this);
    }

    public virtual void RemoveHandVFX()
    {
        if(rightHandVFX != null)
        {
            Destroy(rightHandVFX);
            rightHandVFX = null;
        }
        if (leftHandVFX != null)
        {
            Destroy(leftHandVFX);
            leftHandVFX = null;
        }

    }

    public virtual void SpawnSpell()
    {
        if(equippedSpell != null)
        {
            equippedSpell.SpawnSpell(this, rightHand, character.gameObject.transform.forward);
        }
    }
}
