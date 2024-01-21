using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility_Damage_Text : MonoBehaviour
{
    [SerializeField] float timetUntilDestroyed = 5f;
    
    public Vector3 RandomizeIntensity = new Vector3(0.5f, 0, 0);

    private void Awake()
    {
        Destroy(gameObject, timetUntilDestroyed);

        transform.localPosition += new Vector3(Random.Range(-RandomizeIntensity.x, RandomizeIntensity.x),
            Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y),
            Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z));
    }

    public void Update()
    {
        transform.LookAt(PlayerCamera.instance.transform, Vector3.up);
    }

}
