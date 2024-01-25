using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellsManager : CharacterSpellsManager
{
    PlayerManager player;
    public GameObject StoneGolemPrefab;
    public GameObject StoneGolemPrefabVFX;
    public float detectionRadius = 5f;
    Vector3 summonPosition;


    public void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    public void ChooseStone()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float maxRaycastDistance = 1000f;
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRaycastDistance) && hit.collider.CompareTag("Stone"))
        {
            // Check if the hit object has the HitNotifier script attached
            StonesSummoning stonesSummoning = hit.collider.GetComponent<StonesSummoning>();
            if (stonesSummoning != null)
            {
                // Notify the hit object
                stonesSummoning.ActivateSummon(player.transform.position);
                summonPosition = player.transform.position;
            }
        }
    }

    public void SummonGolem()
    {
        Collider[] colliders = Physics.OverlapSphere(summonPosition, detectionRadius);

        int stoneCount = 0;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Stone"))
            {
                stoneCount++;

                // You may perform additional actions here if needed for each "Stone" object
            }
        }

        // Check if there are at least 5 "Stone" objects within the radius
        if (stoneCount >= 4)
        {
            float offsetY = 1f;
            summonPosition.y = summonPosition.y + offsetY;

            Instantiate(StoneGolemPrefab, summonPosition, Quaternion.identity);
            Instantiate(StoneGolemPrefabVFX, transform.position, Quaternion.identity);
        }
    }

    // Visualize the detection radius in the scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            ChooseStone();
            SummonGolem();

        }




    }
}
