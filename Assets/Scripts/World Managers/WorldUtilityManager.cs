using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUtilityManager : MonoBehaviour
{
    public static WorldUtilityManager instance;

    [Header("Layers")]
    [SerializeField] LayerMask characterLayers;
    [SerializeField] LayerMask enviromentalLayers;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public LayerMask GetCharacterLayers()
    {
        return characterLayers;
    }

    public LayerMask GetEnviromentalLayers()
    {
        return enviromentalLayers;
    }

    public bool CanIDamageThisTarget(CharacterGroup attackingCharacter, CharacterGroup targetCharacter)
    {
        if(attackingCharacter == CharacterGroup.Team01)
        {
            switch(targetCharacter)
            {
                case CharacterGroup.Team01: return false;
                case CharacterGroup.Team02: return true;
                default: 
                    break;
            }
        } 
        else if (attackingCharacter == CharacterGroup.Team02)
        {
            switch (targetCharacter)
            {
                case CharacterGroup.Team01: return true;
                case CharacterGroup.Team02: return false;
                default:
                    break;
            }
        }

        return false;
    }

    public float GetAngleOfTarget(Transform characterTransform, Vector3 targetsDirection)
    {
        targetsDirection.y = 0f;
        float viewableAngle = Vector3.Angle(characterTransform.forward, targetsDirection);
        Vector3 cross = Vector3.Cross(characterTransform.forward, targetsDirection);

        if (cross.y < 0f) viewableAngle = -viewableAngle;

        return viewableAngle;
    }
}
