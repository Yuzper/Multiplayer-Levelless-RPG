using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonesSummoning : MonoBehaviour
{

    public float moveSpeed = 3f;
    bool activateSummon = false;

    // Update is called once per frame
    void Update()
    {
       // MoveObjectTowards();
    }

    public void ActivateSummon()
    {
        activateSummon = true;
    }

    private void MoveObjectTowards(Transform objTransform, Vector3 targetPosition)
    {
        // Calculate the direction towards the target
        Vector3 direction = (targetPosition - objTransform.position).normalized;

        // Move the object towards the target
        objTransform.position = Vector3.MoveTowards(objTransform.position, targetPosition, moveSpeed * Time.deltaTime);
    }


}


