using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystemDontDestroy : MonoBehaviour
{
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
