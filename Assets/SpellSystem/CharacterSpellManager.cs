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

    // List to store functions
    [SerializeField] public List<BaseSpell> spell_List;

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

    public void EquipMostLikelySpell(int index) {

        Debug.Log("Index: "+ index);

        if (index >= 0 && index < spell_List.Count)
        {
            equippedSpell = spell_List[index];
            Debug.Log("EQUIPTED SPELL" + equippedSpell);
        //    equippedSpell.UseSpell(character); ///////////////
        }
        else
        {
            Debug.LogError("Invalid index: " + index);
        }


    }


    public virtual void SpawnSpellRightHand()
    {
        if (equippedSpell != null)
        {
            var target = PlayerCamera.instance.player.playerCombatManager?.currentTarget?.characterCombatManager?.lockOnTransform;
            if (target)
            {
                PlayerUIManager.instance.playerUIHudManager.ToggleCrosshairOff(); // Turn off Crosshair
                Vector3 directionToWorldPointX = (target.position - rightHand.position).normalized;
                equippedSpell.SpawnSpell(this, rightHand, directionToWorldPointX);
            }
            else
            {
                PlayerUIManager.instance.playerUIHudManager.ToggleCrosshairOn(); // Turn on Crosshair

                Vector3 crosshairScreenPosition = PlayerUIManager.instance.playerUIHudManager.crosshair.transform.position;
                // Get the direction the camera is currently looking
                Camera mainCamera = PlayerCamera.instance.GetComponentInChildren<Camera>();
                Vector3 crosshairViewportPosition = mainCamera.ScreenToViewportPoint(crosshairScreenPosition);

                Ray ray = mainCamera.ViewportPointToRay(crosshairViewportPosition); // Points to the position of the UI crosshair element
                Vector3 cameraDirection = ray.direction;

                equippedSpell.SpawnSpell(this, rightHand, cameraDirection);
                //Vector3 cameraForward = PlayerCamera.instance.transform.forward;
                //equippedSpell.SpawnSpell(this, rightHand, cameraForward);
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
