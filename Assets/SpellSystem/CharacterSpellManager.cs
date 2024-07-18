using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;


public class CharacterSpellManager : NetworkBehaviour
{
    public CharacterManager character;
    public Transform rightHand;
    public Transform leftHand;
    public Transform midPoint;
    public GameObject rightHandVFX;
    public GameObject leftHandVFX;


    [SerializeField] private Sprite nullSpellImage;

    // List to store functions
    [SerializeField] public List<BaseSpell> spell_List;
    public BaseSpell equippedSpell;

    [Header("Spell casting")]
    public bool inDrawingMode = false;
    public bool castSpell = false;
    public bool castSpellHold = false;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Update()
    {

    }

    public virtual void SpawnHandVFX()
    {
        if (equippedSpell != null)
        {
            equippedSpell.SpawnHandVFX(this);
        }
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
            ((PlayerNetworkManager)character.characterNetworkManager).NotifyTheServerOfSpellEquipServerRpc(NetworkManager.Singleton.LocalClientId,index);
            PlayerUIManager.instance.playerUIHudManager.equippedSpellsUIBar.UpdateEquippedSpellsImage(equippedSpell.spellImage, 0);
            Debug.Log("EQUIPTED SPELL" + equippedSpell);
        }
        else
        {
            Debug.LogError("Invalid index: " + index);
        }
    }


    protected void SpawnSpellTwordsTargetIfPossible()
    {

    }


    public virtual void SpawnSpellRightHand()
    {
        if (!IsOwner) return;
        if (equippedSpell == null) return;

        Vector3 crosshairScreenPosition = PlayerUIManager.instance.playerUIHudManager.crosshair.transform.position;
        Camera mainCamera = PlayerCamera.instance.GetComponentInChildren<Camera>();
        Vector3 crosshairViewportPosition = mainCamera.ScreenToViewportPoint(crosshairScreenPosition);

        Ray ray = mainCamera.ViewportPointToRay(crosshairViewportPosition); // Points to the position of the UI crosshair element
        Vector3 cameraDirection = ray.direction;

        var target = PlayerCamera.instance.player.playerCombatManager?.currentTarget?.characterCombatManager?.lockOnTransform;
        if (target)
        {
            PlayerUIManager.instance.playerUIHudManager.ToggleCrosshairOff(); // Turn off Crosshair
            cameraDirection = (target.position - rightHand.position).normalized;
        }

        if (equippedSpell.spawnAtSelf)
        {
            equippedSpell.SpawnSpell(this, midPoint.position, cameraDirection);
        }
        if (equippedSpell.spawnAtTarget)
        {
            RaycastHit hit;
            if (target)
            {
                if(Physics.Raycast(target.position, Vector3.down, out hit, 100f))
                {
                    equippedSpell.SpawnSpell(this, hit.point, cameraDirection);
                }
            } else
            {
                // Perform the raycast
                if (Physics.Raycast(ray, out hit))
                {
                    equippedSpell.SpawnSpell(this, hit.point, cameraDirection);
                }
            }
        }
        if (equippedSpell.spawnRightHand)
        {
            equippedSpell.SpawnSpell(this, rightHand.position, cameraDirection);
        }
        if (equippedSpell.spawnLeftHand)
        {
            equippedSpell.SpawnSpell(this, leftHand.position, cameraDirection);
        }
        if (equippedSpell.spawnMidpoint)
        {
            equippedSpell.SpawnSpell(this, midPoint.position, cameraDirection);
        }

        equippedSpell = null;
        //if (equippedSpells.Count > 0)
        //{
        //    equippedSpells.RemoveAt(0);
        //    PlayerUIManager.instance.playerUIHudManager.equippedSpellsUIBar.UpdateEquippedSpellsImage(nullSpellImage, 0);
        //    var target = PlayerCamera.instance.player.playerCombatManager?.currentTarget?.characterCombatManager?.lockOnTransform;
        //    if (target)
        //    {
        //        PlayerUIManager.instance.playerUIHudManager.ToggleCrosshairOff(); // Turn off Crosshair
        //        Vector3 directionToWorldPointX = (target.position - rightHand.position).normalized;
        //        equippedSpell.SpawnSpell(this, rightHand, directionToWorldPointX);
        //    }
        //    else
        //    {
        //        PlayerUIManager.instance.playerUIHudManager.ToggleCrosshairOn(); // Turn on Crosshair

        //        Vector3 crosshairScreenPosition = PlayerUIManager.instance.playerUIHudManager.crosshair.transform.position;
        //        Camera mainCamera = PlayerCamera.instance.GetComponentInChildren<Camera>();
        //        Vector3 crosshairViewportPosition = mainCamera.ScreenToViewportPoint(crosshairScreenPosition);

        //        Ray ray = mainCamera.ViewportPointToRay(crosshairViewportPosition); // Points to the position of the UI crosshair element
        //        Vector3 cameraDirection = ray.direction;

        //        equippedSpell.SpawnSpell(this, rightHand, cameraDirection);
        //        //Vector3 cameraForward = PlayerCamera.instance.transform.forward;
        //        //equippedSpell.SpawnSpell(this, rightHand, cameraForward);
        //    }
        //}
        //else
        //{
        //    PlayerUIManager.instance.playerUIPopUpManager.SendMissingSpellErrorPopUp();
        //}
    }


    public virtual void SpawnSpellLeftHand()
    {
        if (equippedSpell != null)
        {
            equippedSpell.SpawnSpell(this, leftHand.position, character.gameObject.transform.forward);
        }
    }

    public virtual void SpawnSpellMidpoint()
    {
        if (equippedSpell != null)
        {
            equippedSpell.SpawnSpell(this, midPoint.position, character.gameObject.transform.forward);
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
