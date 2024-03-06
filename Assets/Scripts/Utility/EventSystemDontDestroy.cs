using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystemDontDestroy : MonoBehaviour
{
    public static EventSystemDontDestroy instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
