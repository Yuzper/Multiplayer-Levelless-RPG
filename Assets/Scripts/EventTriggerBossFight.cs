using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerBossFight : MonoBehaviour
{

    [SerializeField] private int bossID;

    private void OnTriggerEnter(Collider other)
    {
        WorldAIManager.instance.GetBossCharacterByID(bossID)?.WakeBoss();
    }

}
