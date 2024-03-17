using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFootStepSFXMaker : MonoBehaviour
{
    CharacterManager character;

    AudioSource audioSource;
    GameObject steppedOnObject;

    private bool hasTouchedGround = false;
    private bool hasPlayedFootStepSFX = false;
    [SerializeField] float distanceToGround = 0.05f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        character = GetComponentInParent<CharacterManager>();
    }

    private void FixedUpdate()
    {
        CheckForFootSteps();
    }

    void CheckForFootSteps()
    {
        if (character == null) return;

        if (!character.characterNetworkManager.isMoving.Value) return;

        RaycastHit hit;

        // Draw the ray in the Scene view and log the distance
        Vector3 direction = character.transform.TransformDirection(Vector3.down);
        Debug.DrawRay(transform.position, direction * distanceToGround, Color.green, 10.0f);

        if (Physics.Raycast(transform.position, character.transform.TransformDirection(Vector3.down), out hit, distanceToGround, WorldUtilityManager.instance.GetEnviromentalLayers()))
        {
            hasTouchedGround = true;
            Debug.Log("Hit " + hit.transform.gameObject.name);

            if(!hasPlayedFootStepSFX)
            {
                steppedOnObject = hit.transform.gameObject;
            } 
            else
            {
                hasTouchedGround = false;
                hasPlayedFootStepSFX = false;
                steppedOnObject = null;
            }

            if(hasTouchedGround && !hasPlayedFootStepSFX)
            {
                Debug.Log("PLAYED SOUND");
                hasPlayedFootStepSFX = true;
                PlayFootstepSoundFX();
            }
        }

    }

    private void PlayFootstepSoundFX()
    {
        // method 1
        // different sfx depending on layer of the ground tag (stone, dirt, snow...)
        //audioSource.PlayOneShot(WorldSoundFXManager.instance.ChooseRandomFootstepSoundBasedOnGround(steppedOnObject, character));
        
        // method 2 (Simple)
        character.characterSoundFXManager.PlayFootstep();
    }
}
