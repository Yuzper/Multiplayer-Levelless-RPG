using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonesSummoning : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float rotationSpeed = 180f;
    Vector3 summonPosition;
    bool activateSummon = false;

    // Update is called once per frame
    void Update()
    {
        if (activateSummon)
        {
            MoveObjectTowards(transform, summonPosition);
        }
    }

    // Activates the summon when hit with a raycast to the position it originated from, is not updating.
    public void ActivateSummon(Vector3 rayOrigin)
    {
        activateSummon = true;
        summonPosition = rayOrigin;
    }

    private void MoveObjectTowards(Transform stoneTransform, Vector3 targetPosition)
    {
        // Move the object towards the target
        stoneTransform.position = Vector3.MoveTowards(stoneTransform.position, targetPosition, moveSpeed * Time.deltaTime);

     //   Vector3 direction = (targetPosition - stoneTransform.position).normalized;
    //    Quaternion targetRotation = Quaternion.LookRotation(direction);
    //    stoneTransform.rotation = Quaternion.RotateTowards(stoneTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }


}


