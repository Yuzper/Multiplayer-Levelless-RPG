using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Interactable : NetworkBehaviour
{
    [Tooltip("Text prompt when entering the interaction collider")]
    public string interactableText;

    [SerializeField] protected Collider interactableCollider;

    [Tooltip("When enabled, object cannot be interacted with by co-up players")]
    [SerializeField] protected bool hostOnlyInteractable = false;



    protected virtual void Awake()
    {
        if(interactableCollider == null)
        {
            interactableCollider = GetComponent<Collider>();
        }
    }

    protected virtual void Start()
    {

    }

    public virtual void Interact(PlayerManager player)
    {
        if (!player.IsOwner) return;
        Debug.Log("YOU INTERACTED!");

        interactableCollider.enabled = false;
        player.playerInteractionManager.RemoveInteractionFromList(this);
        PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();


        
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        PlayerManager player = other.GetComponent<PlayerManager>();

        if (player != null)
        {
            if (!player.playerNetworkManager.IsHost && hostOnlyInteractable) return;

            if(!player.IsOwner) return;

            player.playerInteractionManager.AddInteractionToList(this);


        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        PlayerManager player = other.GetComponent<PlayerManager>();

        if (player != null)
        {
            if (!player.playerNetworkManager.IsHost && hostOnlyInteractable) return;

            if (!player.IsOwner) return;

            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
            
        }
    }
}
