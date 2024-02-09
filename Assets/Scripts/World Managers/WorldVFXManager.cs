using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldVFXManager : MonoBehaviour
{
    public static WorldVFXManager instance;

    [Header("Landing Dust")]
    [SerializeField] GameObject LandingDustVFX;
    


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

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public GameObject ChooseRandomVFXFromArray(GameObject[] array)
    {
        int index = Random.Range(0, array.Length);
        return array[index];
    }


    // Landing dust
    public void PlayLandingDustVFX(Vector3 contactpoint, Quaternion contactRotation)
    {
        GameObject LandingDustVFX = Instantiate(WorldVFXManager.instance.LandingDustVFX, contactpoint, contactRotation);
    }


}
