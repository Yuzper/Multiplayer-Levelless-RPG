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
    [SerializeField] public BaseSpell equippedSpell2;

    // List to store functions
    public List<BaseSpell> spell_List;

    [Header("Spell casting")]
    public bool inSpellMode = false;
    public bool castSpell = false;
    public bool castSpellHold = false;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

        // Initialize the list
        spell_List = new List<BaseSpell>();
        spell_List.Add(equippedSpell);
        spell_List.Add(equippedSpell2);
        spell_List.Add(equippedSpell2);
    }

    public virtual void SpawnHandVFX()
    {
        equippedSpell.SpawnHandVFX(this);
    }

    public virtual void RemoveHandVFX()
    {
        if (rightHandVFX != null)
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

    public void CastMostLikelySpell(int index) {

        Debug.Log("Index: "+ index);

        // Invoke a function by index
        InvokeSpellByIndex(index); // Invokes SpellTwo

    }

    // Method to invoke a function by index
    private void InvokeSpellByIndex(int index)
    {
        if (index >= 0 && index < spell_List.Count)
        {
            spell_List[index].UseSpell(character);
        }
        else
        {
            Debug.LogError("Invalid index: " + index);
        }
    }

    public virtual void SpawnSpellOne()
    {
        if (equippedSpell != null)
        {
            var target = PlayerCamera.instance.player.playerCombatManager?.currentTarget?.characterCombatManager?.lockOnTransform;
            if (target)
            {
                Vector3 directionToWorldPointX = (PlayerCamera.instance.player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - rightHand.position).normalized;
                equippedSpell.SpawnSpell(this, rightHand, directionToWorldPointX);
            }
            else
            {
                equippedSpell.SpawnSpell(this, rightHand, character.gameObject.transform.forward);
            }
        }
    }

    public virtual void SpawnSpellTwo()
    {
        if (equippedSpell2 != null)
        {
            var target = PlayerCamera.instance.player.playerCombatManager?.currentTarget?.characterCombatManager?.lockOnTransform;
            if (target)
            {
                Vector3 directionToWorldPointX = (PlayerCamera.instance.player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - rightHand.position).normalized;
                equippedSpell2.SpawnSpell(this, rightHand, directionToWorldPointX);
            }
            else
            {
                equippedSpell2.SpawnSpell(this, rightHand, character.gameObject.transform.forward);
            }
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
