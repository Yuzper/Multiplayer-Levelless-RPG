using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class CharacterSpellManager : NetworkBehaviour
{
    public CharacterManager character;
    public Transform rightHand;
    public Transform leftHand;
    public Transform midPoint;
    public GameObject rightHandVFX;
    public GameObject leftHandVFX;

    [SerializeField] public BaseSpell equippedSpell;

    [Header("Spell casting")]
    public bool inSpellMode = false;
    public bool castSpell = false;
    public bool castSpellHold = false;

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

    public virtual void SpawnSpellRightHand()
    {
        if(equippedSpell != null)
        {
            var target = PlayerCamera.instance.player.playerCombatManager?.currentTarget?.characterCombatManager?.lockOnTransform;
            if (target)
            {
                Vector3 directionToWorldPointX = (PlayerCamera.instance.player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - rightHand.position).normalized;
                equippedSpell.SpawnSpell(this, rightHand, directionToWorldPointX);
            } else
            {
                equippedSpell.SpawnSpell(this, rightHand, character.gameObject.transform.forward);
            }
        }
    }

    public virtual void SpawnSpellLeftHand()
    {
        if (equippedSpell != null)
        {
            equippedSpell.SpawnSpell(this, leftHand, character.gameObject.transform.forward);
        }
    }

    public virtual void SpawnSpellMidpoint()
    {
        if (equippedSpell != null)
        {
            equippedSpell.SpawnSpell(this, midPoint, character.gameObject.transform.forward);
        }
    }

    public virtual void StopSpell()
    {
        if (equippedSpell != null)
        {
            equippedSpell.StopSpell();
        }
    }
}
