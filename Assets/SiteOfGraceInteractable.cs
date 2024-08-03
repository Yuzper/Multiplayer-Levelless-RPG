using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SiteOfGraceInteractable : Interactable
{
    [Header("Site of Grace info")]
    [SerializeField] int siteOfGraceID;

    [Header("VFX")]
    [SerializeField] GameObject particles;

    [Header("Activated")]
    public NetworkVariable<bool> isActivated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Interaction Text")]
    [SerializeField] string unactivatedInteractionText = "Restore Site of Grace";
    [SerializeField] string activatedInteractionText = "Rest";

    // TODO MAKE SITE OF GRACE WORK ON ALL CLIENTS - NOT JUST THE HOST
    protected override void Start()
    {
        base.Start();

        if (IsOwner)
        {
            if (WorldSaveGameManager.instance.currentCharacterData.siteOfGrace.ContainsKey(siteOfGraceID))
            {
                isActivated.Value = WorldSaveGameManager.instance.currentCharacterData.siteOfGrace[siteOfGraceID];
            }
            else
            {
                isActivated.Value = false;
            }
        }

        if(isActivated.Value)
        {
            interactableText = activatedInteractionText;
        } else
        {
            interactableText = unactivatedInteractionText;
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // if we join when a site has been activated, we force the onchange function to run here upon joining
        if (!IsOwner)
        {
            OnIsActivatedChanged(false, isActivated.Value);
        }

        isActivated.OnValueChanged += OnIsActivatedChanged;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkSpawn();

        isActivated.OnValueChanged -= OnIsActivatedChanged;
    }

    private void RestoreSiteOfGrace(PlayerManager player)
    {
        
        isActivated.Value = true;

        // if our save file contains info on this site, we remove it and then add it as activated
        if (WorldSaveGameManager.instance.currentCharacterData.siteOfGrace.ContainsKey(siteOfGraceID) )
        {
            WorldSaveGameManager.instance.currentCharacterData.siteOfGrace.Remove(siteOfGraceID);
        }
        WorldSaveGameManager.instance.currentCharacterData.siteOfGrace.Add(siteOfGraceID, true);

        player.playerAnimatorManager.PlayerTargetActionAnimation("Active_Site_Of_Grace_01", true);
        // TODO hide weapon models

        PlayerUIManager.instance.playerUIPopUpManager.SendBossDefeatedPopUp("Site of Grace RESTORED");

        StartCoroutine(WaitForAnimationAndPopUpThenRestoreCollider());
    }

    private void RestAtSiteOfGrace(PlayerManager player)
    {
        Debug.Log("resting");
        interactableCollider.enabled = true;

        player.playerNetworkManager.currentHealth.Value = player.playerNetworkManager.maxHealth.Value;
        player.playerNetworkManager.currentStamina.Value = player.playerNetworkManager.maxStamina.Value;
        player.playerNetworkManager.currentMana.Value = player.playerNetworkManager.maxMana.Value;


        // reset monsters, character location
        WorldAIManager.instance.ResetAllCharacters();
    }

    private IEnumerator WaitForAnimationAndPopUpThenRestoreCollider()
    {
        yield return new WaitForSeconds(2); // this should give enough time for animation to play
        interactableCollider.enabled = true;
    }

    private void OnIsActivatedChanged(bool oldStatus, bool newStatus)
    {
        if (isActivated.Value)
        {
            // PLAY FX HERE to light the fire
            particles.SetActive(true);
            interactableText = activatedInteractionText;
        } else
        {
            interactableText = unactivatedInteractionText;
        }


    }


    public override void Interact(PlayerManager player)
    {
        base.Interact(player);

        if(!isActivated.Value)
        {
            RestoreSiteOfGrace(player);
        } else
        {
            RestAtSiteOfGrace(player);
        }
    }
}
