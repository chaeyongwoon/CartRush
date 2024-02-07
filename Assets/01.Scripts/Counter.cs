using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : Auto
{




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _player.state != Player_Move.State.Auto)
        {
            Fix_Player(other.transform, Player_Move.State.Auto);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player.Recovery(false);
        }
    }



}
