using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    PlayerManager player;

    private List<Interactable> currentInteractableActions;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    private void Start()
    {
        currentInteractableActions = new List<Interactable>();
    }


    private void FixedUpdate()
    {
        if(!player.IsOwner)
        {
            return;
        }

        // if our ui menu is not open, and we dont have a pop up (current interaction message) check for interactable
        if(!PlayerUIManager.instance.menuWindowIsOpen && !PlayerUIManager.instance.popUpWindowIsOpen)
        {
            CheckForInteractable();
        }
    }


    private void CheckForInteractable()
    {
        if(currentInteractableActions.Count == 0)
        {
            return;
        }

        if (currentInteractableActions[0] == null)
        {
            currentInteractableActions.RemoveAt(0);
            return;
        }

        if (currentInteractableActions[0] != null)
        {
            PlayerUIManager.instance.playerUIPopUpManager.SendPlayerMessagePopUp(currentInteractableActions[0].interactableText);
        }
    }

    private void RefreshInteractionList()
    {
        for (int i = currentInteractableActions.Count - 1; i > -1; i--)
        {
            if (currentInteractableActions[i] == null)
            {
                currentInteractableActions.RemoveAt(i);
            }
               
        }
    }

    public void Interact()
    {
        if (currentInteractableActions.Count == 0) return;
        if (currentInteractableActions[0] != null)
        {
            currentInteractableActions[0].Interact(player);
            RefreshInteractionList();
        }
    }

    public void AddInteractionToList(Interactable interactable)
    {
        RefreshInteractionList();

        if (!currentInteractableActions.Contains(interactable))
        {
            currentInteractableActions.Add(interactable);
        }
    }

    public void RemoveInteractionFromList(Interactable interactable)
    {
        if (currentInteractableActions.Contains(interactable))
        {
            currentInteractableActions.Remove(interactable);
        }
        RefreshInteractionList();
    }
}
