using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaZombieSpawner : MonoBehaviour
{
    public LayerMask characterLayer;  // Set this to the layer where your characters are placed
    public float halfExtents = 1;
    [SerializeField] AICharacterSpawner undeadCharacterSpawner;


    // Function to check if there is at least one undead character within a square
    public bool NeedToRespawnUndeadCharacters()
    {
        Collider[] colliders = Physics.OverlapBox(this.gameObject.transform.position, new Vector3(halfExtents, 1, halfExtents), Quaternion.identity, characterLayer);

        int undeadCount = 0;
        foreach (Collider collider in colliders)
        {
            AICharacterManager AIcharacter = collider.GetComponent<AICharacterManager>();

            if (AIcharacter != null && !AIcharacter.isDead.Value)
            {
                undeadCount += 1;
            }
        }

        if (undeadCount < 5)
        {
            return true;  // No undead characters found in the square
        }
        else
        {
            return false;  // Found an undead character
        }
        
    }

    public void SpawnUndead()
    {
        float randomX = Random.Range(-1f, 1f); // Adjust the range as needed
        float randomZ = Random.Range(-1f, 1f); // Adjust the range as needed
        Vector3 randomOffset = new Vector3(randomX, 0f, randomZ);
        Vector3 spawnPosition = transform.position + randomOffset;

        Instantiate(undeadCharacterSpawner, spawnPosition, Quaternion.identity);
      //  Debug.Log("Spawn Undead");
    }


    private void Update()
    {
        if (NeedToRespawnUndeadCharacters())
        {
            SpawnUndead();
        }
    }

    // Draw Gizmos in the Unity editor
    private void OnDrawGizmos()
    {
        DrawOverlapBoxGizmo();
    }

    private void OnDrawGizmosSelected()
    {
        DrawOverlapBoxGizmo();
    }

    // Helper method to draw Gizmo for the overlap box
    private void DrawOverlapBoxGizmo()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

        // Use the same dimensions as in the OverlapBox function
        Vector3 boxSize = new Vector3(halfExtents, 1f, halfExtents);

        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}


