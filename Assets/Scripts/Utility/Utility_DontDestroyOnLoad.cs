using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility_DontDestroyOnLoad : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);

    }
}
