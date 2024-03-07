using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility_Damage_Text : MonoBehaviour
{
    [SerializeField] float timeUntilDestroyed = 5f;

    public Vector3 RandomizeIntensity = new Vector3(0.5f, 0.5f, 0.5f);

    private void Awake()
    {
        transform.localPosition += new Vector3(Random.Range(-RandomizeIntensity.x, RandomizeIntensity.x),
                                                Random.Range(RandomizeIntensity.y * 3, RandomizeIntensity.y * 6), // Not in minus since the number should be slighty above head.
                                                Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z));

        Destroy(gameObject, timeUntilDestroyed);
    }

    public void Update()
    {
        Vector3 lookAtRotation = Quaternion.LookRotation(PlayerCamera.instance.transform.position - transform.position).eulerAngles;
        transform.rotation = Quaternion.Euler(0, lookAtRotation.y + 180, lookAtRotation.z);
    }

}
