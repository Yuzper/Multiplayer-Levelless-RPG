using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFXManager : CharacterVFXManager
{
    PlayerManager player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }


}
