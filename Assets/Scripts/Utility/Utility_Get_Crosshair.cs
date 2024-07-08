using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility_Get_Crosshair : MonoBehaviour
{
    [SerializeField] public GameObject crosshairObject;
    public void ToggleCrosshairOn()
    {
        crosshairObject.SetActive(true);    
    }
    public void ToggleCrosshairOff()
    {
        crosshairObject.SetActive(false);
    }

}
