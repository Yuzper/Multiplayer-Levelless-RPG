using UnityEngine;
using UnityEngine.SceneManagement;

public class Box_Collision_Script : MonoBehaviour
{
    Scene currentScene;

    public void OnTriggerEnter(Collider other)
    {
        currentScene = SceneManager.GetActiveScene();

        if (this.gameObject.CompareTag("Teleport Platform") && other.CompareTag("Player"))
        {
            // TELEPORT PLATFORM LOGIC
            Debug.Log("TELEPORT PLATFORM");
            other.transform.position = new Vector3(-4, 41, -8);
        }
        else if (other.CompareTag("Player"))
        {
            // TELEPORT WORLD LOGIC
            Debug.Log("TELEPORT WORLD");

            if (currentScene.name == "Scene_World_01")
            {
                SceneManager.LoadScene("Scene_World_02");
            }
            else if (currentScene.name == "Scene_World_02")
            {
                SceneManager.LoadScene("Scene_World_01");
            }
        }
    }
}
