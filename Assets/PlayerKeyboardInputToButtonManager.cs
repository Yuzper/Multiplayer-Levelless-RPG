using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerKeyboardInputToButtonManager : MonoBehaviour
{
    PlayerManager player;

    // Action bools
    public bool danceInput = false;


    protected void Awake()
    {
        
    }


    public void DanceButton()
    {
        danceInput = true;
        PlayerUIManager.instance.playerUIHudManager.Button_Z.onClick.Invoke();
        danceInput = false;
    }

    public void ReviveButton()
    {
        PlayerUIManager.instance.playerUIHudManager.Button_R.onClick.Invoke();
    }


}
