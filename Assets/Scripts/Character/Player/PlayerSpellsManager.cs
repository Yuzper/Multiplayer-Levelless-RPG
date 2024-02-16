using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellsManager : CharacterSpellsManager
{
    PlayerManager player;


    public void Awake()
    {
        player = GetComponent<PlayerManager>();
    }




}
